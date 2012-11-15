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
    public class when_serializing_a_document_with_a_string_to_string_dictionary
    {
        static JsonSerializer serializer;
        static string serializedString;

        Establish context = () =>
        {
            serializer = Serializer.CreateDefault();
            var conventions = new DefaultConventions();
            conventions.ContractResolver.ContractInterceptors.Add(new IncludeTypeNameInDictionaryKeyFieldNameConvention());
            serializer.ContractResolver = conventions.ContractResolver;
        };

        Because of = () =>
        {
            var document = new Document();
            document.Name = "Hej";
            document.StringToStringDictionary.Add("key", "value");
            document.StringToStringDictionary.Add("key2", "value2");
            document.IntToStringDictionary.Add(1, "value");
            //document.StringToIntDictionary.Add("key", 1);
            //document.StringToDoubleDictionary.Add("key", 1.0);
            //document.StringToDateTimeDictionary.Add("key", DateTime.Now);
            serializedString = serializer.Serialize(document);

            var document2 = serializer.Deserialize<Document>(serializedString);
            var key = document2.StringToStringDictionary["key"];
        };

        It should_be_suffixed_nested = () =>
            serializedString.ShouldEqual("$$string");
    }

    public class Document
    {
        public Document()
        {
            StringToStringDictionary = new Dictionary<string, string>();
            IntToStringDictionary = new Dictionary<int, string>();
            //StringToIntDictionary = new Dictionary<string, int>();
            //StringToDoubleDictionary = new Dictionary<string, double>();
            //StringToDateTimeDictionary = new Dictionary<string, DateTime>();
        }

        public string Name { get; set; }

        public Dictionary<string, string> StringToStringDictionary { get; set; }

        public Dictionary<int, string> IntToStringDictionary { get; set; }

        //public Dictionary<string, int> StringToIntDictionary { get; set; }

        //public Dictionary<string, double> StringToDoubleDictionary { get; set; }

        //public Dictionary<string, DateTime> StringToDateTimeDictionary { get; set; }
    }
}
