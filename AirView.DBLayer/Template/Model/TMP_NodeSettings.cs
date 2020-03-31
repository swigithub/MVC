namespace AirView.DBLayer.Template.Model
{
    public class TMP_NodeSettings
    {
        public decimal NodeSettingsId { get; set; }
        public decimal NodeId { get; set; }
        public decimal DefinationId { get; set; }
        public string KeyName { get; set; }
        public string MappedId { get; set; }
        public string Value { get; set; }
        public string Settings { get; set; }
        public int SortOrder { get; set; }

        // Extra property to handle Defination Name like 'text, image, dropdown' etc 
        public string DefinationName { get; set; }
        public string QueryWhereClause { get; set; }
    }
}
