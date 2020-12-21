using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekRAO.Models
{
    public class Op
    {
        public bool IsSelected { get; set; }
        public int Id { get; set; }
        public int Idf { get; set; }
        public Int16 OpCode { get; set; }
        public bool OpCode_Type { get; set; }
        public DateTime OpDate { get; set; }
        public string RAOCode { get; set; }
        public double Kbm { get; set; }
        public double Kg { get; set; }
        public string Nuclid { get; set; }
        public DateTime ActDate { get; set; }
        public Int16 DocVid { get; set; }
        public string DocN { get; set; }
        public DateTime DocDate { get; set; }
        public string OkpoPIP { get; set; }
        public string OkpoPrv { get; set; }
        public string UktPrTyp { get; set; }
        public string UktPrN { get; set; }
        public Company MainCompany { get; set; }
        public Company SlaveCompany { get; set; }
        public bool IsUsed { get; set; }

        public Op(int _id, int _idf, Int16 _opcode, bool _opCode_type, DateTime _opDate, string _raocode, double _kbm, double _kg, string _nuclid, DateTime _actDate,
            Int16 _docVid, string _docN, DateTime _docDate, string _okpoPIP, string _okpoPRV, string _uktPrTyp, string _uktPrN) 
        {
            Id = _id;
            Idf = _idf;
            OpCode = _opcode;
            OpCode_Type = _opCode_type;
            OpDate = _opDate;
            RAOCode = _raocode;
            Kbm = _kbm;
            Kg = _kg;
            Nuclid = _nuclid;
            ActDate = _actDate;
            DocVid = _docVid;
            DocN = _docN;
            DocDate = _docDate;
            OkpoPIP = _okpoPIP;
            OkpoPrv = _okpoPRV;
            UktPrTyp = _uktPrTyp;
            UktPrN = _uktPrN;
            MainCompany = Storage.Storage.IdfToComp[Idf];

            foreach (Company com in Storage.Storage.Companies) 
            {
                if (com.OKPO == OkpoPIP) 
                {
                    SlaveCompany = com;
                    break;
                }
            }
        }
    }
}
