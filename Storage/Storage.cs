using ChekRAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChekRAO.DBManager;

namespace ChekRAO.Storage
{
    public static class Storage
    {
        public static List<Company> Companies;
        public static List<Op> Ops;
        public static List<Op> ExportOps;
        public static DBManager.DBManager DBManager;
        public static Dictionary<int, Company> IdfToComp;

        public static void Initialize() 
        {
            Companies = new List<Company>();
            Ops = new List<Op>();
            ExportOps = new List<Op>();
            IdfToComp = new Dictionary<int, Company>();
            DBManager = new DBManager.DBManager();
            DBManager.Initialize();
        }

        public static void Dispatch() 
        {
            DBManager.Dispatch();
            ExportOps = null;
            IdfToComp = null;
            DBManager = null;
            Companies = null;
            Ops = null;
        }
    }
}
