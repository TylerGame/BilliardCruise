using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public static class JsonUtils
{
    public static T Deserialize<T>(string text)
    {
        return JsonConvert.DeserializeObject<T>(text, JsonUtils.ComposeSerializerSettings());
    }

    public static string Serialize(object obj)
    {
        return JsonConvert.SerializeObject(obj, JsonUtils.ComposeSerializerSettings());
    }

    public static JsonSerializerSettings ComposeSerializerSettings()
    {
        var serializerSettings = new JsonSerializerSettings
        {
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            TypeNameHandling = TypeNameHandling.Auto,
        };

        serializerSettings.Converters.Add(
            new StringEnumConverter
            {
                NamingStrategy = new CamelCaseNamingStrategy { OverrideSpecifiedNames = false }
            }
        );

        return serializerSettings;
    }

}
