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
            List<object> keys = new List<object>();
            List<object> values = new List<object>();

            writer.WriteStartObject();
            foreach (var item in (IEnumerable)value)
            {
                Type t = item.GetType();
                PropertyInfo keyProperty = t.GetProperty("Key");
                PropertyInfo valueProperty = t.GetProperty("Value");

                var keyPairKey = keyProperty.GetGetMethod().Invoke(item, null);
                keys.Add(keyPairKey);
                writer.WritePropertyName(!contract.IsNull() ? contract.PropertyNameResolver(keyPairKey.ToString()) : keyPairKey.ToString());
                var keyPairValue = valueProperty.GetGetMethod().Invoke(item, null);
                values.Add(keyPairValue);
                serializer.Serialize(writer, keyPairValue);
            }

            writer.WritePropertyName("Keys");
            serializer.Serialize(writer, keys);
            writer.WritePropertyName("Values");
            serializer.Serialize(writer, values);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (existingValue == null)
            {
                existingValue = objectType.GetConstructor(new Type[0]).Invoke(new object[0]);
            }

            IList<Type> genericArguments = objectType.GetGenericArguments();
            Type keyType = genericArguments[0];
            Type valueType = genericArguments[1];

            object key = null;
            object value = null;

            reader.Read();
            while (reader.TokenType == JsonToken.PropertyName)
            {
                var keyValue = reader.Value.ToString();
                if (keyValue.Equals("Keys") || keyValue.Equals("Values"))
                {
                    reader.Read();
                    serializer.Deserialize(reader);
                    reader.Read();
                    continue;
                }
                key = PropertyNameDesolver(keyValue.ToString());
                if (keyType.IsEnum)
                {
                    key = Enum.Parse(keyType, key.ToString());
                }
                else if (!keyType.IsAssignableFrom(typeof(string)))
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
