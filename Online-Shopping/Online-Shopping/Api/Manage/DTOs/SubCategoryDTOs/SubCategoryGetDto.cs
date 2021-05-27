using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class SubCategoryGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

    }
    public class CategoryInSubDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }
   
}
