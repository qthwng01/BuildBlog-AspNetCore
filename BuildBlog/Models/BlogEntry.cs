using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildBlog.Models
{
    public class BlogEntry
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
    }
}
