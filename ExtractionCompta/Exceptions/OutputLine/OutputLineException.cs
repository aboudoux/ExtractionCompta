using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractionCompta.Exceptions.OutputLine
{
    public abstract class OutputLineException : Exception
    {
        protected OutputLineException(string message) : base(message)
        {
            
        }
    }
}
