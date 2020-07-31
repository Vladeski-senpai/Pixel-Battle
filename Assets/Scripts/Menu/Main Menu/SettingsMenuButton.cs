using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuButton : MonoBehaviour
{
    public GameObject settings_menu;

    private AudioSource audio_s;

    private void Awake()
    {
        audio_s = GetComponent<AudioSource>();
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        // Звук нажатия
        if (GlobalData.GetInt("Sound") != 0) audio_s.Play();

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
