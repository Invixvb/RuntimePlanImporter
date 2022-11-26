using UnityEngine;

public class FPSCapFix : MonoBehaviour
{
    /// <summary>
    /// Standard Unity Awake function.
    /// Makes sure the fps is capped at 60 fps. For a smooth experience.
    /// </summary>
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}
