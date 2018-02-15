using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiServer.Find.Json;
using Newtonsoft.Json.Serialization;
using EPiServer.Find;

namespace Dictionary2Find.ClientConventions
{
    public class DictionaryInterceptor : IInterceptContract
    {
        public JsonContract ModifyContract(JsonContract contract)
        {
            if (contract is JsonDictionaryContract)
            {
                var dictionaryContract = contract as JsonDictionaryContract;
                if (typeof(GeoLocation).IsAssignableFrom(dictionaryContract.DictionaryValueType))
                {
                    dictionaryContract.DictionaryKeyResolver = x => x + "$$geo";
                }
                else if (typeof(Attachment).IsAssignableFrom(dictionaryContract.DictionaryValueType))
                {
                    dictionaryContract.DictionaryKeyResolver = x => x + "$$attachment";
                }
                else
                {
                    dictionaryContract.DictionaryKeyResolver = x => TypeSuffix.GetSuffixedFieldName(x, dictionaryContract.DictionaryValueType);
                }

                dictionaryContract.Converter = new Dictionary2Find.Json.DictionaryConverter() { PropertyNameDesolver = x => x.Contains('$') ? x.Split('$')[0] : x };
            }

            return contract;
        }
    }
}
