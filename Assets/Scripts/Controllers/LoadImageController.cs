using System.Collections;
using Configs;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class LoadImageController : MonoBehaviour
    {
        public Texture2D loadedTexture;

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
            if (!Config.ConfigGo.LoadedImg)
                StartCoroutine(LoadImageFromXam());
        }
        
        /// <summary>
        /// Here we load in the image and set it to a texture from the newly created gameobject.
        /// </summary>
        private IEnumerator LoadImageFromXam()
        {
            var texture = loadedTexture;

            Config.ConfigGo.LoadedImg = new GameObject("LoadedImage");
            
            if (!Config.ConfigGo.LoadedImg)
                Debug.LogError("This Image is not available, make sure the file is selected!");
            
            AddConfigToImage();
            
            Config.ConfigGo.LoadedImg.GetComponent<RawImage>().texture = texture;

            SetFileName.Instance.SetName();

            yield return null;
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
