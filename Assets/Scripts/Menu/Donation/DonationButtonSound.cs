using UnityEngine;
using UnityEngine.UI;

public class DonationButtonSound : MonoBehaviour
{
    private AudioSource audio_s;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
        audio_s = GetComponent<AudioSource>();
    }

    private void TaskOnClick()
    {
        if (GlobalData.GetInt("Sound") != 0)
            audio_s.Play();
    }
}
