using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class _s_Utils_Random
    {
        private static Random random = new Random();
        public static string RandomString(int len)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, len)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
