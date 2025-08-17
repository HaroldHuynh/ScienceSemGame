using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CameraBound : MonoBehaviour
{
    public float minX, maxX, minY, maxY; 

    void LateUpdate() 
    {
        Vector3 clampedPosition = transform.position;

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        transform.position = clampedPosition;
    }
}
