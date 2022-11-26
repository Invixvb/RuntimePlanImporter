using Configs;
using UnityEngine;

public class CameraMovAR : MonoBehaviour
{
    private float _xDeg;
    private Quaternion _currentRotation;
    private Quaternion _desiredRotation;
    private Quaternion _rotation;
 
    private Vector3 _firstPosition;
    private Vector3 _secondPosition;
    private Vector3 _delta;
    private Vector3 _lastOffset;
    private Vector3 _lastOffsetTemp;
    
    private float _initialDistance;
    private Vector3 _initialScale;

    /// <summary>
    /// Standard Unity Start function.
    /// Assigns the rotations and positions to the variables.
    /// </summary>
    private void Start()
    {
        _rotation = transform.rotation;
        _currentRotation = transform.rotation;
        _desiredRotation = transform.rotation;

        _xDeg = Vector3.Angle(Vector3.right, transform.right);
    }

    /// <summary>
    /// Standard Unity Update function.
    /// Zooms in on pinch gesture. Rotates on one finger press gesture.
    /// </summary>
    private void Update()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            var touchPosition = Input.GetTouch(0).deltaPosition;
            _xDeg -= touchPosition.x * Config.ConfigAR.Speed * Config.ConfigAR.SpeedRotate;
        }
        
        _desiredRotation = Quaternion.Euler(0, _xDeg, 0);
        _currentRotation = transform.rotation;
        _rotation = Quaternion.Lerp(_currentRotation, _desiredRotation, Time.deltaTime * Config.ConfigAR.SpeedDampening);
        transform.rotation = _rotation;

        if (Input.touchCount == 2)
        {
            var touchZero = Input.GetTouch(0);
            var touchOne = Input.GetTouch(1);

            if (touchZero.phase is TouchPhase.Ended or TouchPhase.Canceled ||
                touchOne.phase is TouchPhase.Ended or TouchPhase.Canceled)
                return;

            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
            {
                if (!Config.ConfigGo.LoadedGo)
                    Debug.LogError("The Object isn't loaded, try again!");
                
                _initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
                _initialScale = Config.ConfigGo.LoadedGo.transform.localScale;
            }
            else
            {
                var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);
                if (Mathf.Approximately(_initialDistance, 0))
                    return;

                var factor = currentDistance / _initialDistance;
                Config.ConfigGo.LoadedGo.transform.localScale = _initialScale * factor;
            }
        }
        
        /*if (Input.touchCount == 2)
        {
            var touchZero = Input.GetTouch(0);
            var touchOne = Input.GetTouch(1);

            var touchZeroPreviousPosition = touchZero.position - touchZero.deltaPosition;
            var touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;

            var prevTouchDeltaMag = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;
            var touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            var deltaMagDiff = prevTouchDeltaMag - touchDeltaMag;
        }*/
    }
}
