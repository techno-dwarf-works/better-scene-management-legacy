# AdvancedSceneManagement
Unity Scene Management

This plugin allows you to serialize scene asset in inspector for loaing via `SceneLoader`.

Usage
```c#
[SerializeField] private SceneLoaderAsset nextScene;

private async void Awake()
{
    await LoadSceneAsync(nextScene);
    //or
    await LoadSceneAsync(asset, new LoadSceneOptions()
        {
         SceneLoadMode = LoadSceneMode.Additive,
         SceneUnloadMode = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects,
         UseIntermediate = false,
         ProgressChanged = ProgressChanged
        });
}

private static void ProgressChanged(float value)
{ 

}
```
