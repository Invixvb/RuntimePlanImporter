using Configs;
using UnityEngine;

public class CameraMov3D : MonoBehaviour
{
    private Transform _target;
    private Vector3 _targetOffset;
    
    private int _yMinLimit = -90;
    private int _yMaxLimit = 90;

    private float _zoomRate = 10.0f;
    
    private float _xDeg;
    private float _yDeg;
    private float _currentDistance;
    private float _desiredDistance;
    private Quaternion _currentRotation;
    private Quaternion _desiredRotation;
    private Quaternion _rotation;
    private Vector3 _position;
 
    private Vector3 _firstPosition;
    private Vector3 _secondPosition;
    private Vector3 _delta;
    private Vector3 _lastOffset;
    private Vector3 _lastOffsetTemp;
    
    #region Singleton pattern
    private static CameraMov3D _instance;
    public static CameraMov3D Instance
    {
        get 
        {
            if (!_instance)
                _instance = FindObjectOfType<CameraMov3D>();
            return _instance;
        }
    }
    #endregion

    /// <summary>
    /// Creates a camera target and assigns the camera's rotations and positions to the variables.
    /// </summary>
    public void CreateTarget()
    {
        //Create Camera target and assign variables
        if (!_target)
        {
            Config.ConfigGo.LoadedTarget = new GameObject("Cam Target");
            Config.ConfigGo.LoadedTarget.transform.position = transform.position + transform.forward * Config.Config3D.Distance;
            _target = Config.ConfigGo.LoadedTarget.transform;
        }
        
        Config.Config3D.Distance = Vector3.Distance(transform.position, _target.position);
        _currentDistance = Config.Config3D.Distance;
        _desiredDistance = Config.Config3D.Distance;
        
        _position = transform.position;
        _rotation = transform.rotation;
        _currentRotation = transform.rotation;
        _desiredRotation = transform.rotation;
 
        _xDeg = Vector3.Angle(Vector3.right, transform.right);
        _yDeg = Vector3.Angle(Vector3.up, transform.up);
    }

    /// <summary>
    /// Standard Unity Update function.
    /// Zooms in on pinch gesture. Rotates on one finger press gesture. And pans if 2 finger swipe over the screen.
    /// </summary>
    private void Update()
    {
        switch (Input.touchCount)
        {
            //Zoom
            case 2:
            {
                var touchZero = Input.GetTouch (0);
                var touchOne = Input.GetTouch (1);

                var touchZeroPreviousPosition = touchZero.position - touchZero.deltaPosition;
                var touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;

                var prevTouchDeltaMag = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;
                var touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
            
                var deltaMagDiff = prevTouchDeltaMag - touchDeltaMag;
                _desiredDistance += deltaMagDiff * Time.deltaTime * _zoomRate * Config.Config3D.SpeedZoom * Mathf.Abs(_desiredDistance);
                break;
            }
            
            //Rotate
            case 1 when Input.GetTouch(0).phase == TouchPhase.Moved:
            {
                var touchPosition = Input.GetTouch(0).deltaPosition;
                _xDeg += touchPosition.x * Config.Config3D.XSpeed * Config.Config3D.SpeedRotate;
                _yDeg -= touchPosition.y * Config.Config3D.YSpeed * Config.Config3D.SpeedRotate;
                _yDeg = ClampAngle(_yDeg, _yMinLimit, _yMaxLimit);
                break;
            }
        }
        _desiredRotation = Quaternion.Euler(_yDeg, _xDeg, 0);
        _currentRotation = transform.rotation;
        _rotation = Quaternion.Lerp(_currentRotation, _desiredRotation, Time.deltaTime * Config.Config3D.SpeedDampening);
        transform.rotation = _rotation;

        //Use panning and zooming
        if (Input.GetMouseButtonDown(1))
        {
            _firstPosition = Input.mousePosition;
            _lastOffset = _targetOffset;
        }

        if (Input.GetMouseButton(1))
        {
            _secondPosition = Input.mousePosition;
            _delta = _secondPosition - _firstPosition;
            _targetOffset = _lastOffset + transform.right * _delta.x * Config.Config3D.SpeedPan + transform.up * _delta.y * Config.Config3D.SpeedPan;
        }

        _desiredDistance = Mathf.Clamp(_desiredDistance, Config.Config3D.MinDistance, Config.Config3D.MaxDistance);
        _currentDistance = Mathf.Lerp(_currentDistance, _desiredDistance, Time.deltaTime * Config.Config3D.SpeedDampening);

        if (_target)
            _position = _target.position - _rotation * Vector3.forward * _currentDistance;

        _position -= _targetOffset;
        
        transform.position = _position;
    }

    /// <summary>
    /// Restricts negative rotation
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}