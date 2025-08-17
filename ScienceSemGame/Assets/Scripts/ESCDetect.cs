using UnityEngine;

public class ESCDetect : MonoBehaviour
{
    [Tooltip("The GameObject to SHOW when ESCAPE is pressed.")]
    public GameObject shownObject1;
    public GameObject shownObject2;

    [Tooltip("The Gameobject(s) to HIDE when ESCAPE is pressed.")]
    public GameObject hiddenObject1;
    public GameObject hiddenObject2;

    void Update()
    {
        bool isActive = true;

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (shownObject1 != null)
            {
                shownObject1.SetActive(isActive);
            }

            if (shownObject2 != null)
            {
                shownObject2.SetActive(isActive);
            }

            if (hiddenObject1 != null)
            {
                hiddenObject1.SetActive(!isActive);
            }

            if (hiddenObject2 != null)
            {
                hiddenObject2.SetActive(!isActive);
            }
        }
    }
}