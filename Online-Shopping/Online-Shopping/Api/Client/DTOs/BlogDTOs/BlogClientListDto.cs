using Online_Shopping.Api.Manage.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Client.DTOs
{
    public class BlogClientListDto
    {
        public List<BlogClientItemDto> Blogs { get; set; }
        public int TotalCount { get; set; }
    }
    public class BlogClientItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string Photo { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}

