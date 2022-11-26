using Configs;
using Controllers;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Marker2D : MonoBehaviour
{
    private MarkerType MarkerType { get; set; }
    private string Title { get; set; }
    private string Comment { get; set; }
    private Vector3 Pos { get; set; }
    
    private GameObject _buttonTextGo;
    private GameObject _panelGo;
    private TMP_InputField _markerInputField;

    public void Initialize(MarkerType markerType, string title, string comment, Vector3 pos)
    {
        MarkerType = markerType;
        Title = title;
        Comment = comment;
        Pos = pos;

        //Add needed components
        var rectTransform = gameObject.AddComponent<RectTransform>();
        gameObject.AddComponent<CanvasRenderer>();
        var image = gameObject.AddComponent<Image>();
        var button = gameObject.AddComponent<Button>();

        //Set Image as parent
        gameObject.transform.SetParent(Config.ConfigGo.LoadedImg.transform);
        
        //Set Transform
        rectTransform.localPosition = Pos;
        rectTransform.localScale = Vector3.one;

        //Set Sprite
        image.sprite = MarkerType switch
        {
            MarkerType.Left => MarkerController.Instance.left,
            MarkerType.TopLeft => MarkerController.Instance.leftUp,
            MarkerType.Up => MarkerController.Instance.up,
            MarkerType.TopRight => MarkerController.Instance.rightUp,
            MarkerType.Right => MarkerController.Instance.right,
            MarkerType.BottomRight => MarkerController.Instance.rightDown,
            MarkerType.Down => MarkerController.Instance.down,
            MarkerType.BottomLeft => MarkerController.Instance.leftDown,
            MarkerType.Circle => MarkerController.Instance.circle,
            MarkerType.Square => MarkerController.Instance.square,
            _ => image.sprite
        };
        
        //Create EditPanel
        _panelGo = Instantiate(MarkerController.Instance.editPanelPrefab, MarkerController.Instance.parentTransform);
        var panelName = _panelGo.name = $"{Title}Panel";

        //Set Title
        gameObject.name = $"{Title}Button";

        //Open EditPanel
        button.onClick.AddListener(OpenPanel);

        //Delete Function
        var deleteButton = GameObject.Find($"/UI2D/EditPanelCanvas/{panelName}/EditPanel/VerwijderButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(DeleteMarker);

        //Save Function
        var saveButton = GameObject.Find($"/UI2D/EditPanelCanvas/{panelName}/EditPanel/OpslaanButton").GetComponent<Button>();
        saveButton.onClick.AddListener(SavePanel);

        //Cancel/Close Function
        var closeButton = GameObject.Find($"/UI2D/EditPanelCanvas/{panelName}/EditPanel/AnnuleerButton").GetComponent<Button>();
        closeButton.onClick.AddListener(ClosePanel);
        
        //Get PanelInput
        _markerInputField = GameObject.Find($"/UI2D/EditPanelCanvas/{panelName}/EditPanel/OpmerkingInput").GetComponent<TMP_InputField>();
        _markerInputField.text = Comment;
        
        //Get Title
        var markerTitle = GameObject.Find($"/UI2D/EditPanelCanvas/{panelName}/EditPanel/TitleText").GetComponent<TMP_Text>();
        markerTitle.text = Title;
        
        CreateButtonText();
    }

    private void CreateButtonText()
    {
        _buttonTextGo = new GameObject();

        var rectTransform = _buttonTextGo.AddComponent<RectTransform>();
        _buttonTextGo.AddComponent<CanvasRenderer>();
        var textMeshPro = _buttonTextGo.AddComponent<TextMeshProUGUI>();

        _buttonTextGo.name = "ButtonText";
        
        _buttonTextGo.transform.SetParent(gameObject.transform);
        
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.pivot = new Vector2(.5f, .5f);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.localPosition = new Vector3(0, 0, 0);
        rectTransform.localScale = Vector3.one;
        
        textMeshPro.text = Title;
        textMeshPro.fontSize = 20;
        textMeshPro.color = Color.black;
        textMeshPro.alignment = TextAlignmentOptions.Center;
    }
    
    private void OpenPanel()
    {
        _panelGo.SetActive(true);
        _panelGo.SetActive(true);
        
        _markerInputField.text = Comment;
    }

    public void ClosePanel()
    {
        _panelGo.SetActive(false);
        _panelGo.SetActive(false);
    }

    private void SavePanel()
    {
        Comment = _markerInputField.text;

        ClosePanel();
    }

    private void DeleteMarker()
    {
        ClosePanel();

        Destroy(_panelGo);
        Destroy(gameObject);
    }
}
