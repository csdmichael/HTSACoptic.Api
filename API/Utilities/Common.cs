using System;

namespace HTSA.Utilities
{
    public static class Common
    {
        public static string getFileDateTime()
        {
            return "_" + DateTime.Now.Year.ToString() + "_"
                + DateTime.Now.Month.ToString() + "_"
                + DateTime.Now.Day.ToString() + "_"
                + DateTime.Now.Hour.ToString() + "_"
                + DateTime.Now.Minute.ToString() + "_"
                + DateTime.Now.Second.ToString();
        }
    }
}
