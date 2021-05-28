using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class BrandListDto
    {
        public List<BrandItemDto> Brands { get; set; }
        public int TotalCount { get; set; }
    }
    public class BrandItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
