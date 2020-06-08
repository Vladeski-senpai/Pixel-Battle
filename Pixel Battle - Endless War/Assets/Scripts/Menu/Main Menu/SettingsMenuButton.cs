using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuButton : MonoBehaviour
{
    public GameObject settings_menu;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        if (name.Substring(3) == "Settings")
            settings_menu.SetActive(true);
        else
            StartCoroutine(CloseMenu());
    }

    private IEnumerator CloseMenu()
    {
        yield return new WaitForSeconds(0.2f);

        settings_menu.SetActive(false);
    }
}
