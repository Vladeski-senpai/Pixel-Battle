using System.Collections;
using UnityEngine;

public class DarkForestFogSpawner : MonoBehaviour
{
    public GameObject fog_obj;
    public Sprite[] fogs_sprites; // Спрайты туманов

    [Space]
    public Transform particles_trashcan;

    private GameObject temp;
    private float spawn_time = 6;

    private void Start()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(spawn_time);

        temp = Instantiate(fog_obj, new Vector2(-15, Random.Range(-3.26f, 1.46f)), Quaternion.identity, particles_trashcan);
        temp.GetComponent<SpriteRenderer>().sprite = fogs_sprites[Random.Range(0, fogs_sprites.Length)];

        spawn_time = Random.Range(3, 9);
        StartCoroutine(Timer());
    }
}
