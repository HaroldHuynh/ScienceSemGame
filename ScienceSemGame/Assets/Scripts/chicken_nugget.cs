using UnityEngine;

public class chicken_nugget : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        if (collision.gameObject.TryGetComponent<player>(out player hit))
        {
            hit.Damaged();
        }
    }

}
