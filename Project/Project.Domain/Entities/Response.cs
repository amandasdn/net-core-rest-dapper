using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.Entities
{
    public class Response<T>
    {
        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        public bool Status { get; set; } = true;

        public T Data { get; set; }
    }
}
