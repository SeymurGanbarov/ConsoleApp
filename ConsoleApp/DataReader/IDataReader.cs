using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public interface IDataReader
    {
        void ImportAndPrintData(string fileToImport, bool printData = true);
    }
}
