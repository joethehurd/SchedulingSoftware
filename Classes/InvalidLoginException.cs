using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingSoftware.Classes
{
    [Serializable]
    class InvalidLoginException : Exception
    {       
        public InvalidLoginException() 
        {}

        public InvalidLoginException(string message) : base(message)
        {}      
    }
}
