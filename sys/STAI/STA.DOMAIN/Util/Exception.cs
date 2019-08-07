using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace STA.DOMAIN.Util
{
    public class Exception : System.Exception
    {
        [Serializable]
        public class MyException : System.Exception
        {
            //
            // For guidelines regarding the creation of new exception types, see
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
            // and
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
            //

            public MyException()
            {
            }

            public MyException(string message)
                : base(message)
            {
            }

            public MyException(string message, System.Exception inner)
                : base(message, inner)
            {
            }

            protected MyException(
                SerializationInfo info,
                StreamingContext context)
                : base(info, context)
            {
            }
        }
    }
}
