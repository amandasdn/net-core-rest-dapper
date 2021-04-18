using Project.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.Entities
{
    public class Product : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public ProductType Type { get; set; }

        public ProductImage Image { get; set; } = new ProductImage();

        public Category Category { get; set; }
    }

    public class ProductImage
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Base64 { get; set; }
    }
}
