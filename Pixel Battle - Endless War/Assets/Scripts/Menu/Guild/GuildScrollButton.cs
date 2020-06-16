using UnityEngine;
using UnityEngine.UI;

public class GuildScrollButton : MonoBehaviour
{
    public GuildScrollButton scroll_button; // Другая кнопка для скролла
    public GuildScrollMenu scroll_menu; // Тело для перемещения с кнопками покупок

    private Image image;
    private AudioSource audio_s;

    private bool
        isOn, // Включён ли звук
        isActive; // Активна ли кнопка

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
        audio_s = GetComponent<AudioSource>();
        image = GetComponent<Image>();

        if (GlobalData.GetInt("Sound") != 0) isOn = true;

        CheckActive();
    }

    public void CheckActive()
    {
        if (name.Substring(3) == "Up")
        {
            if (scroll_menu.state == 0)
            {
                isActive = false;
                image.color = new Color32(100, 100, 100, 255);
            }
            else
            {
                isActive = true;
                image.color = new Color32(255, 255, 255, 255);
            }
        }
        else
        {
            if (scroll_menu.state == 2)
            {
                isActive = false;
                image.color = new Color32(100, 100, 100, 255);
            }
            else
            {
                isActive = true;
                image.color = new Color32(255, 255, 255, 255);
            }
        }
    }

    private void TaskOnClick()
    {
        if (isActive)
        {
            // Звук нажатия
            if (isOn) audio_s.Play();

            if (name.Substring(3) == "Up")
                scroll_menu.Move(-1);
            else
                scroll_menu.Move(1);

            CheckActive();
            scroll_button.CheckActive();
        }
    }
}
