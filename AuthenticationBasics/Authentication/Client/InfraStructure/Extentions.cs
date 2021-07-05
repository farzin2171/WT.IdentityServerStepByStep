using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.InfraStructure
{
    public static class Extentions
    {
        public static byte[] ConvertFromBase64String(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;
            try
            {
                string working = input.Replace('-', '+').Replace('_', '/'); ;
                while (working.Length % 4 != 0)
                {
                    working += '=';
                }
                return Convert.FromBase64String(working);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
