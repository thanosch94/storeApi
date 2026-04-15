
using StoreApi.Data.Enums;
using StoreApi.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataNex.Model.Models
{
    [Table("store_logs")]
    public class Log:BaseModel
    {
        public Log()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id {  get; set; }

        public string LogName {  get; set; }

        public LogTypeEnum LogType { get; set; }

        public LogOriginEnum LogOrigin { get; set; }

    }
}
