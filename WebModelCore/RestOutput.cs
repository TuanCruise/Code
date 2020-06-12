using System;
using System.Collections.Generic;
using System.Text;

namespace WebModelCore {
    public class RestOutput<T> : RestOutput {

        public T Data { get; set; }
    }
    public abstract class RestOutput {

        public int ResultCode { get; set; }
        public string Message { get; set; }
    }
}
