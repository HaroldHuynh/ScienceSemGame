using UnityEngine;

public class EnterHitBox : MonoBehaviour
{
    [SerializeField] private GameObject toolTip;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            toolTip.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            toolTip.SetActive(false);
        }
    }
}
