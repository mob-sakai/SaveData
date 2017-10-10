SaveData
===

Manage player's data in runtime and in editor.

* Create, Save, Load, Delete data
* Store Unity supported serialized fields to PlayerPrefs or local file
* Supports mobile device
* Multiple save datas in a device
* Available in runtime, also in editor
* SaveData viewer in editor.  
![image](https://user-images.githubusercontent.com/12690315/29917139-fda7bf3e-8e7b-11e7-8eca-4ab197ba0959.png)



## Requirement

* Unity 5.3+ *(included Unity 2017.x)*
* No other SDK are required



## Usage

1. Download [SaveData.unitypackage](https://github.com/mob-sakai/SaveData/master/SaveData.unitypackage) and install to your project.
1. 
1. Enjoy!

Use `YourSaveData` from code as follows.

```csharp
// Initialize and desplay current data.
// current data is loaded automatically.
YourSaveData.Initialize();
Debug.LogFormat("Current: {0}, {1}", YourSaveData.current.uniqueId, YourSaveData.current.sampleValue);

// Create new data.
YourSaveData.CreateEntity( "<uniqueId>" );
YourSaveData.current.sampleValue = 100;

// Change value and save.
YourSaveData.current.sampleValue = 200;
YourSaveData.SaveEntity();

// Load other data with id.
YourSaveData.LoadEntity( "<otherId>" );
Debug.LogFormat("Current: {0}, {1}", YourSaveData.current.uniqueId, YourSaveData.current.sampleValue);
```



## Multiple Environment?

In game development, not only **Production** environment but also **Development** environment is necessary.  
Database, API server, executable binary, file server, application ID, provisioning profile ... and *save data*.  
For example, like this.

* Development environment: data "Tommy" is exist in DB.
* Production environment: data "John" is exist in DB.

Probably, accessing the production environment with data "Tommy" will fail.  
There is no data　"Tommy" in the production environment.  

The SaveData class has a `path` property which indicates the save data save path.  
By using `path`, save data can be created for each environment.
```csharp
#if DEVELOP
const string PATH = "YourSaveDataPath_develop";
#else
const string PATH = "YourSaveDataPath";
#endif

public override string path { get { return PATH; } }
```


## Demo

You can have multiple save datas in a device!  
![image](https://user-images.githubusercontent.com/12690315/29918678-3ba4b422-8e81-11e7-917e-e7c3c993f437.png)  
[WebGL Demo](https://developer.cloud.unity3d.com/share/W1fv8sYS9f/)



## Release Notes

### ver.0.2.0

* Export and Import
* Encrypt and Decrypt

### ver.0.1.0

* Create, Save, Load, Delete data
* Multiple save datas in a device
* Create custom SaveData script with template
* SaveData viewer in editor.
* Add demo



## License
MIT



## Author
[mob-sakai](https://github.com/mob-sakai)
