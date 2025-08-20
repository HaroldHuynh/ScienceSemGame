using UnityEngine;

public class chicken_nugget : MonoBehaviour
{
    Collider2D[] inExplosionRadius = null;
    [SerializeField] private float explosionStrength = 5;
    [SerializeField] private float explosionRadius = 3;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D o in inExplosionRadius) 
        {
            Debug.Log(o.gameObject.layer);
            Rigidbody2D oRB = o.GetComponent<Rigidbody2D>();
            if (oRB!= null)
            {
                Vector2 distanceVector = o.transform.position - transform.position;
            if (distanceVector.magnitude > 0) {
                    float explosionForce = explosionStrength / distanceVector.magnitude;
                    oRB.AddForce(distanceVector.normalized * explosionForce, ForceMode2D.Impulse);
                }
            }
        if (o.gameObject.TryGetComponent<player>(out player hit))
        {
            hit.Damaged();
        }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
