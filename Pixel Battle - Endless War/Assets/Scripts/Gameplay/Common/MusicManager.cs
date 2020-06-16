using UnityEngine;

public class MusicManager : MonoBehaviour
{
    void Start()
    {
        if (name != "MusicController")
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("SoundManager");
            if (objs.Length > 1)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("SoundManager");
            if (objs.Length > 1)
                Destroy(objs[0].gameObject);
        }

        if (GlobalData.GetInt("Music") != 0)
            GetComponent<AudioSource>().Play();
    }
}
