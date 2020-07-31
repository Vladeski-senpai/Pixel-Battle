using UnityEngine;
using UnityEngine.UI;

public class SettingsSoundButton : MonoBehaviour
{
    public Sprite[] sprites;

    private Image image;
    private Text txt;
    private AudioClip clip;
    private AudioSource 
        audio_s,
        music_obj;

    private int value;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
        image = GetComponent<Image>();
        audio_s = GetComponent<AudioSource>();
        clip = audio_s.clip;
        txt = transform.GetChild(0).GetComponent<Text>();

        CheckButtonCondition();
    }

    private void Start()
    {
        if (name.Substring(3) == "Music")
            music_obj = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
    }

    private void TaskOnClick()
    {
        if (GlobalData.GetInt("Sound") != 0) audio_s.PlayOneShot(clip);

        if (value == 0)
        {
            GlobalData.SetInt(name.Substring(3), 1);

            // Включаем музыку
            if (name.Substring(3) == "Music") music_obj.Play();
        }
        else
        {
            GlobalData.SetInt(name.Substring(3), 0);

            // Выключаем музыку
            if (name.Substring(3) == "Music") music_obj.Stop();
        }

        CheckButtonCondition();
    }

    public void CheckButtonCondition()
    {
        int id = 0; // Если 0, картинка "выключена"

        switch (name.Substring(3))
        {
            case "Music":
                value = GlobalData.GetInt("Music");
                // Если музыка включена
                if (value != 0)
                {
                    txt.text = "Music ON";
                    id = 1;
                }
                else txt.text = "Music OFF";
                break;

            case "Sound":
                value = GlobalData.GetInt("Sound");
                // Если звуки включены
                if (value != 0)
                {
                    txt.text = "Sound ON";
                    id = 1;
                }
                else txt.text = "Sound OFF";
                break;
        }

        image.sprite = sprites[id];
    }
}
