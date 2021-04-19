using Newtonsoft.Json;
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

        public ProductImage Image { get; set; }

        public Category Category { get; set; }
    }

    public class ProductImage
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Base64 { get; set; }

        public void SetImage(string name, string type, string base64)
        {
            Name = name;
            Type = type;
            Base64 = base64;
        }
    }

    public class ProductRequest
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public decimal UnitPrice { get; set; }
        
        public int Quantity { get; set; }
        
        public ProductType Type { get; set; }

        public int CategoryId { get; set; }
    }
}
