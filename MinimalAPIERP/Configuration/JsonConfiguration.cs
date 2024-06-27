using System.Text.Json.Serialization;
using System.Text.Json;

namespace MinimalAPIERP.Configuration{
    public class JsonConfiguration{
        public static JsonSerializerOptions GetAPIJsonSerializerOptions()
        {
            return new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                //PropertyNameCaseInsensitive = false,
                //PropertyNamingPolicy = null,
                WriteIndented = true,
                //IncludeFields = false,
                MaxDepth = 0,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                //ReferenceHandler = ReferenceHandler.Preserve
                //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }
    }
}
