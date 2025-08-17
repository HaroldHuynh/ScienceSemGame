using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] Animator transitionAnim;
    private string SceneName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneName = sceneName;
        StartCoroutine(ExitScene());
    }
    IEnumerator ExitScene()
    {
        transitionAnim.SetTrigger("ExitScene");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(SceneName);
    }
}
