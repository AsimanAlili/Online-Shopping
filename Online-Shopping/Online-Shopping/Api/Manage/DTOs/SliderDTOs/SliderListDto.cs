using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class SliderListDto
    {
        public List<SliderItemDto> Sliders { get; set; }
        public int TotalCount { get; set; }
    }
    public class SliderItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public string RedirectUrl { get; set; }
        public string Photo { get; set; }
    }
}
