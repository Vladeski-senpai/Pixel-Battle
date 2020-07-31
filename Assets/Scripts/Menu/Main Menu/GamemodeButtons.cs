using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GamemodeButtons : MonoBehaviour
{
    public GameObject[] other_outlines;
    public GameObject mode_menu;
    public Button play_button;

    private AudioSource audio_s;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
        audio_s = GetComponent<AudioSource>();
    }

    private void TaskOnClick()
    {
        if (GlobalData.GetInt("Sound") != 0) audio_s.Play();

        switch (name.Substring(3))
        {
            // Закрываем меню 
            case "Close":
                StartCoroutine(Timer());
                break;

            // Включаем меню
            case "Gamemode":
                mode_menu.SetActive(true);
                break;

            // Выбираем режим Классический
            case "Classic":
                GlobalData.SetInt("GameMode", 0);
                play_button.interactable = true;
                other_outlines[0].SetActive(true);
                other_outlines[1].SetActive(false);
                break;

            // Выбираем режим Арены
            case "Arena":
                GlobalData.SetInt("GameMode", 1);
                play_button.interactable = true;
                other_outlines[0].SetActive(true);
                other_outlines[1].SetActive(false);
                break;
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < other_outlines.Length; i++)
        {
            other_outlines[i].SetActive(false);
        }

        play_button.interactable = false;
        mode_menu.SetActive(false);
    }
}
