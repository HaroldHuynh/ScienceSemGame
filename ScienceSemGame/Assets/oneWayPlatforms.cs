using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class OneWayPlatforms : MonoBehaviour
{
    private GameObject currentonewayplatform;

    [SerializeField] private BoxCollider2D playerCollider;

    public InputActionReference drop;

    private void OnEnable()
    {
        drop.action.started += Drop;
    }

    private void OnDisable()
    {
        drop.action.started -= Drop;
    }

    private void Drop(InputAction.CallbackContext obj)
    {
        if (currentonewayplatform != null)
        {
            StartCoroutine(DisableCollision());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentonewayplatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentonewayplatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentonewayplatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
