# [Deprecated] Better Scene Management

> [!CAUTION]
> Package deprecated and replaced with - [Better Scene Management](https://github.com/techno-dwarf-works/better-scene-management)

[![openupm](https://img.shields.io/npm/v/com.uurha.betterscenemanagement?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.uurha.betterscenemanagement/)

This plugin allows you to serialize scene asset in inspector for loaing via `SceneLoader`.

## Usage
```c#
[SerializeField] private SceneLoaderAsset nextScene;

private async void Awake()
{
    await SceneLoader.LoadSceneAsync(nextScene);
    //or
    await SceneLoader.LoadSceneAsync(asset, new LoadSceneOptions()
        {
         SceneLoadMode = LoadSceneMode.Additive,
         UseIntermediate = false,
         ProgressChanged = ProgressChanged
        });
}

private static void ProgressChanged(float value)
{ 

}
```

## Install
[How to install](https://github.com/uurha/BetterPluginCollection/wiki/How-to-install)
