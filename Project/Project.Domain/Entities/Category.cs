using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.Entities
{
    public class Category : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
