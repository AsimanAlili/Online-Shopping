﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Entities
{
    public class ProductColor
    {
        public int Id { get; set; }
        public bool IsAvailableColor { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public Product Product { get; set; }
        public Color Color { get; set; }
        //public int ColorCount { get; set; }
    }
}
