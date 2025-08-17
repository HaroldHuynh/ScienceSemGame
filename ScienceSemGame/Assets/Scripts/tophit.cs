using UnityEngine;

public class tophit : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            Destroy(transform.parent.gameObject);
            if(collision.gameObject.TryGetComponent<player>(out player playerScript))
            {
            playerScript.Jump();
            }
        }
    }
}
