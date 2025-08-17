using UnityEngine;

public class EnterHitBox : MonoBehaviour
{
    [SerializeField] private GameObject toolTip;
    [SerializeField] private string nextLevelName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            toolTip.SetActive(true);
            SceneController.instance.LoadScene(nextLevelName);
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
