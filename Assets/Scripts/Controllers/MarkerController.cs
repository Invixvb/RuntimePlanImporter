using System;
using Enums;
using UnityEngine;

namespace Controllers
{
    public class MarkerController : MonoBehaviour
    {
        public GameObject panel;
        public GameObject shadow;

        private bool _clicking;
        
        private float _totalDownTime;
        private const float ClickDuration = .5f;

        private Touch _touch;

        private Vector2 _openedPanelPos;

        private int _titleAmount;

        public GameObject editPanelPrefab;

        public Transform parentTransform;

        #region MarkerType Sprites
        public Sprite circle;
        public Sprite square;
        public Sprite up;
        public Sprite rightUp;
        public Sprite right;
        public Sprite rightDown;
        public Sprite down;
        public Sprite leftDown;
        public Sprite left;
        public Sprite leftUp;
        #endregion
        
        #region Singleton pattern
        private static MarkerController _instance;
        public static MarkerController Instance 
        {
            get 
            {
                if (!_instance)
                    _instance = FindObjectOfType<MarkerController>();
                return _instance;
            }
        }
        #endregion

        private void Update()
        {
            if (Input.touchCount == 1)
            {
                _touch = Input.GetTouch(0);
                
                switch (_touch.phase)
                {
                    case TouchPhase.Began:
                    {
                        _totalDownTime = 0;
                        _clicking = true;
                        break;
                    }
                    case TouchPhase.Stationary when _clicking:
                    {
                        _totalDownTime += Time.deltaTime;

                        if (_totalDownTime >= ClickDuration)
                        {
                            panel.transform.position = _touch.rawPosition;
                            shadow.transform.position = _touch.rawPosition;
                        
                            panel.SetActive(true);
                            shadow.SetActive(true);

                            _clicking = false;
                            
                            _openedPanelPos = _touch.rawPosition;
                        }

                        break;
                    }
                    case TouchPhase.Ended when _clicking:
                    {
                        panel.SetActive(false);
                        shadow.SetActive(false);

                        _clicking = false;
                        break;
                    }
                }
            }
        }

        public void PlaceMarker(string markerType)
        {
            Enum.TryParse(markerType, out MarkerType markerTypeEnum);

            var screenWidth = Screen.width / 2;

            if (_openedPanelPos.x < screenWidth)
            {
                var marker2Dgo = new GameObject();
                var marker2D = marker2Dgo.AddComponent<Marker2D>();
                
                marker2D.Initialize(markerTypeEnum, $"MarkerTitle{_titleAmount}", "MarkerComment", _openedPanelPos);

                _titleAmount++;
            }
            else if (_openedPanelPos.x > screenWidth)
            {
                //Do place 3D marker logic
                
                
            }
        }
    }
}
