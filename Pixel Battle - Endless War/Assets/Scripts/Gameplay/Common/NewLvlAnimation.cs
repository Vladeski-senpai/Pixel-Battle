using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NewLvlAnimation : MonoBehaviour
{
    public static NewLvlAnimation new_lvl_animation;
    public GameObject glow_obj;

    private AudioSource audio_s;
    private Text txt;

    private float
        speed = 6,
        currentY;

    private bool isOn;

    private byte color;

    private void Awake()
    {
        new_lvl_animation = this;
        txt = glow_obj.GetComponent<Text>();
        audio_s = GetComponent<AudioSource>();

        if (GlobalData.GetInt("Sound") != 0) isOn = true;
    }

    private void Update()
    {
        currentY = transform.localPosition.y;
        currentY = Mathf.Lerp(currentY, currentY + 4, speed + Time.deltaTime);
        transform.localPosition = new Vector2(transform.localPosition.x, currentY);

        if (currentY > 210)
            Stop();
    }

    private IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(0.06f);

        color++;

        switch (color)
        {
            case 1:
                txt.color = new Color32(40, 255, 0, 255); // Зелёный
                break;

            case 2:
                txt.color = new Color32(255, 219, 0, 255); // Оранжевый
                break;

            case 3:
                txt.color = new Color32(255, 0, 14, 255); // Красный
                break;

            case 5:
                txt.color = new Color32(255, 0, 248, 255); // Фиолетовый
                break;

            case 4:
                txt.color = new Color32(0, 211, 255, 255); // Голубой
                break;
        }

        if (color == 5)
            color = 0;

        StartCoroutine(ChangeColor());
    }

    private void Stop()
    {
        StopAllCoroutines();
        transform.localPosition = new Vector2(transform.localPosition.x, -235);
        glow_obj.SetActive(false);
    }

    public void Enable()
    {
        // Звук
        if (isOn) audio_s.Play();

        transform.localPosition = new Vector2(transform.localPosition.x, -235);
        glow_obj.SetActive(true);
        StartCoroutine(ChangeColor());
    }
}
