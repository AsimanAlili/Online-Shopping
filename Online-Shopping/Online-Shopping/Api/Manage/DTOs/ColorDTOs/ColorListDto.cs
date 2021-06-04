using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class ColorListDto
    {
        public List<ColorItemDto> Colors { get; set; }
        public int TotalCount { get; set; }
    }
    public class ColorItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
