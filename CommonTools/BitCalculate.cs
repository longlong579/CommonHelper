using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZZG.Common.Tolls
{
    public class BitCalculate
    {
        public static int BitOneCount(int num)
        {
            int count = 0;
            while (num > 0)
            {
                num = num & (num - 1);
                count++;
            }
            return count;
        }
    }
}
