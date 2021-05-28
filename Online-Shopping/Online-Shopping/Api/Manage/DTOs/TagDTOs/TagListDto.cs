using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class TagListDto
    {
        public List<TagItemDto> Tags { get; set; }
        public int TotalCount { get; set; }

    }
    public class TagItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
