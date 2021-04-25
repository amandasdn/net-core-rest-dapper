using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.Entities
{
    public class AppSettings
    {
        public string Secret { get; set; }

        public int Expiration { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
