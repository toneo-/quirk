using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quirk.Graphics
{
    public class QuirkGraphicsException : System.Exception
    {
        public QuirkGraphicsException(string Message)
            : base(Message)
        {

        }
    }
}
