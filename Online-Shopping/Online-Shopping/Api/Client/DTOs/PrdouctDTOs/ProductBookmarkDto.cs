using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Client.DTOs
{
    public class ProductBookmarkDto
    {
        public int Id { get; set; }
        public bool IsBookmarked { get; set; }
    }
}
