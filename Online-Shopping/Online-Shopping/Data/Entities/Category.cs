﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }

        [Range(minimum: 1, maximum: int.MaxValue)]
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public List<SubCategory> SubCategories { get; set; }
    }
}
