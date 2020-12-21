using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekRAO.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OKPO { get; set; }

        public Company(int _id, string _name, string _okpo) 
        {
            Id = _id;
            Name = _name;
            OKPO = _okpo;
        }
    }
}
