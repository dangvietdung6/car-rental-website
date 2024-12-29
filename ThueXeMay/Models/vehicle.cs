using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ThueXeMay.Models
{
    public partial class vehicle
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }


        public Nullable<System.DateTime> DateEnd { get; set; }


        public Nullable<int> Amount { get; set; }
    }
}