using StoreApi.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreApi.Data.Models
{
    [Table("store_import_settings")]
    public class ImportSetting:BaseModel
    {
        public ImportSetting()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string Name { get; set; }
        public string? Folder { get; set; }
        public string? GetUrl { get; set; }
        public string? DbMatchProperty { get; set; }
        public string? FileMatchProperty { get; set; }

        public bool UpdateExistingEntities { get; set; }
        public ImportTypeEnum ImportType { get; set; }


    }
}
