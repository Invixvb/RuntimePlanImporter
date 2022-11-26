using UnityEngine;

namespace Controllers
{
    public class SplitScreenController : MonoBehaviour
    {
        public Camera camera2D;
        public Camera camera3D;

        private bool _isSplitView = true;

        [HideInInspector]public CameraMov2D cameraMov2D;
        [HideInInspector]public CameraMov3D cameraMov3D;
        
        #region Singleton pattern
        private static SplitScreenController _instance;
        public static SplitScreenController Instance 
        {
            get 
            {
                if (!_instance)
                    _instance = FindObjectOfType<SplitScreenController>();
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// Standard Unity Start function.
        /// We find and set the camera's and make sure they aren't null.
        /// </summary>
        private void Start()
        {
            cameraMov2D = GameObject.FindWithTag("loadedCanvas").GetComponent<CameraMov2D>();
            cameraMov3D = GameObject.Find("Camera3D").GetComponent<CameraMov3D>();
            
            if (!cameraMov2D || !camera3D)
                Debug.LogError("The canvas or camera3D isn't loaded, try again!");
        }

        /// <summary>
        /// Standard Unity Update function.
        /// Here we check where the player touches on the screen so we can enable the right movement script accordingly.
        /// </summary>
        private void Update()
        {
            if (Input.touchCount > 0 && _isSplitView)
            {
                var screenWidth = Screen.width / 2;
                var touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.position.x < screenWidth)
                    {
                        cameraMov2D.enabled = true;
                        cameraMov3D.enabled = false;
                    }
                    else if (touch.position.x > screenWidth)
                    {
                        cameraMov2D.enabled = false;
                        cameraMov3D.enabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Here we enable the 2D full screen view if we press the button.
        /// </summary>
        public void Set2DFullScreen()
        {
            camera2D.rect = new Rect(0, 0, 1, 1);
            camera3D.rect = new Rect(0, 0, 0, 1);
            
            cameraMov2D.enabled = true;
            cameraMov3D.enabled = false;

            _isSplitView = false;
        }        
        
        /// <summary>
        /// Here we enable the 3D full screen view if we press the button.
        /// </summary>
        public void Set3DFullScreen()
        {
            camera2D.rect = new Rect(0, 0, 0, 1);
            camera3D.rect = new Rect(0, 0, 1, 1);                                                                 
            
            cameraMov2D.enabled = false;
            cameraMov3D.enabled = true;
            
            _isSplitView = false;
        }        
        
        /// <summary>
        /// Here we enable the split view if we press the button.
        /// </summary>
        public void SetSplitView()
        {
            camera2D.rect = new Rect(0, 0, .5f, 1);
            camera3D.rect = new Rect(.5f, 0, .5f, 1);
            
            cameraMov2D.enabled = true;
            cameraMov3D.enabled = true;
            
            _isSplitView = true;
        }
    }
}
