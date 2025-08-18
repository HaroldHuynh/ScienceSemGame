using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class crumbling : MonoBehaviour
{
    [SerializeField] private float crumblingTime = 1.5f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(Crumble());

    }

    private IEnumerator Crumble()
    {
        yield return new WaitForSeconds(crumblingTime);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(crumblingTime * 4);
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
