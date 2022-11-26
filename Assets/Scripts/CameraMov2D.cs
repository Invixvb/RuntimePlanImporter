using Configs;
using UnityEngine;
using UnityEngine.UI;

public class CameraMov2D : ScrollRect
{
    private float _currentZoom = 1f;
    private bool _isPinching;
    private float _startPinchDist;
    private float _startPinchZoom;
    private Vector2 _startPinchCenterPosition;
    private Vector2 _startPinchScreenPosition;
    private bool _blockPan;

    public Camera camera;

    /// <summary>
    /// Standard Unity Start function.
    /// Gets the transform component and set it's values.
    /// </summary>
    protected override void Start()
    {
        if (Config.ConfigGo.LoadedImg)
        {
            var imageRect = Config.ConfigGo.LoadedImg.GetComponent<RectTransform>();
        
            if (!imageRect)
                Debug.LogError("The Image isn't loaded, try again!");

            imageRect.transform.localPosition = new Vector3(0, 0, 0);
            imageRect.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }
    }

    /// <summary>
    /// Standard Unity Update function.
    /// Makes sure the touches are blocked when needed. Does the zoom for pc. And smooths the zoom for mobile.
    /// </summary>
    private void Update()
    {
        if (!content && Config.ConfigGo.LoadedImg)
            content = (RectTransform) Config.ConfigGo.LoadedImg.transform;

        if (Input.touchCount == 2)
        {
            if (!_isPinching)
            {
                _isPinching = true;
                OnPinchStart();
            }

            OnPinch();
        }
        else
        {
            _isPinching = false;
            if (Input.touchCount == 0)
                _blockPan = false;
        }

        if (content && Config.ConfigGo.LoadedImg)
        {
            if (Mathf.Abs(content.localScale.x - _currentZoom) > 0.001f)
            {
                if (!content)
                    Debug.LogError("The Canvas Rect Content isn't loaded, try again!");
            
                content.localScale = Vector3.Lerp(content.localScale, Vector3.one * _currentZoom, Config.Config2D.SpeedDampening * Time.deltaTime);
            }   
        }

#if (UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR)
        if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > float.Epsilon)
        {
            _currentZoom *= 1 + Input.GetAxis("Mouse ScrollWheel") * 1;
            _currentZoom = Mathf.Clamp(_currentZoom, Config.Config2D.MinZoom, Config.Config2D.MaxZoom);
            _startPinchScreenPosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(content, _startPinchScreenPosition, camera, out _startPinchCenterPosition);
            Vector2 pivotPosition = new Vector3(content.pivot.x * content.rect.size.x, content.pivot.y * content.rect.size.y);
            var posFromBottomLeft = pivotPosition + _startPinchCenterPosition;
            SetPivot(content, new Vector2(posFromBottomLeft.x / content.rect.width, posFromBottomLeft.y / content.rect.height));
        }
#endif
    }

    /// <summary>
    /// Makes sure touches from zoom and panning don't interfere each other and blocks them when needed.
    /// </summary>
    /// <param name="position"></param>
    protected override void SetContentAnchoredPosition(Vector2 position)
    {
        if (_isPinching || _blockPan) return;
        base.SetContentAnchoredPosition(position);
    }

    /// <summary>
    /// Gets and sets the screen and finger position. And determines the size of the view box/canvas.
    /// </summary>
    private void OnPinchStart()
    {
        var pos1 = Input.touches[0].position;
        var pos2 = Input.touches[1].position;

        _startPinchDist = Distance(pos1, pos2) * content.localScale.x;
        _startPinchZoom = _currentZoom;
        _startPinchScreenPosition = (pos1 + pos2) / 2;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(content, _startPinchScreenPosition, camera, out _startPinchCenterPosition);

        Vector2 pivotPosition = new Vector3(content.pivot.x * content.rect.size.x, content.pivot.y * content.rect.size.y);
        var posFromBottomLeft = pivotPosition + _startPinchCenterPosition;

        SetPivot(content, new Vector2(posFromBottomLeft.x / content.rect.width, posFromBottomLeft.y / content.rect.height));
        _blockPan = true;
    }

    /// <summary>
    /// Does the zoom
    /// </summary>
    private void OnPinch()
    {
        var currentPinchDist = Distance(Input.touches[0].position, Input.touches[1].position) * content.localScale.x;
        _currentZoom = currentPinchDist / _startPinchDist * _startPinchZoom;
        _currentZoom = Mathf.Clamp(_currentZoom, Config.Config2D.MinZoom, Config.Config2D.MaxZoom);
    }

    /// <summary>
    /// Gets the distance between 2 points, and essentially gets the distance between the 2 fingers.
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    /// <returns></returns>
    private float Distance(Vector2 pos1, Vector2 pos2)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(content, pos1, camera, out pos1);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(content, pos2, camera, out pos2);
        return Vector2.Distance(pos1, pos2);
    }

    /// <summary>
    /// Gets the size and pivots of the view box/canvas
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <param name="pivot"></param>
    private static void SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        if (!rectTransform) return;

        var size = rectTransform.rect.size;
        var deltaPivot = rectTransform.pivot - pivot;
        var deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y) * rectTransform.localScale.x;
        rectTransform.pivot = pivot;
        rectTransform.localPosition -= deltaPosition;
    }
}