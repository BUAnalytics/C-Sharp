# C# and Unity Plugin

BU Analytics plugin for C# and Unity written in .NET.

Please visit our [BU Analytics](http://bu-games.bmth.ac.uk) website for more information.

## Installation

For Unity you must install the BUAnalytics package into your project by navigating to `Edit > Import Package > Custom Package`.

For native C# you must copy the BUAnalytics library from the [BUAnalytics](src/Assets/Plugins) folder into your Visual Studio project.

## Authentication

To authenticate with the backend you must first create an access key through the web management interface.
Then pass these details into the api singleton instance.

```csharp
BUAPI.Instance.Auth = new BUAccessKey("58ac40cd126553000c426f91", "06239e3a1401ba6d7250260d0f8fd680e52ff1e754ebe10a250297ebda2bac41");
```

## Getting Started

You can use the convenience method to quickly add a document to a collection which will be created and uploaded automatically.

```csharp
BUCollectionManager.Instance.Add("Users", new Dictionary<string, object>(){
    { "userId", .. },
    { "name", .. },
    { "age", .. },
    { "gender", .. },
    { "device", new Dictionary<string, string>{
        { "type", .. },
        { "name", .. },
        { "model", .. },
    } }
});
```

If you would like to manage your own collections and documents please see below.

## Creating Collections

We must then create the collections that we would like to use throughout the application. 
This can be done at any point and as many times as needed however collections will not be overwritten if created with a duplicate names.

```csharp
BUCollectionManager.Instance.Create(new string[]{
    "Users",
    "Sessions",
    "Clicks"
});
```

## Creating a Document

We can create a document using a dictionary literal that allows for as many nested values as needed. 
Documents support nested dictionaries, arrays and will encode literal data types when uploading to the backend server.

```csharp
var userDoc = new BUDocument(new Dictionary<string, object>(){
    { "userId", .. },
    { "name", .. },
    { "age", .. },
    { "gender", .. },
    { "device", new Dictionary<string, string>{
        { "type", .. },
        { "name", .. },
        { "model", .. },
    } }
});
```

You can also create documents through the add method or can access the raw dictionary object through the contents property.

```csharp
var userDoc = new BUDocument();

userDoc.Add("userId", ..);
userDoc.Add("name", ..);

userDoc.Contents["age"] = ..;
userDoc.Contents["gender"] = ..;
```

## Adding a Document to Collection

You can then add one or more documents to a collection through the collection manager.

```csharp
BUCollectionManager.Instance.Collections["Users"].Add(userDoc);
BUCollectionManager.Instance.Collections["Users"].AddRange(new BUDocument[]{ userDoc1, userDoc2, userDoc3 });
```

Collections will automatically push all documents to the backend server every two seconds if not empty. 
You can also manually initiate an upload either on all or a specific collection.

```csharp
BUCollectionManager.Instance.UploadAll();
BUCollectionManager.Instance.Collections["Users"].Upload();
```

You can also use the interval property to configure how often collections are uploaded in milliseconds. 
The default is 2000 milliseconds and setting it to 0 will disable automatic uploads.

```csharp
BUCollectionManager.Instance.Interval = 4000;
```

## Error Handling

You can subscribe to actions in the collection manager to notify you when collections upload successfully or return errors.

```csharp
BUCollectionManager.Instance.Error = (collection, errorCode) => {
    //...
};

BUCollectionManager.Instance.Success = (collection, successCount) => {
    //...
};
```

You can also provide error and success actions to an individual collection using the upload method.

## Unique Identifiers

You can use our backend to generate unique identifiers for use inside documents.
Setup the cache at startup specifying how many identifiers you'd like to hold.

```csharp
BUID.Instance.Start(200);
```

Once the cache has been marked as ready you can generate identifiers at any time.

```csharp
if (BUID.Instance.IsReady){
    userDoc.Add("userId", BUID.Instance.Generate());
}
```

You can modify the refresh frequency or size of the cache depending on how many identifiers you require.
GUIDs will be generated as a backup should the cache become empty.

```csharp
BUID.Instance.Interval = 4000;
BUID.Instance.Size = 100;
```

## Advanced

The hostname defaults to the university server although we can change this if necessary.

```csharp
BUAPI.Instance.URL = "https://192.168.0.x";
BUAPI.Instance.Path = "/api/v1";
```