Dictionary2Find
===============

Adds dictionary searching/filtering to EPiServer Find's .NET API

### Build

In order to build Dictionary2Find the NuGet packages that it depends on must be restored.
See http://docs.nuget.org/docs/workflows/using-nuget-without-committing-packages

### Usage

Include the IncludeTypeNameInDictionaryKeyFieldNameConvention to the conventions:

```c#
client.Conventions.ContractResolver.ContractInterceptors.Add(new IncludeTypeNameInDictionaryKeyFieldNameConvention());
```

and start searching:

```c#
result = client.Search<Document>()
            .For("Henrik")
            .InField(x => x.MetadataDictionary["Author"])
            .GetResult();
```

or filtering...
...by key/value:

```c#
result = client.Search<Document>()
            .Filter(x => x.MetadataDictionary["Author"].Match("Henrik"))
            .GetResult();
```

...by keys:

```c#
result = client.Search<Document>()
            .Filter(x => x.MetadataDictionary.Keys.Match("Author"))
            .GetResult();
```

...by values:

```c#
result = client.Search<Document>()
            .Filter(x => x.MetadataDictionary.Values.Match("Henrik"))
            .GetResult();
```