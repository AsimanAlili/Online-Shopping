using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Entities
{
    public class Slider:BaseEntity
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string RedirectUrl { get; set; }
        public string Photo { get; set; }
        public int Order { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
    }
}
