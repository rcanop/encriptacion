using System;
using System.Collections.Generic;
using System.Text;

namespace RafaCano.Util.Encriptacion
{
    public class DistinctCodePageException : Exception
    {
        public DistinctCodePageException() { }
        public DistinctCodePageException(string message) : base(message) { }
        public DistinctCodePageException(string message, Exception inner) : base(message, inner) { }
    }
}
