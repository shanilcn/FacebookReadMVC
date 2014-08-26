using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FacebookReadMVC
{
    public static class Infrastructure
    {
        public static string GenerateStars(int Count)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Count; i++)
                sb.Append("*");
            return sb.ToString();
        }

        //public static string DateTimeToUnixTimestamp(DateTime dateTime)
        //{
        //    return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds.ToString();
        //}
    }
}