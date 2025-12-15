using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnterHitBox : MonoBehaviour
{
    [SerializeField] private GameObject toolTip;
    [SerializeField] private string nextLevelName;
    [SerializeField] private AudioClip doorSound;
    [SerializeField] private float waitTime = 1.5f;
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
            //StartCoroutine(waitForDoor());
            SceneController.instance.LoadScene(nextLevelName);

        }
    }

    private IEnumerator waitForDoor()
    {
        yield return new WaitForSeconds(waitTime);

    }
}