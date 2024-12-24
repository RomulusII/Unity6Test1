using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class PolymorphicJsonConverter<TBase> : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(TBase).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var typeName = jsonObject["$type"]?.ToString();
        if (typeName == null)
        {
            throw new JsonSerializationException("Missing $type property.");
        }

        var type = Type.GetType(typeName) ?? throw new JsonSerializationException($"Unknown type: {typeName}");

        var target = Activator.CreateInstance(type) ?? throw new JsonSerializationException($"Unknown type: {typeName}");

        serializer.Populate(jsonObject.CreateReader(), target);
        return target;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var jsonObject = JObject.FromObject(value, serializer);
        jsonObject.AddFirst(new JProperty("$type", value.GetType().AssemblyQualifiedName));
        jsonObject.WriteTo(writer);
    }
}
