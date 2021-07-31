namespace DmuFileUploader
{
    using Newtonsoft.Json;

    [JsonObject(Title = "EntityDefinition")]
    public class EntityDefinition
    {
        [JsonProperty("EntitySetName")]
        public string ResourcePath { get; set; }

        [JsonProperty("MetadataID")]
        public string MetadataID { get; set; }

        [JsonProperty("ManyToOneRelationships")]
        public ManyToOneRelationship[] ManyToOneRelationships { get; set; }

        [JsonProperty("ManyToManyRelationships")]
        public ManyToManyRelationship[] ManyToManyRelationships { get; set; }
    }
}
