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

        public T Data { get; set; }

        public ResponseInfo Info { get; set; } = new ResponseInfo();
    }

    public class ResponseInfo
    {
        public int Code { get; set; } = 0;

        [JsonProperty("Title")]
        public string MessageTitle { get; set; } = "Sucesso";

        [JsonProperty("Description")]
        public string MessageDescription { get; set; } = "Operação realizada com sucesso.";
    }
}
