using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace System
{
    public class BizException : ApplicationException
    {
        public BizException() : base() { }

        public BizException(string message) : base(message) { }

        public BizException(string message, Exception innerException) : base(message, innerException) { }

        public BizException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
