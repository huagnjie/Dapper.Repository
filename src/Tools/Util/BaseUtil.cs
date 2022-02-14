using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Util
{
    public static class BaseUtil
    {
        public static string baseDirectory => AppDomain.CurrentDomain.BaseDirectory.ToString();
    }
}
