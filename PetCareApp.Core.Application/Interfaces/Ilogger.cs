using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface Ilogger
    {
        void Info(string mensaje, string usuario = "");

        void Warning(string mensaje, string usuario = "");

        void Error(string mensaje, string usuario = "");
    }
}
