using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.DTOs
{
    public class FeatureListDto
    {
        public List<FeatureItemDto> Features { get; set; }
        public int TotalCount { get; set; }
    }
    public class FeatureItemDto
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public int Order { get; set; }
    }
}
