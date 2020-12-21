using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChekRAO.Models;
using Microsoft.Office.Interop.Excel;

namespace ChekRAO
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Storage.Storage.Initialize();
            Post_List.ItemsSource = Storage.Storage.Companies;
            Pol_List.ItemsSource = Storage.Storage.Companies;
            //OpsList.ItemsSource = Storage.Storage.ExportOps;
            StartDate.SelectedDate = DateTime.MinValue;
            EndDate.SelectedDate = DateTime.Now;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Storage.Storage.Dispatch();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Storage.Storage.ExportOps.Clear();
            OpsList.Items.Clear();

            List<Op> Post_Ops = Storage.Storage.Ops.Where(x => x.OpCode_Type == true).Where(x => Storage.Storage.IdfToComp[x.Idf].Id == (Post_List.SelectedItem as Company).Id).Where(x => (Pol_List.SelectedItem as Company).OKPO == x.OkpoPIP).Where(x => x.OpDate >= StartDate.SelectedDate).Where(x => x.OpDate <= EndDate.SelectedDate).ToList();
            List<Op> Pol_Ops = Storage.Storage.Ops.Where(x => x.OpCode_Type == false).Where(x => Storage.Storage.IdfToComp[x.Idf].Id == (Pol_List.SelectedItem as Company).Id).Where(x => (Post_List.SelectedItem as Company).OKPO == x.OkpoPIP).Where(x => x.OpDate >= StartDate.SelectedDate).Where(x => x.OpDate <= EndDate.SelectedDate).ToList();

            foreach (Op op in Post_Ops) 
            {
                bool allIsGood = true;
                foreach(Op sec_op in Pol_Ops)
                {
                    if (op.DocVid == sec_op.DocVid && op.UktPrN == sec_op.UktPrN && op.DocN == sec_op.DocN && op.OpDate == sec_op.OpDate && !op.IsUsed && !sec_op.IsUsed) 
                    {
                        if ((bool)Check_RAOCod.IsChecked)
                        {
                            if (op.RAOCode != sec_op.RAOCode) 
                            {
                                allIsGood = false;
                            }
                        }

                        if ((bool)Check_Kbm.IsChecked)
                        {
                            if (op.Kbm != sec_op.Kbm)
                            {
                                allIsGood = false;
                            }
                        }

                        if ((bool)Check_Kg.IsChecked)
                        {
                            if (op.Kg != sec_op.Kg)
                            {
                                allIsGood = false;
                            }
                        }

                        if (!allIsGood)
                        {
                            Storage.Storage.ExportOps.Add(op);
                            foreach (Op close_op in Post_Ops)
                            {
                                if (op.Id == close_op.Id)
                                {
                                    op.IsUsed = true;
                                    break;
                                }
                            }
                            Storage.Storage.ExportOps.Add(sec_op);

                            foreach (Op close_op in Pol_Ops)
                            {
                                if (sec_op.Id == close_op.Id)
                                {
                                    op.IsUsed = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }

            }

            foreach (Op op in Post_Ops)
            {
                if (!op.IsUsed)
                {
                    Storage.Storage.ExportOps.Add(op);
                }
            }
            foreach (Op op in Pol_Ops)
            {
                if (!op.IsUsed)
                {
                    Storage.Storage.ExportOps.Add(op);
                }
            }
            OpsList.Items.Clear();
            foreach (Op op in Storage.Storage.ExportOps)
            {
                OpsList.Items.Add(op);
            }
            MessageBox.Show("Поиск завершен");
        }

        private void ClearSelect_Click(object sender, RoutedEventArgs e)
        {
            foreach (Op op in Storage.Storage.ExportOps) 
            {
                op.IsSelected = false;
            }

            OpsList.Items.Clear();
            foreach (Op op in Storage.Storage.ExportOps)
            {
                OpsList.Items.Add(op);
            }
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (Op op in Storage.Storage.ExportOps)
            {
                op.IsSelected = true;
            }

            OpsList.Items.Clear();
            foreach (Op op in Storage.Storage.ExportOps)
            {
                OpsList.Items.Add(op);
            }
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application Excel;
            _Workbook Workbook;
            _Worksheet WorkSheet;
            Range Range;
            object misvalue = System.Reflection.Missing.Value;

            Excel = new Microsoft.Office.Interop.Excel.Application();
            Excel.Visible = true;

            Workbook = Excel.Workbooks.Add("");
            WorkSheet = Workbook.ActiveSheet;

            WorkSheet.Cells[1, 1] = "Имя компании";
            WorkSheet.Cells[1, 2] = "Код операции";
            WorkSheet.Cells[1, 3] = "Дата операции";
            WorkSheet.Cells[1, 4] = "Код RAO";
            WorkSheet.Cells[1, 5] = "Объем";
            WorkSheet.Cells[1, 6] = "Вес";
            WorkSheet.Cells[1, 7] = "Нуклид";
            WorkSheet.Cells[1, 8] = "Дата активности";
            WorkSheet.Cells[1, 9] = "Вид документа";
            WorkSheet.Cells[1, 10] = "Номер документа";
            WorkSheet.Cells[1, 11] = "Дата документа";
            WorkSheet.Cells[1, 12] = "ОКПО Поставщика";
            WorkSheet.Cells[1, 13] = "ОКПО Перевозчика";
            WorkSheet.Cells[1, 14] = "Тип контейнера";
            WorkSheet.Cells[1, 15] = "Номер контейнера";

            int i = 2;

            foreach (Op op in OpsList.Items) 
            {
                if (op.IsSelected) 
                {
                    WorkSheet.Cells[i, 1] = op.MainCompany.Name.ToString();
                    WorkSheet.Cells[i, 2] = op.OpCode.ToString();
                    WorkSheet.Cells[i, 3] = op.OpDate.ToString();
                    WorkSheet.Cells[i, 4] = op.RAOCode.ToString();
                    WorkSheet.Cells[i, 5] = op.Kbm.ToString();
                    WorkSheet.Cells[i, 6] = op.Kg.ToString();
                    WorkSheet.Cells[i, 7] = op.Nuclid.ToString();
                    WorkSheet.Cells[i, 8] = op.ActDate.ToString();
                    WorkSheet.Cells[i, 9] = op.DocVid.ToString();
                    WorkSheet.Cells[i, 10] = op.DocN.ToString();
                    WorkSheet.Cells[i, 11] = op.DocDate.ToString();
                    WorkSheet.Cells[i, 12] = op.OkpoPIP.ToString();
                    WorkSheet.Cells[i, 13] = op.OkpoPrv.ToString();
                    WorkSheet.Cells[i, 14] = op.UktPrTyp.ToString();
                    WorkSheet.Cells[i, 15] = op.UktPrN.ToString();
                    i++;
                }
            }



            Workbook.SaveAs("Отчет.xlsx");

            Excel.Visible = false;
            Workbook.Close();
            Excel.Quit();

            MessageBox.Show("Отчет готов");
        }
    }
}
