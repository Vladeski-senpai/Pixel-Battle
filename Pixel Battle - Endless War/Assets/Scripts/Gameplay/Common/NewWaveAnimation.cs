using UnityEngine;
using UnityEngine.UI;

public class NewWaveAnimation : MonoBehaviour
{
    [HideInInspector]
    public int wave_num = 1;

    private AudioSource audio_s;
    private GameObject glow_obj;
    private Text
        txt_wave,
        txt_wave_glow;

    private float
        speed = 5,
        currentY;

    private void Awake()
    {
        glow_obj = transform.GetChild(0).gameObject;
        txt_wave_glow = transform.GetChild(0).GetComponent<Text>();
        txt_wave = transform.GetChild(0).GetChild(0).GetComponent<Text>();

        // Если звук включён
        if (GlobalData.GetInt("Sound") != 0)
            audio_s = GetComponent<AudioSource>();
    }

    private void Update()
    {
        currentY = transform.localPosition.y;
        currentY = Mathf.Lerp(currentY, currentY + 4, speed + Time.deltaTime);
        transform.localPosition = new Vector2(transform.localPosition.x, currentY);

        if (currentY > 210)
            Stop();
    }

    private void Stop()
    {
        StopAllCoroutines();
        transform.localPosition = new Vector2(transform.localPosition.x, -235);
        glow_obj.SetActive(false);
    }

    public void Enable()
    {
        // Если звук "включён"
        if (audio_s != null)
            audio_s.Play();

        txt_wave.text = "WAVE " + wave_num;
        txt_wave_glow.text = "WAVE " + wave_num;
        transform.localPosition = new Vector2(transform.localPosition.x, -235);
        glow_obj.SetActive(true);
    }
}
