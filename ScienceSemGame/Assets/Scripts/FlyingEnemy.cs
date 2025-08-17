using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private Transform sight;

    public Transform nuggetSpawner;
    public GameObject chickenNuggetPrefab;
    private float shootspeed = 20f;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask wallLayer;

    private float attackCooldown;
    private float maxAttackCooldown = 2f;



        void FixedUpdate()
    {
        attackCooldown -= Time.deltaTime;
        Vector2 pDir = (player.transform.position - sight.transform.position).normalized;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(sight.transform.position, pDir, Vector2.Distance(sight.transform.position, player.transform.position), layerMask);
        //Debug.Log(raycastHit2D.collider.gameObject.layer);
        if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject.layer == 6)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            if ((player.transform.position.x - transform.position.x) / transform.localScale.x < 0)
            {
                Flip();
            }
            if (attackCooldown <= 0f)
            {
                float angle = Mathf.Atan2(pDir.y, pDir.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
                GameObject nuggetshot = Instantiate(chickenNuggetPrefab, nuggetSpawner.position, rotation);
                Rigidbody2D rb2D = nuggetshot.GetComponent<Rigidbody2D>();

                rb2D.AddForce(pDir.normalized * shootspeed, ForceMode2D.Impulse);
                attackCooldown = maxAttackCooldown;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(6 * transform.localScale.x, rb.linearVelocity.y);
            Collider2D hit = Physics2D.OverlapCircle(sight.position, 0.2f, wallLayer);
            if(hit == null)
            {
                return;
            }
            if (hit.gameObject.layer == 3 || hit.gameObject.layer == 0)
            {
                Flip();
            }
        }


    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

}
