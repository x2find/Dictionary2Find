using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiServer.Find;

namespace Dictionary2Find.ClientConventions
{
    public static class DictionaryClientConventionsExtensions
    {
        public static void AddDictionaryConventions(this IClientConventions conventions)
        {
            conventions.ContractResolver.ContractInterceptors.Add(new DictionaryInterceptor());
        }
    }
}
