using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private Transform sight;

    public Transform nuggetSpawner;
    public GameObject chickenNuggetPrefab;
    private float shootspeed = 15f;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private int mode;
    private float pursuitTime;

    private float attackCooldown;
    private float maxAttackCooldown = 2f;

    void FixedUpdate()
    {
        attackCooldown -= Time.deltaTime;
        Vector2 pDir = (player.transform.position - sight.transform.position).normalized;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(sight.transform.position, pDir, Vector2.Distance(sight.transform.position, player.transform.position), layerMask);
        Debug.Log(raycastHit2D.collider.gameObject.layer);
        if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject.layer == 6)
        {
            mode = 1;
            pursuitTime = 3f;
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
            pursuitTime -= Time.deltaTime;
            if(pursuitTime <= 0f)
            {
                mode = 0;
            }
        }


    }
}
