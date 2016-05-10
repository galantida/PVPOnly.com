using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PVPOnlyLibrary;

namespace PVPOnlyWebServices
{
    public class clsUtil
    {
        public static string fileNameFromPath(string path)
        {
            return path.Substring(path.LastIndexOf("\\")+1);
        }

        public static int toNumber(string value)
        {
            int result;
            if (!int.TryParse(value, out result))
            {
                result = 0;
            }
            return result;
        }
    }
}