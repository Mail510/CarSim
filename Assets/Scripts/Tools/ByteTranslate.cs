using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public class ByteTranslate
    {
        public static short ByteToInt(byte a,byte b)
        {
            short buffer = 0;
            buffer = (short)((a << 8) + b);
            return buffer;
        }

        public static long ByteToLong(byte[] buf)
        {
            long ans = 0;

            int m = 8;
            for (int i = 0; i < 4; i++)
            {
                ans = ((buf[i] << m) & 0xFF);
                m *= 2;
            }

            return ans;
        }
    }
}
