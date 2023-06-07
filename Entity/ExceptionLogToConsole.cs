using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ExceptionLogToConsole : IExceptionLog
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
