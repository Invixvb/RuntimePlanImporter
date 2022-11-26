using System.Collections;
using Configs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class SceneController : MonoBehaviour
    {
        private AsyncOperation _asyncOperation;

        private Scene _currentScene;
        private string _sceneName;
        
        /// <summary>
        /// Used in a button, loads the Main Menu Scene. 
        /// </summary>
        public void StartMainMenuScene()
        {
            StartCoroutine(AsyncLoadScene("MainMenu"));
        }

        /// <summary>
        /// Used in a button, loads the Split view scene.
        /// </summary>
        public void StartSplitScene()
        {
            StartCoroutine(AsyncLoadScene("SplitViewScene"));
        }

        /// <summary>
        /// Used in a button, loads the AR scene.
        /// </summary>
        public void StartARScene()
        {
            StartCoroutine(AsyncLoadScene("ARScene"));
        }
        
        /// <summary>
        /// Used in a button, loads the Debug Menu scene.
        /// </summary>
        public void StartDebugScene()
        {
            StartCoroutine(AsyncLoadScene("DebugMenu"));
        }

        /// <summary>
        /// Here we set the Parent to the root project whenever we go to a scene that is not the SplitViewScene.
        /// So the MoveToScene works accordingly.
        /// </summary>
        private void SetParentToNull()
        {
            if (Config.ConfigGo.LoadedImg)
                Config.ConfigGo.LoadedImg.transform.SetParent(null);
        }
        
        /// <summary>
        /// Here we set the parent to the canvas if we are in the SplitViewScene.
        /// </summary>
        private void SetParent()
        {
            var imgCanvasTransform = GameObject.FindWithTag("loadedCanvas");

            if (!imgCanvasTransform || !Config.ConfigGo.LoadedImg)
                Debug.LogError("The canvas or Image isn't loaded, try again.");
            else
                Config.ConfigGo.LoadedImg.transform.SetParent(imgCanvasTransform.transform);
        }

        /// <summary>
        /// Here we Load in the scene and save our values. This happens every time we switch scenes.
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        private IEnumerator AsyncLoadScene(string sceneName)
        {
            _sceneName = sceneName;
            
            PlayerPrefs.Save();
            
            _currentScene = SceneManager.GetActiveScene();

            _asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            _asyncOperation.completed += AsyncOperationOnCompleted;

            yield return null;
        }

        /// <summary>
        /// This happens whenever the scene is loaded.
        /// Here we enable the newly loaded scene and make sure to set the parent or set it to the root.
        /// As well as move the objects to the new scene. And unload the old scene after.
        /// </summary>
        /// <param name="obj"></param>
        private void AsyncOperationOnCompleted(AsyncOperation obj)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneName));
            
            CameraMov3D.Instance.CreateTarget();

            if (SceneManager.GetActiveScene().name == "SplitViewScene" && Config.ConfigGo.LoadedImg)
            {
                SetParent();
            }
            else if (Config.ConfigGo.LoadedImg)
            {
                SetParentToNull();
                SceneManager.MoveGameObjectToScene(Config.ConfigGo.LoadedImg, SceneManager.GetSceneByName(_sceneName));
            }

            if (Config.ConfigGo.LoadedGo)
            {
                SceneManager.MoveGameObjectToScene(Config.ConfigGo.LoadedGo, SceneManager.GetSceneByName(_sceneName));
                SceneManager.MoveGameObjectToScene(Config.ConfigGo.LoadedTarget, SceneManager.GetSceneByName(_sceneName));
            }

            SceneManager.UnloadSceneAsync(_currentScene);
            
            GameManager.Instance.SetARMovScript();

            _asyncOperation.completed -= AsyncOperationOnCompleted;
        }
    }
}