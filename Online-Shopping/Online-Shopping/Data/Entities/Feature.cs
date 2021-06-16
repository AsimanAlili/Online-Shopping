using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Entities
{
    public class Feature:BaseEntity
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }

        [Range(minimum: 1, maximum: int.MaxValue)]
        public int Order { get; set; }
    }
}
