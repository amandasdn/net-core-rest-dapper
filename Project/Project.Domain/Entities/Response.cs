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

        public void SetError(string message)
        {
            Data = default;
            Info.Success = false;
            Info.Message = message;
        }
    }

    public class ResponseInfo
    {
        public bool Success { get; set; } = true;

        public string Message { get; set; } = "Operação realizada com sucesso.";
    }
}
