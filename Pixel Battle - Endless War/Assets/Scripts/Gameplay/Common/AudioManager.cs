using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip
        ally_death_clip,
        enemy_death_clip;

    public bool IsMuicOn { get; private set; }

    private AudioSource audio_s;

    private const int max_hits = 10; // Максимум звуков удара
    private const int max_deaths = 8; // Максимум звуков смерти
    private const int max_spawns = 10; // Максимум звуков спавна
    private const int max_value = 10; // Различные звуки

    private float sound_destroy_delay = 0.2f;

    private int
        current_hits,
        current_deaths,
        current_spawns,
        current_value;

    private bool isOn; // Включены ли звуки

    private void Awake()
    {
        instance = this;
        audio_s = GetComponent<AudioSource>();

        if (GlobalData.GetInt("Sound") != 0) isOn = true;
        if (GlobalData.GetInt("Music") != 0) IsMuicOn = true;
    }

    // Проверяем можем ли запустить звук удара/выстрела
    public bool PlayHitSound()
    {
        if (isOn && current_hits < max_hits)
        {
            StartCoroutine(RemoveSound(0)); // Удаляем звук удара/выстрела из очереди
            current_hits++;
            return true;
        }
        else return false;
    }

    // Проверяем можем ли запустить звук смерти
    public void CheckDeathSound(bool isAlly)
    {
        if (isOn && current_deaths < max_deaths)
        {
            StartCoroutine(RemoveSound(1)); // Удаляем звук смерти
            current_deaths++;

            if (isAlly) PlayAllyDeath();
            else PlayEnemyDeath();
        }
    }

    // Играем звук смерти союзного юнита
    public void PlayAllyDeath()
    {
        audio_s.PlayOneShot(ally_death_clip);
    }

    // Играем звук смерти вражеского юнита
    public void PlayEnemyDeath()
    {
        audio_s.PlayOneShot(enemy_death_clip);
    }

    // Проверяем можем ли запустить звук спавна
    public bool PlaySpawnSound()
    {
        if (isOn && current_spawns < max_spawns)
        {
            StartCoroutine(RemoveSound(2)); // Удаляем звук спавна
            current_spawns++;
            return true;
        }
        else return false;
    }

    // Проверяем можем ли запустить звук спавна
    public bool PlayValueSound()
    {
        if (isOn && current_value < max_value)
        {
            StartCoroutine(RemoveSound(3)); // Удаляем звук спавна
            current_value++;
            return true;
        }
        else return false;
    }

    // Включён ли звук
    public bool IsOn()
    {
        if (isOn) return true;
        else return false;
    }

    // Удаляем звуки из очереди, есть следующие типы звуков: 0 - удар/выстрел, 1 - смерть, 2 - спавн, 3 - различные звуки
    private IEnumerator RemoveSound(byte type)
    {
        yield return new WaitForSeconds(sound_destroy_delay);

        switch (type)
        {
            case 0: current_hits--; break;
            case 1: current_deaths--; break;
            case 2: current_spawns--; break;
            case 3: current_value--; break;
        }
    }
}
