﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPiServer.Find;
using EPiServer.Find.Api.Querying.Filters;
using EPiServer.Find.ClientConventions;
using FluentAssertions;
using StoryQ;
using Xunit;
using System.Threading;
using Dictionary2Find.ClientConventions;

namespace Dictionary2Find.Tests.Stories
{
    public class Dictionaries
    {
        [Fact]
        public void FilterByValueInDictionary()
        {
            new Story("Filter by matching a key/value in a dictionary")
                .InOrderTo("be able to filter on a value of a specific key in a dictionary")
                .AsA("developer")
                .IWant("to be able to map dictionaries to their correct value type")
                .WithScenario("map dictionaries to their correct value type")
                .Given(IHaveAClient)
                    .And(IHaveMappedTypesToDictionaryKeys)
                    .And(IHaveADocument)
                    .And(TheDocumentContainsAMetadataEntryWithAuthorHenrik)
                    .And(IHaveIndexedTheDocumentObject)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForADocumentWithAuthorHenrikInTheMetadataEntries)
                .Then(IShouldGetASingleHit)
                .Execute();
        }

        protected IClient client;
        void IHaveAClient()
        {
            client = Client.CreateFromConfig();
        }

        void IHaveMappedTypesToDictionaryKeys()
        {
            client.Conventions.ContractResolver.ContractInterceptors.Add(new IncludeTypeNameInDictionaryKeyFieldNameConvention());
        }

        private Document document;
        void IHaveADocument()
        {
            document = new Document();
        }

        void TheDocumentContainsAMetadataEntryWithAuthorHenrik()
        {
            document.MetadataDictionary.Add("Author", "Henrik");
        }

        void IHaveIndexedTheDocumentObject()
        {
            client.Index(document);
        }

        void IHaveWaitedForASecond()
        {
            Thread.Sleep(1000);
        }

        SearchResults<Document> result;
        void ISearchForADocumentWithAuthorHenrikInTheMetadataEntries()
        {
            result = client.Search<Document>()
                        .Filter(x => x.MetadataDictionary["Author"].Match("Henrik"))
                        .GetResult();
        }

        void IShouldGetASingleHit()
        {
            result.TotalMatching.Should().Be(1);
        }

        public class Document
        {
            public Document()
            {
                MetadataDictionary = new Dictionary<string, string>();
            }

            public string Name { get; set; }

            public Dictionary<string, string> MetadataDictionary { get; set; }
        }
    }
}
