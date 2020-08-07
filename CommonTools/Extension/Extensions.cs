using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZZG.Common.Tolls
{

    public static class Extensions
    {
        public static int SecToMs(this double sec)
        {
            return (int)sec * 1000;
        }
    }
}
