using System.Collections;
using UnityEngine;

public class DarkForestFogSpawner : MonoBehaviour
{
    public GameObject 
        ambient_obj,
        firefly_obj,
        fog_obj;


    public Sprite[] fogs_sprites; // Спрайты туманов

    [Space]
    public Transform particles_trashcan;

    private GameObject temp;

    private void Start()
    {
        // Включаем эмбиент звуки (ветер и дарк-эмбиент)
        if (AudioManager.instance.IsOn()) ambient_obj.SetActive(true);

        StartCoroutine(FogTimer(1));
        StartCoroutine(FireflyTimer(Random.Range(5, 10)));
    }

    private IEnumerator FogTimer(float time)
    {
        yield return new WaitForSeconds(time);

        temp = Instantiate(fog_obj, new Vector2(-15, Random.Range(-3.26f, 1.46f)), Quaternion.identity, particles_trashcan);
        temp.GetComponent<SpriteRenderer>().sprite = fogs_sprites[Random.Range(0, fogs_sprites.Length)];

        StartCoroutine(FogTimer(Random.Range(3f, 8f)));
    }

    private IEnumerator FireflyTimer(float time)
    {
        yield return new WaitForSeconds(time);

        temp = Instantiate(firefly_obj, new Vector2(-10, Random.Range(-1.42f, 1.46f)), Quaternion.identity, particles_trashcan);
        temp.GetComponent<Animator>().SetTrigger("move " + Random.Range(1, 5));
        temp.transform.GetChild(0).GetComponent<Animator>().SetTrigger("flick " + Random.Range(1, 7));

        StartCoroutine(FireflyTimer(Random.Range(10, 15)));
    }
}
