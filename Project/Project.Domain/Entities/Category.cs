using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Domain.Entities
{
    public class Category : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; } = string.Empty;
    }

    public class CategoryRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
