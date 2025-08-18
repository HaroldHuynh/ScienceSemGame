using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnterHitBox : MonoBehaviour
{
    [SerializeField] private GameObject toolTip;
    [SerializeField] private string nextLevelName;
    [SerializeField] private AudioClip doorSound;
    public InputActionReference Interact;
    private bool inEnterZone;

    private void OnEnable()
    {
        Interact.action.started += Interact1;
    }

    private void OnDisable()
    {
        Interact.action.started -= Interact1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            toolTip.SetActive(true);
            inEnterZone = true;
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

    private void Interact1(InputAction.CallbackContext obj)
    {
        if (inEnterZone == true)
        {
            SoundManager.instance.PlaySoundFXClip(doorSound, transform, 1f);
            SceneController.instance.LoadScene(nextLevelName);
        }
    }
}