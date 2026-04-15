using StoreApi.Data.Enums;

namespace StoreApi.Data.Dto
{
    public class ImportSettingsDto
    {
        public Guid? Id { get; set; }

        public string? Title { get; set; }
        public string? Name { get; set; }
        public string? Folder { get; set; }

        public string? GetUrl { get; set; }

        /// <summary>
        /// The property which will be used to determine if the entity exists in the database
        /// </summary>
        public string? DbMatchProperty { get; set; }

        public string? FileMatchProperty { get; set; }

        public bool UpdateExistingEntities { get; set; }
        public ImportTypeEnum ImportType { get; set; }
    }
}
