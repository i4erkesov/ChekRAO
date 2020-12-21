using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChekRAO.Models;

namespace ChekRAO.DBManager
{
    public class DBManager
    {
        private string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=RAO+ROZ.mdb";
        private string commandString;

        private OleDbConnection connection;
        private OleDbCommand command;
        private OleDbDataReader reader;
        

        public void Initialize() 
        {
            connection = new OleDbConnection(connectionString);
            connection.Open();
            GetCompanies();
            GetIdfsToCompany();
            GetOps();
        }

        public void GetCompanies() 
        {
            commandString = "SELECT `ID`, `NAME_FULL`, `OKPO` FROM SprORG";
            command = connection.CreateCommand();
            command.CommandText = commandString;
            reader = command.ExecuteReader();

            while (reader.Read()) 
            {
                Storage.Storage.Companies.Add(new Company((int)reader["ID"],(string)reader["NAME_FULL"], (string)reader["OKPO"]));
            }
            reader.Close();
        }

        public void GetIdfsToCompany() 
        {
            commandString = "SELECT `ID`, `IDP` FROM FORMP WHERE FORM_NAME = 'RAO'";
            command = connection.CreateCommand();
            command.CommandText = commandString;
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                int companyId = (int)reader["IDP"];
                Storage.Storage.IdfToComp.Add((int)reader["ID"], Storage.Storage.Companies.Single(rec => rec.Id == companyId));
            }
            reader.Close();
        }

        public void GetOps() 
        {
            commandString = "SELECT `ID`, `IDF`, `OpCod`, `OpDate`, `RAOCod`, `Kbm`, `Kg`, `Nuclid`, `ActDate`, " +
                "`DocN`, `DocVid`, `DocDate`, `OkpoPIP`, `OkpoPrv`, `UktPrTyp`, `UktPrN` FROM RAO";
            command = connection.CreateCommand();
            command.CommandText = commandString;
            reader = command.ExecuteReader();

            int[] PostOpCodes = new int[3] { 11,28,38 };

            while (reader.Read())
            {

                Storage.Storage.Ops.Add( 
                    new Op 
                    (
                        (int)reader["ID"],
                        (int)reader["IDF"],
                        (Int16)reader["OpCod"],
                        PostOpCodes.Contains((Int16)reader["OpCod"]),
                        (DateTime)reader["OpDate"],
                        (string)reader["RAOCod"],
                        (double)reader["Kbm"],
                        (double)reader["Kg"],
                        (string)reader["Nuclid"],
                        (DateTime)reader["ActDate"],
                        (Int16)reader["DocVid"],
                        (string)reader["DocN"],
                        (DateTime)reader["DocDate"],
                        (string)reader["OkpoPIP"],
                        (string)reader["OkpoPrv"],
                        (string)reader["UktPrTyp"],
                        (string)reader["UktPrN"]
                    )

                    );
            }
            reader.Close();
        }

        public void Dispatch() 
        {
            connection.Close();
            command = null;
            connection = null;
            reader = null;
        }
    }
}
