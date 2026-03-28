## `UltralightUnity`

It is a bride between unity and `ultralight v1.4`  

Currently, it is working on `Linux` & `Windows`  

## Getting started

Requires: `Unity Addressables`
- If you didn't have addressables before, you need to configure it.
- Window -> Asset Management -> Addressables -> Groups
- Create Addressables Settings

Requires: `Unity Old Input System`
- Project settings -> Player -> Other Settings -> Configuration -> Active Input Handling
- Set `Active Input Handling` to `Both` or `Old` depending or your needs.
- If you don't want old input system, edit `ULInputSender` and `ExampleSiteController`. (Maybe I will add support for the new system. But feel free to upload yours.)

Download `unitySample.unitypackage` from Releases  
Import it into your project  
Read README.md inside sample  
Open example scene inside `Latacko/Ultralight/Example/Scenes/UltralightSample`

Required compontents are:
- UltralightManager
- UnityFileSystem
- ULView

Ready prefabs:
- UltralightManager
- WebView (fullscreen browser)

## Contribute
c# sdk .net 10

## License

`UltralightUnity` and its subprojects are MIT.

This project depends on `Ultralight`, which is licensed separately under the Ultralight Free License Agreement or a Commercial License from Ultralight, Inc.

Ultralight is not open source.

Please review Ultralight's official licensing terms here:
[ultralig.ht](https://ultralig.ht) 
