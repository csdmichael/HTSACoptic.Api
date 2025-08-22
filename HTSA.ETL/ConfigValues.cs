using System.Configuration;

namespace HTSA.ETL
{
    public static class ConfigValues
    {
        public static string dbConn;
       


        public static void ReadConfigFile()
        {
            dbConn = ConfigurationManager.ConnectionStrings["CPConnection"].ConnectionString;
            

        }
    }
}