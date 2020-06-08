using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DefaultMenuButtons : MonoBehaviour
{
    public GameObject[] some_obj;

    private AudioSource audio_s;
    private Text txt; // Текст кнопки

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
        txt = transform.GetChild(0).GetComponent<Text>();

        // Переводим текст кнопки
        if (txt != null)
            txt.text = GlobalTranslateSystem.TranslateShortText(name.Substring(3));

        // Еслю звук включён
        if (GlobalData.GetInt("Sound") != 0)
            audio_s = GetComponent<AudioSource>();
    }

    private void TaskOnClick()
    {
        // Если звук "включён"
        if (audio_s != null)
            audio_s.Play();

        switch (name.Substring(3))
        {
            case "Pause":
                //some_obj[0]; //Music obj
                StartCoroutine(Timer("Pause", 0.1f));
                break;

            case "Resume":
                Time.timeScale = 1;
                StartCoroutine(Timer("Resume", 0.1f));
                break;

            // Загружаем главное меню
            case "Menu":
                Time.timeScale = 1;
                ScenesManager.scenes_manager.LoadLevel(0);
                break;
        }
    }

    private IEnumerator Timer(string code, float time)
    {
        yield return new WaitForSeconds(time);

        switch (code)
        {
            case "Pause":
                some_obj[0].SetActive(true);
                Time.timeScale = 0;
                break;

            case "Resume":
                some_obj[0].SetActive(false);
                break;
        }
    }
}
