using UnityEngine;

public class EnterToolTipAnimation : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float zoomAmount = 0.05f;
    public float jitterAmount = 0.5f;

    private Vector3 startScale;
    private Vector3 startPos;

    void Start()
    {
        startScale = transform.localScale;
        startPos = transform.localPosition;
    }

    void Update()
    {
        // the smooth zoom
        float scaleFactor = 1 + Mathf.Sin(Time.time * zoomSpeed) * zoomAmount;
        transform.localScale = startScale * scaleFactor;

        // if you want it to jitter
        transform.localPosition = startPos + new Vector3(
            Random.Range(-jitterAmount, jitterAmount),
            Random.Range(-jitterAmount, jitterAmount),
            0
        );
    }
}
