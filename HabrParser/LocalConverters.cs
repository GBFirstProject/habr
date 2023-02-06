using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabrParser
{
    public static class LocalConverters
    {
        public static Guid Int2Guid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public static Guid Str2Guid(string value)
        {
            int iValue = Convert.ToInt32(value);
            return Int2Guid(iValue);
        }

        public static int Guid2Int(Guid value)
        {
            byte[] b = value.ToByteArray();
            int bint = BitConverter.ToInt32(b, 0);
            return bint;
        }

    }
}
