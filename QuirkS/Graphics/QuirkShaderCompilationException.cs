using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quirk.Graphics
{
    public class QuirkShaderCompilationException : System.Exception
    {
        public QuirkShaderCompilationException(string Message, string CompilationLog)
            : base(Message + Environment.NewLine + CompilationLog)
        {
            
        }
    }
}
