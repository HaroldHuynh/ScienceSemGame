using UnityEngine;  
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MainMenuCam : MonoBehaviour
{
    [Header("Pan Settings")]
    public float targetY = -10f;   
    public float duration = 2f;         
    public AnimationCurve easing;     

    [Header("Script To Enable")]
    public MonoBehaviour scriptToEnable;  
    public MonoBehaviour otherScriptToEnable;
    public MonoBehaviour otherOtherScriptToEnable;

    private Vector3 startPos;
    private Vector3 targetPos;
    private float elapsedTime = 0f;
    private bool isPanning = true;

    void Start()
    {
        startPos = transform.position;
        targetPos = new Vector3(startPos.x, targetY, startPos.z);

        if (easing == null || easing.length == 0)
            easing = AnimationCurve.EaseInOut(0, 0, 1, 1); 
    }

    void Update()
    {
        if (isPanning)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float easedT = easing.Evaluate(t);
            transform.position = Vector3.Lerp(startPos, targetPos, easedT);

            if (t >= 1f)
                isPanning = false;
        }

        if (!isPanning && Input.anyKey)
        {
            if (scriptToEnable != null)
                scriptToEnable.enabled = true; 
            if (otherScriptToEnable != null)
                otherScriptToEnable.enabled = true;
            if (otherOtherScriptToEnable != null)
                otherOtherScriptToEnable.enabled = true;

        }
    }
}

