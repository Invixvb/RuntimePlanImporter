using System.Collections;
using System.Threading.Tasks;
using Configs;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers
{
    public class LoadImageController : MonoBehaviour
    {
        private const string StartingPath = @"C:\Users\levis\Documents\c4784584-bb57-428d-8019-a258c936fbdb_copy.jpg";

        #region Singleton pattern
        private static LoadImageController _instance;
        public static LoadImageController Instance 
        {
            get 
            {
                if (!_instance)
                    _instance = FindObjectOfType<LoadImageController>();
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// Standard Unity Start function.
        /// Load Image if we're in the editor, for debug purposes.
        /// </summary>
        private void Start()
        {
#if UNITY_EDITOR
            if (!Config.ConfigGo.LoadedImg)
                StartCoroutine(LoadImageFromXam(StartingPath));
#endif
        }
        
        /// <summary>
        /// Here we load in the image and set it to a texture from the newly created gameobject.
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public IEnumerator LoadImageFromXam(string imagePath)
        {
            Config.ConfigGo.PublicImagePath = imagePath;
            
            var unityWebRequest = UnityWebRequestTexture.GetTexture("file://"+imagePath);
            unityWebRequest.SendWebRequest();
            
            while (!unityWebRequest.isDone)
            {
                Task.Delay(50).Wait();
                yield return null;
            }

            var texture = DownloadHandlerTexture.GetContent(unityWebRequest);

            Config.ConfigGo.LoadedImg = new GameObject("LoadedImage");
            
            if (!Config.ConfigGo.LoadedImg)
                Debug.LogError("This Image is not available, make sure the file is selected!");
            
            AddConfigToImage();
            
            Config.ConfigGo.LoadedImg.GetComponent<RawImage>().texture = texture;

            SetFileName.Instance.SetName();
        }
        
        /// <summary>
        /// Here we add the necessary config and components to the image.
        /// </summary>
        private void AddConfigToImage()
        {
            Config.ConfigGo.LoadedImg.tag = "loadedImage";
            
            var imageRect = Config.ConfigGo.LoadedImg.AddComponent<RectTransform>();
            Config.ConfigGo.LoadedImg.AddComponent<CanvasRenderer>();
            Config.ConfigGo.LoadedImg.AddComponent<RawImage>();

            imageRect.sizeDelta = new Vector2 (3160, 2235);
        }
    }
}
