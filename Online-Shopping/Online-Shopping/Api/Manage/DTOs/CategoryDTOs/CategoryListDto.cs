﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class CategoryListDto
    {
        public List<CategoryItemDto> Categories { get; set; }
        public int TotalCount { get; set; }
    }

    public class CategoryItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string Desc { get; set; }
        public string Photo { get; set; }
    }
}
