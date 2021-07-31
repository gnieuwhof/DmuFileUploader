namespace DmuFileUploader
{
    using Newtonsoft.Json;

    [JsonObject(Title = "ManyToManyRelationship")]
    public class ManyToManyRelationship
    {
        [JsonProperty("Entity2LogicalName")]
        public string Entity2LogicalName { get; set; }

        [JsonProperty("Entity2NavigationPropertyName")]
        public string Entity2NavigationPropertyName { get; set; }
    }
}
