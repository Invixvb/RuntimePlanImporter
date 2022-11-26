using Configs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton pattern
        private static GameManager _instance;
        public static GameManager Instance 
        {
            get 
            {
                if (!_instance)
                    _instance = FindObjectOfType<GameManager>();
                return _instance;
            }
        }
        #endregion
        
        /// <summary>
        /// Standard Unity Awake function.
        /// Indicates the system handles multiple touches.
        /// </summary>
        protected void Awake()
        {
            Input.multiTouchEnabled = true;
        }

        /// <summary>
        /// Here we check if the Game object is in what scene.
        /// And according to that we tell the manager to delete or add the AR Movement script.
        /// </summary>
        public void SetARMovScript()
        {
            if (Config.ConfigGo.LoadedGo)
            {
                if (SceneManager.GetActiveScene().name == "ARScene")
                    Config.ConfigGo.LoadedGo.AddComponent<CameraMovAR>();
                else
                    Destroy(Config.ConfigGo.LoadedGo.GetComponent<CameraMovAR>());
            }
        }

        public void SendFinish()
        {
            XamarinSendMessage.SendAndroid("ReceiveFinish");
            
#if !UNITY_EDITOR && UNITY_IOS
            XamarinSendMessage.receiveFinish();
#else
            Debug.LogError("Error: Method isn't called from an iOS device");
#endif
        }
    }
}