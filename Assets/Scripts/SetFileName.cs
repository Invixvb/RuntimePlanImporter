using System.IO;
using Configs;
using TMPro;
using UnityEngine;

public class SetFileName : MonoBehaviour
{
    public TextMeshProUGUI fileTitle;
    
    #region Singleton pattern
    private static SetFileName _instance;
    public static SetFileName Instance 
    {
        get 
        {
            if (!_instance)
                _instance = FindObjectOfType<SetFileName>();
            return _instance;
        }
    }
    #endregion

    private void Start()
    {
        if (Config.ConfigGo.LoadedImg)
            SetName();
    }

    public void SetName()
    {
        fileTitle.text = Path.GetFileName(Config.ConfigGo.PublicImagePath);
    }
}
