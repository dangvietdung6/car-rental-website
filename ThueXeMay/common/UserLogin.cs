﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThueXeMay.Common
{
    [Serializable]
    public class UserLogin
    {
        public int id_user { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string phoneNumber { get; set; }
    }
}