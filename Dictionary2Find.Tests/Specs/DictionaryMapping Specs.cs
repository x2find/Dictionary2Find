using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using System.Linq.Expressions;
using Dictionary2Find.ClientConventions;
using Newtonsoft.Json;
using EPiServer.Find.Json;
using EPiServer.Find.ClientConventions;
using Dictionary2Find.Json;

namespace Dictionary2Find.Tests.Specs
{
    public class when_serializing_a_document_with_dictionaries
    {
        static JsonSerializer serializer;
        static string serializedString;
        static Document originalDocument;

        Establish context = () =>
        {
            serializer = Serializer.CreateDefault();
            var conventions = new DefaultConventions();
            conventions.AddDictionaryConventions();
            serializer.ContractResolver = conventions.ContractResolver;

            originalDocument = new Document();
            originalDocument.Name = "Name";
            originalDocument.StringToStringDictionary.Add("key", "value");
            originalDocument.StringToStringDictionary.Add("key2", "value2");
            originalDocument.IntToStringDictionary.Add(1, "value");
            originalDocument.StringToIntDictionary.Add("key", 1);
            originalDocument.StringToDoubleDictionary.Add("key", 1.0);
            originalDocument.StringToDateTimeDictionary.Add("key", DateTime.Now);
            originalDocument.EnumToStringDictionary.Add(KeyEnum.EnumValue, "value");
        };

        static Document deserializedDocument;
        Because of = () =>
        {
            serializedString = serializer.Serialize(originalDocument);

            deserializedDocument = serializer.Deserialize<Document>(serializedString);
        };

        It should_equal_the_original_object = () =>
        {
            deserializedDocument.StringToStringDictionary["key"].ShouldEqual("value");
            deserializedDocument.StringToStringDictionary["key2"].ShouldEqual("value2");
            deserializedDocument.IntToStringDictionary[1].ShouldEqual("value");
            deserializedDocument.StringToIntDictionary["key"].ShouldEqual(1);
            deserializedDocument.StringToDoubleDictionary["key"].ShouldEqual(1.0);
            deserializedDocument.StringToDateTimeDictionary["key"].ShouldBeCloseTo(DateTime.Now, new TimeSpan(0, 0, 1));
            deserializedDocument.EnumToStringDictionary[KeyEnum.EnumValue].ShouldEqual("value");
        };
    }

    public enum KeyEnum
    {
        EnumValue
    }

    public class Document
    {
        public Document()
        {
            StringToStringDictionary = new Dictionary<string, string>();
            IntToStringDictionary = new Dictionary<int, string>();
            StringToIntDictionary = new Dictionary<string, int>();
            StringToDoubleDictionary = new Dictionary<string, double>();
            StringToDateTimeDictionary = new Dictionary<string, DateTime>();
            EnumToStringDictionary = new Dictionary<KeyEnum, string>();
        }

        public string Name { get; set; }

        public Dictionary<string, string> StringToStringDictionary { get; set; }

        public Dictionary<int, string> IntToStringDictionary { get; set; }

        public Dictionary<string, int> StringToIntDictionary { get; set; }

        public Dictionary<string, double> StringToDoubleDictionary { get; set; }

        public Dictionary<string, DateTime> StringToDateTimeDictionary { get; set; }

        public Dictionary<KeyEnum, string> EnumToStringDictionary { get; set; }
    }
}
