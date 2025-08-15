using UnityEngine;

public class RetroTitleEffect : MonoBehaviour
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
        // Smooth zoom
        float scaleFactor = 1 + Mathf.Sin(Time.time * zoomSpeed) * zoomAmount;
        transform.localScale = startScale * scaleFactor;

        // Tiny retro jitter
        transform.localPosition = startPos + new Vector3(
            Random.Range(-jitterAmount, jitterAmount),
            Random.Range(-jitterAmount, jitterAmount),
            0
        );
    }
}