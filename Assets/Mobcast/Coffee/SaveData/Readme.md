SaveData
===

Manage player's data in runtime and in editor.

* Create, Save, Load, Delete data
* Store Unity supported serialized fields to PlayerPrefs or local file
* Supports mobile device
* Multiple datas in a device
* Available in runtime, also in editor
* SaveData viewer in editor.  
![image](https://user-images.githubusercontent.com/12690315/29917139-fda7bf3e-8e7b-11e7-8eca-4ab197ba0959.png)



## Requirement

* Unity 5.3+ *(included Unity 2017.x)*
* No other SDK are required



## Usage

1. Download [SaveData.unitypackage](../master/SaveData.unitypackage) and install to your project.
1. Enjoy!

Use `YourSaveData` from code as follows.
```cs
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


## Demo

[WebGL Demo](https://developer.cloud.unity3d.com/share/W1fv8sYS9f/)



## Release Notes

### ver.0.1.0

* Create, Save, Load, Delete data
* Multiple datas in a device
* Create custom SaveData script with template
* SaveData viewer in editor.
* Add demo



## License
MIT



## Author
[mob-sakai](https://github.com/mob-sakai)