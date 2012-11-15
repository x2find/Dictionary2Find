using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using EPiServer.Find.Helpers;

namespace Dictionary2Find.Json
{
    public class DictionaryConverter : JsonConverter
    {
        public Func<string, string> PropertyNameDesolver { get; set; }

        public DictionaryConverter()
        {
            PropertyNameDesolver = x => x;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // if it is a JsonDictionaryContract use the PropertyNameResolver
            var contract = serializer.ContractResolver.ResolveContract(value.GetType()) as JsonDictionaryContract;
            
            writer.WriteStartObject();
            foreach (var item in (IEnumerable)value)
            {
                Type t = item.GetType();
                PropertyInfo keyProperty = t.GetProperty("Key");
                PropertyInfo valueProperty = t.GetProperty("Value");

                var keyPairKey = keyProperty.GetGetMethod().Invoke(item, null);
                writer.WritePropertyName(!contract.IsNull() ? contract.PropertyNameResolver(keyPairKey.ToString()) : keyPairKey.ToString());
                var keyPairValue = valueProperty.GetGetMethod().Invoke(item, null);
                serializer.Serialize(writer, keyPairValue);
            }
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            IList<Type> genericArguments = objectType.GetGenericArguments();
            Type keyType = genericArguments[0];
            Type valueType = genericArguments[1];

            object key = null;
            object value = null;

            reader.Read();
            while (reader.TokenType == JsonToken.PropertyName)
            {
                var keyValue = reader.Value.ToString();
                key = PropertyNameDesolver(keyValue.ToString());
                if(!keyType.IsAssignableFrom(typeof(string)))
                {
                    // try to parse the key if not a string
                    key = keyType.GetMethod("Parse", new Type[] { typeof(string) }).Invoke(null, new[] { key });
                }

                reader.Read();
                value = serializer.Deserialize(reader, valueType);
                
                objectType.GetMethod("Add").Invoke(existingValue, new[] { key, value });

                reader.Read();
            }
            
            return existingValue;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType && typeof(IDictionary<,>).IsAssignableFrom(objectType.GetGenericTypeDefinition());
        }
    }
}
