using System;
using System.Configuration;
using System.IO;

namespace FUJIFILM.InterpretacionesDF.DataAccess
{
    public static class Helper
    {
        public static string ConnectionString()
        {
            try
            {
                return ConfigurationManager.ConnectionStrings["BASEDATOS"].ConnectionString;
            }
            catch (Exception ehp)
            {
                throw ehp;
            }
        }

    }
}
