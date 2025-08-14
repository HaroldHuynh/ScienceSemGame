using UnityEngine;

public class Framerate : MonoBehaviour
{
 

    public int targetFPS = 60; 

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
    }
}
