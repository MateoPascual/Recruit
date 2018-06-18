using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public static class Utils
    {
        public static string ConvertStringToLikeExpression(string inputString)
        {
            var builder = new StringBuilder();
            builder.Append('%');
            foreach (var c in inputString)
            {
                builder.Append(c);
                builder.Append('%');
            }
            return builder.ToString();
        }
    }
}
