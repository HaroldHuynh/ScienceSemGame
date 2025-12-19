using UnityEngine;

public class JumpPad : MonoBehaviour
{

    [SerializeField] private float bounce = 20f;
    [SerializeField] private AudioClip bounceSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == 6)
        {

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
            SoundManager.instance.PlaySoundFXClip(bounceSound, transform, 1f);
        }
    }
}
