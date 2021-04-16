using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.Entities
{
    public class Response<T>
    {
        public string Date
        {
            get => DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public bool Status { get; set; } = true;

        public T Data { get; set; }
    }
}
