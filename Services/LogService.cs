

using DataNex.Model.Models;
using StoreApi.Data;
using StoreApi.Data.Enums;

namespace StoreApi.Services
{
    public class LogService
    {
        public static void CreateLog(string Name, LogTypeEnum logType, LogOriginEnum logOrigin, Guid? userId, ApplicationDbContext context)
        {
            var log = new Log();
            log.LogName = Name;
            log.LogType = logType;
            log.LogOrigin = logOrigin;
            log.DateAdded = DateTime.UtcNow;
            log.UserAdded = userId;      
            context.Logs.Add(log);

            context.SaveChanges();
        }
    }
}
