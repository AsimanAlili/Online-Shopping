using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class SliderGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public string Photo { get; set; }
        public string RedirectUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
