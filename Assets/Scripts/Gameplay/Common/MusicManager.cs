using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip
        default_battle_theme,
        arena_theme;

    void Awake()
    {
        // Контроллер в меню
        if (name != "MusicController")
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("SoundManager");
            if (objs.Length > 1)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        // Контроллер в игре
        else
        {
            // Находим и уничтожаем контроллеры из меню
            GameObject[] objs = GameObject.FindGameObjectsWithTag("SoundManager");
            if (objs.Length > 1)
                Destroy(objs[0].gameObject);
        }
    }

    private void Start()
    {
        // Контроллер в меню
        if (name != "MusicController")
        {
            if (GlobalData.GetInt("Music") != 0)
                GetComponent<AudioSource>().Play();
        }
    }

    public void PlayMusic(string code)
    {
        AudioSource audio_s = GetComponent<AudioSource>();

        switch (code)
        {
            // Стандартная боевая музыка
            case "Default":
                audio_s.clip = default_battle_theme;
                break;

            // Музыка для арены
            case "Arena":
                audio_s.clip = arena_theme;
                break;
        }

        audio_s.Play();
    }
}
