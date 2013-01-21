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
    public class DictionariesFilterKeyValues
    {
        [Fact]
        public void FilterByKeyValueInDictionary()
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
            client.Conventions.ContractResolver.ContractInterceptors.Add(new DictionaryConvention());
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

    public class DictionariesFilterStringKeys
    {
        [Fact]
        public void FilterByKeysInDictionary()
        {
            new Story("Filter by matching a string key in a dictionary")
                .InOrderTo("be able to filter on specific key in a dictionary")
                .AsA("developer")
                .IWant("to be able to map dictionaries to their correct value type")
                .WithScenario("map dictionaries to their correct value type")
                .Given(IHaveAClient)
                    .And(IHaveMappedTypesToDictionaryKeys)
                    .And(IHaveADocument)
                    .And(TheDocumentContainsAMetadataEntryWithAuthorHenrik)
                    .And(IHaveIndexedTheDocumentObject)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForADocumentWithAuthorInTheMetadataEntries)
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
            client.Conventions.ContractResolver.ContractInterceptors.Add(new DictionaryConvention());
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
        void ISearchForADocumentWithAuthorInTheMetadataEntries()
        {
            result = client.Search<Document>()
                        .Filter(x => x.MetadataDictionary.Keys.Match("Author"))
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

    public class DictionariesSearchStringKeys
    {
        [Fact]
        public void SearchByKeysInDictionary()
        {
            new Story("Search by matching a string key in a dictionary")
                .InOrderTo("be able to search on keys in a dictionary")
                .AsA("developer")
                .IWant("to be able to map dictionaries to their correct value type")
                .WithScenario("map dictionaries to their correct value type")
                .Given(IHaveAClient)
                    .And(IHaveMappedTypesToDictionaryKeys)
                    .And(IHaveADocument)
                    .And(TheDocumentContainsAMetadataEntryWithAuthorHenrik)
                    .And(IHaveIndexedTheDocumentObject)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForADocumentWithAuthorInTheMetadataEntries)
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
            client.Conventions.ContractResolver.ContractInterceptors.Add(new DictionaryConvention());
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
        void ISearchForADocumentWithAuthorInTheMetadataEntries()
        {
            result = client.Search<Document>()
                        .For("Author")
                        .InField(x => x.MetadataDictionary.Keys)
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

    public class DictionariesFilterIntKeys
    {
        [Fact]
        public void FilterByKeysInDictionary()
        {
            new Story("Filter by matching an int key in a dictionary")
                .InOrderTo("be able to filter on specific key in a dictionary")
                .AsA("developer")
                .IWant("to be able to map dictionaries to their correct value type")
                .WithScenario("map dictionaries to their correct value type")
                .Given(IHaveAClient)
                    .And(IHaveMappedTypesToDictionaryKeys)
                    .And(IHaveADocument)
                    .And(TheDocumentContainsAMetadataEntryWithAuthorHenrik)
                    .And(IHaveIndexedTheDocumentObject)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForADocumentWithAuthorInTheMetadataEntries)
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
            client.Conventions.ContractResolver.ContractInterceptors.Add(new DictionaryConvention());
        }

        private Document document;
        void IHaveADocument()
        {
            document = new Document();
        }

        void TheDocumentContainsAMetadataEntryWithAuthorHenrik()
        {
            document.MetadataDictionary.Add(1, "Henrik");
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
        void ISearchForADocumentWithAuthorInTheMetadataEntries()
        {
            result = client.Search<Document>()
                        .Filter(x => x.MetadataDictionary.Keys.Match(1))
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
                MetadataDictionary = new Dictionary<int, string>();
            }

            public string Name { get; set; }

            public Dictionary<int, string> MetadataDictionary { get; set; }
        }
    }

    public class DictionariesSearchIntKeys
    {
        [Fact]
        public void SearchByIntKeysInDictionary()
        {
            new Story("Search by matching a int key in a dictionary")
                .InOrderTo("be able to search on keys in a dictionary")
                .AsA("developer")
                .IWant("to be able to map dictionaries to their correct value type")
                .WithScenario("map dictionaries to their correct value type")
                .Given(IHaveAClient)
                    .And(IHaveMappedTypesToDictionaryKeys)
                    .And(IHaveADocument)
                    .And(TheDocumentContainsAMetadataEntryWithAuthorHenrik)
                    .And(IHaveIndexedTheDocumentObject)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForADocumentWithAuthorInTheMetadataEntries)
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
            client.Conventions.ContractResolver.ContractInterceptors.Add(new DictionaryConvention());
        }

        private Document document;
        void IHaveADocument()
        {
            document = new Document();
        }

        void TheDocumentContainsAMetadataEntryWithAuthorHenrik()
        {
            document.MetadataDictionary.Add(1, "Henrik");
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
        void ISearchForADocumentWithAuthorInTheMetadataEntries()
        {
            result = client.Search<Document>()
                        .For("1")
                        .InField(x => x.MetadataDictionary.Keys)
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
                MetadataDictionary = new Dictionary<int, string>();
            }

            public string Name { get; set; }

            public Dictionary<int, string> MetadataDictionary { get; set; }
        }
    }

    public class DictionariesFilterValues
    {
        [Fact]
        public void FilterByValuesInDictionary()
        {
            new Story("Filter by matching a value in a dictionary")
                .InOrderTo("be able to filter on specific value in a dictionary")
                .AsA("developer")
                .IWant("to be able to map dictionaries to their correct value type")
                .WithScenario("map dictionaries to their correct value type")
                .Given(IHaveAClient)
                    .And(IHaveMappedTypesToDictionaryKeys)
                    .And(IHaveADocument)
                    .And(TheDocumentContainsAMetadataEntryWithAuthorHenrik)
                    .And(IHaveIndexedTheDocumentObject)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForADocumentWithHenrikInTheMetadataValues)
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
            client.Conventions.ContractResolver.ContractInterceptors.Add(new DictionaryConvention());
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
        void ISearchForADocumentWithHenrikInTheMetadataValues()
        {
            result = client.Search<Document>()
                        .Filter(x => x.MetadataDictionary.Values.Match("Henrik"))
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

    public class DictionariesSearchValues
    {
        [Fact]
        public void SearchByValuesInDictionary()
        {
            new Story("Search by matching a value in a dictionary")
                .InOrderTo("be able to search on values in a dictionary")
                .AsA("developer")
                .IWant("to be able to map dictionaries to their correct value type")
                .WithScenario("map dictionaries to their correct value type")
                .Given(IHaveAClient)
                    .And(IHaveMappedTypesToDictionaryKeys)
                    .And(IHaveADocument)
                    .And(TheDocumentContainsAMetadataEntryWithAuthorHenrik)
                    .And(IHaveIndexedTheDocumentObject)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForADocumentWithHenrikInTheMetadataValues)
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
            client.Conventions.ContractResolver.ContractInterceptors.Add(new DictionaryConvention());
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
        void ISearchForADocumentWithHenrikInTheMetadataValues()
        {
            result = client.Search<Document>()
                        .For("Henrik")
                        .InField(x => x.MetadataDictionary.Values)
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

    public class DictionariesWithGeolocation
    {
        [Fact]
        public void FilterByValueInDictionary()
        {
            new Story("Filter by matching a key/geolocation in a dictionary")
                .InOrderTo("be able to filter on a geolocation of a specific key in a dictionary")
                .AsA("developer")
                .IWant("to be able to map dictionaries to their correct value type")
                .WithScenario("map dictionaries to their correct value type")
                .Given(IHaveAClient)
                    .And(IHaveMappedTypesToDictionaryKeys)
                    .And(IHaveAStore)
                    .And(TheStoreHasALocationInStockholm)
                    .And(IHaveIndexedTheStoreObject)
                    .And(IHaveWaitedForASecond)
                .When(ISearchForAStoreWithin1kmFromSergelsTorg)
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
            client.Conventions.ContractResolver.ContractInterceptors.Add(new DictionaryConvention());
        }

        private Store store;
        void IHaveAStore()
        {
            store = new Store();
        }

        void TheStoreHasALocationInStockholm()
        {
            store.Locations.Add("Stockholm", new GeoLocation(59.33265, 18.06468));
        }

        void IHaveIndexedTheStoreObject()
        {
            client.Index(store);
        }

        void IHaveWaitedForASecond()
        {
            Thread.Sleep(1000);
        }

        SearchResults<Store> result;
        void ISearchForAStoreWithin1kmFromSergelsTorg()
        {
            result = client.Search<Store>()
                        .Filter(x => x.Locations["Stockholm"].WithinDistanceFrom(new GeoLocation(59.33234, 18.06291), 1.Kilometers()))
                        .GetResult();
        }

        void IShouldGetASingleHit()
        {
            result.TotalMatching.Should().Be(1);
        }

        public class Store
        {
            public Store()
            {
                Locations = new Dictionary<string, GeoLocation>();
            }

            public string Name { get; set; }

            public Dictionary<string, GeoLocation> Locations { get; set; }
        }
    }
}
