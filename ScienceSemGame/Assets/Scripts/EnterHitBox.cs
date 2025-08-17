using UnityEngine;
using UnityEngine.InputSystem;

public class EnterHitBox : MonoBehaviour
{
    [SerializeField] private GameObject toolTip;
    [SerializeField] private string nextLevelName;
    public InputActionReference Interact;
    private bool inEnterZone = false;

    private void OnEnable()
    {
        if (inEnterZone)
        {
            //Interact.action.started += LoadSceneFunc;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            toolTip.SetActive(true);
            inEnterZone = true;
            SceneController.instance.LoadScene(nextLevelName);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            toolTip.SetActive(false);
            inEnterZone = false;
        }
    }
}