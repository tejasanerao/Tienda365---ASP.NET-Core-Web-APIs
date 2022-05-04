using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda365.API.DTO
{
    public class Response<T>
    {
        public T Data { get; set; }
        public List<string> Message { get; set; } = new List<string>();
    }

    public class Response
    {
        public List<string> Message { get; set; } = new List<string>();
    }
}

