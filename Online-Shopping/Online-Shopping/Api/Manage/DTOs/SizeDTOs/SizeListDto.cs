using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class SizeListDto
    {
        public List<SizeItemDto> Sizes { get; set; }
        public int TotalCount { get; set; }

    }
    public class SizeItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

    }
}
