﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Entities
{
    public class Contact:BaseEntity
    {
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Support { get; set; }
    }
}
