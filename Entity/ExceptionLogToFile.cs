using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ExceptionLogToFile : IExceptionLog
    {
        public void Log(string message)
        {
            throw new NotImplementedException();
        }
    }
}
