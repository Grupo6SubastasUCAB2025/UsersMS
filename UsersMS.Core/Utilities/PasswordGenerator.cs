using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersMS.Core.Utilities
{
    public static class PasswordGenerator
    {
        public static string GeneratePassword(int length = 6)
        {
            var random = new Random();
            return random.Next((int)Math.Pow(10, length - 1), (int)Math.Pow(10, length) - 1).ToString();
        }
    }
}
