using System.Collections;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager instance;

    public GameObject enemy_slider;
    public NewWaveAnimation wave_anim;

    //[HideInInspector]
    public int wave_num = 1; // Текущий норме волны
    
    [HideInInspector]
    public bool isArenaOn;

    private int wave_record; // Рекорд арены на начало уровня

    private void Awake()
    {
        instance = this;

        // Если режим Арена
        if (GlobalData.GetInt("GameMode") == 1)
        {
            isArenaOn = true;
            wave_record = GlobalData.GetInt("MaxWave");
        }
        else
        {
            enabled = false;
        }
    }

    private void Start()
    {
        if (isArenaOn)
        {
            enemy_slider.SetActive(false); // Выключаем слайдер хп врага
            StartCoroutine(WaveTimer(Random.Range(15, 25)));
            DefaultGameController.default_controller.isArenaOn = true;
        }
    }

    // Новая волна
    private IEnumerator WaveTimer(float time)
    {
        yield return new WaitForSeconds(time);

        wave_num++;
        wave_anim.wave_num = wave_num;
        wave_anim.Enable();
        StartCoroutine(WaveTimer(Random.Range(15, 25)));

        // Рекорд арены
        if (wave_num > wave_record)
        {
            wave_record = wave_num;
            GlobalData.SetInt("MaxWave", wave_num);
        }
    }
}
