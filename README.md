# C-Sharp
BU Analytics C# and Unity Plugin

## Installation

To install the plugin you must copy the BUGamesLab folder into your Unity project under the /Assets/Plugins directory.

Authentication

To authenticate with the backend you must first create an access key through the web management interface. Then pass these details into the api singleton instance.

```cs
BGAPI.Instance.Auth = new BGAccessKey("58569825377ba00001ae8315", "KUygr6bgxhEtsSQ1RJYla2UCtiEE8R");
```

The hostname defaults to the university server although we can change this if necessary.

```cs
BGAPI.Instance.URL = "https://bu-games.bmth.ac.uk";
BGAPI.Instance.Path = "/api/v1";
```

## Creating Collections

We must then create the collections that we would like to use throughout the application. This can be done at any point and as many times as needed however collections will not be overwritten if created with a duplicate names.

```cs
BGCollectionManager.Instance.Create(new string[]{
    "Users",
    "Sessions",
    "Clicks"
});
```

## Creating a Document

We can create a document using a dictionary literal that allows for as many nested values as needed. Documents support nested dictionaries, arrays and will encode literal data types when uploading to the backend server.

```cs
var userDoc = new BGDocument(new Dictionary<string, object>(){
    { "userId", userId },
    { "name", nameField.text },
    { "age", age },
    { "gender", gender },
    { "device", new Dictionary<string, string>{
        { "type", SystemInfo.deviceType.ToString() },
        { "name", SystemInfo.deviceName },
        { "model", SystemInfo.deviceModel },
    } }
});
```

You can also create documents through the add method or can access the raw dictionary object through the contents property.

```cs
var userDoc = new BGDocument();
userDoc.Add("userId", userId);
userDoc.Add("name", nameField.text);
userDoc.Add("age", age);
userDoc.Add("gender", gender);
```

## Adding a Document to Collection

You can then add one or more documents to a collection through the collection manager.

```cs
BGCollectionManager.Instance.Collections["Users"].Add(userDoc);
BGCollectionManager.Instance.Collections["Users"].AddRange(new BGDocument[]{ userDoc1, userDoc2, userDoc3 });
```

Collections will automatically push all documents to the backend server every two seconds if not empty. You can also manually initiate an upload either on all or a specific collection.

```cs
BGCollectionManager.Instance.UploadAll();
BGCollectionManager.Instance.Collections["Users"].Upload();
```

You can also use the interval property to configure how often collections are uploaded in milliseconds. The default is 2000 milliseconds and setting it to 0 will disable automatic uploads.

```cs
BGCollectionManager.Instance.Interval = 4000;
```

## Error Handling

You can subscribe to actions in the collection manager to notify you when collections upload successfully or return errors.

```cs
BGCollectionManager.Instance.Error = (collection, errorCode) => {
  //...
};

BGCollectionManager.Instance.Success = (collection, successCount) => {
  //...
};
```

You can also provide error and success actions to an individual collection using the upload method.