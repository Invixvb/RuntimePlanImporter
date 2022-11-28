using System.Collections;
using Configs;
using UnityEngine;
using TriLibCore;
using TriLibCore.Extensions;

namespace Controllers
{
    public class LoadObjectController : MonoBehaviour
    {
        private const string StartingPath = "Assets/Resources/AssetsToLoad/building_04.obj";

        #region Singleton pattern
        private static LoadObjectController _instance;
        public static LoadObjectController Instance 
        {
            get 
            {
                if (!_instance)
                    _instance = FindObjectOfType<LoadObjectController>();
                return _instance;
            }
        }
        #endregion
        
        /// <summary>
        /// Standard Unity Start function.
        /// Load Game object if we're in the editor, for debug purposes.
        /// </summary>
        private void Start()
        {
            if (!Config.ConfigGo.LoadedGo)
                StartCoroutine(LoadObjectFromXam(StartingPath));
        }
        
        /// <summary>
        /// Load the Game object
        /// Thi gets sent from Xamarin so we know which path we need to load from.
        /// </summary>
        /// <param name="objPath"></param>
        /// <returns></returns>
        public IEnumerator LoadObjectFromXam(string objPath)
        {
            Config.ConfigGo.PublicObjectPath = objPath;
            
            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            AssetLoader.LoadModelFromFile(objPath, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);

            yield return null;
        }

        /// <summary>
        /// This adds the necessary config and components to the Game object
        /// </summary>
        private void AddConfigToObject()
        {
            Config.ConfigGo.LoadedGo.tag = "loadedOBJ";

            Config.ConfigGo.LoadedGo.AddComponent<MeshRenderer>();
            Config.ConfigGo.LoadedGo.isStatic = true;
            Config.ConfigGo.LoadedGo.layer = 3;
            
            foreach (Transform child in Config.ConfigGo.LoadedGo.transform)
            { 
                child.gameObject.layer = 3;
                child.gameObject.isStatic = true;
            }

            Config.ConfigGo.LoadedGo.name = "LoadedObject";
        }

        /// <summary>
        /// Called when any error occurs.
        /// </summary>
        /// <param name="obj">The contextualized error, containing the original exception and the context passed to the method where the error was thrown.</param>
        private void OnError(IContextualizedError obj)
        {
            Debug.LogError($"An error occurred while loading your Model: {obj.GetInnerException()}");
        }

        /// <summary>
        /// Called when the Model loading progress changes.
        /// </summary>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        /// <param name="progress">The loading progress.</param>
        private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
        {
            Debug.Log($"Loading Model. Progress: {progress:P}");
        }

        /// <summary>
        /// Called when the Model (including Textures and Materials) has been fully loaded.
        /// </summary>
        /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
        {
            Debug.Log("Materials loaded. Model fully loaded.");
            
            AddConfigToObject();
        }

        /// <summary>
        /// Called when the Model Meshes and hierarchy are loaded.
        /// </summary>
        /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        private void OnLoad(AssetLoaderContext assetLoaderContext)
        {
            if (Config.ConfigGo.LoadedGo)
                Destroy(Config.ConfigGo.LoadedGo);
            
            Config.ConfigGo.LoadedGo = assetLoaderContext.RootGameObject;
            
            if (Config.ConfigGo.LoadedGo)
                Camera.main.FitToBounds(assetLoaderContext.RootGameObject, 2f);
            
            Debug.Log("Model loaded. Loading materials.");
        }
    }
}