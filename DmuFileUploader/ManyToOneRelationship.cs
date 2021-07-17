namespace DmuFileUploader
{
    using Newtonsoft.Json;

    [JsonObject(Title = "ManyToOneRelationship")]
    public class ManyToOneRelationship
    {
        [JsonProperty("ReferencedEntity")]
        public string ReferencedEntity { get; set; }

        [JsonProperty("ReferencingAttribute")]
        public string ReferencingAttribute { get; set; }

        [JsonProperty("ReferencingEntityNavigationPropertyName")]
        public string ReferencingEntityNavigationPropertyName { get; set; }
    }
}
