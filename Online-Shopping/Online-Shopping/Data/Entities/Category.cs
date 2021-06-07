using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public string Desc { get; set; }

        [Range(minimum: 1, maximum: int.MaxValue)]
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public string Photo { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
        public List<SubCategory> SubCategories { get; set; }
    }
}
