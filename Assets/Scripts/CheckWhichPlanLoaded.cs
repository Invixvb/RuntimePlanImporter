using Configs;
using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckWhichPlanLoaded : MonoBehaviour
{
    public GameObject arButton;
    public GameObject debugButton;
    public GameObject fullScreen2DButton;
    public GameObject fullScreen3DButton;
    public GameObject splitView2DButton;
    public GameObject splitView3DButton;

    private void Awake()
    {
        Invoke(nameof(CheckPlanLoaded), .001f);
    }

    public void CheckPlanLoaded()
    {
        if (SceneManager.GetActiveScene().name == "SplitViewScene")
        {
            if (Config.ConfigGo.LoadedImg && Config.ConfigGo.LoadedGo)
                ObjectLoaded();
            else if (Config.ConfigGo.LoadedImg && !Config.ConfigGo.LoadedGo)
                ObjectNotLoaded();
        }
    }

    private void ObjectLoaded()
    {
        debugButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-216, 85, 0);
            
        arButton.SetActive(true);
        fullScreen2DButton.SetActive(true);
        fullScreen3DButton.SetActive(true);
            
        SplitScreenController.Instance.SetSplitView();
    }

    private void ObjectNotLoaded()
    {
        debugButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-80, 85, 0);
            
        arButton.SetActive(false);
        fullScreen2DButton.SetActive(false);
        fullScreen3DButton.SetActive(false);
        splitView2DButton.SetActive(false);
        splitView3DButton.SetActive(false);
            
        SplitScreenController.Instance.Set2DFullScreen();
    }
}
