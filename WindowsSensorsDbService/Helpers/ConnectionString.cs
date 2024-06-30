using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsSensorsDbService.Helpers
{
    public static class ConnectionString
    {
        public static string GetConnectionString()
        {
            return $"" +
                $"Server={UserConfigManager.GetSetting("db_server")};" +
                $"Initial Catalog={UserConfigManager.GetSetting("db_name")};" +
                $"User ID={UserConfigManager.GetSetting("db_user")};" +
                $"Password={UserConfigManager.GetSetting("db_password")};" +
                $"TrustServerCertificate=True;";
        }
    }
}
