using System.Collections;
using UnityEngine;

public class DesertSpawner : MonoBehaviour
{
    public GameObject grass;

    private void Start()
    {
        StartCoroutine(Spawn(20));
    }

    private IEnumerator Spawn(float time)
    {
        yield return new WaitForSeconds(time);

        Instantiate(grass, new Vector2(Random.Range(-13.0f, -11.0f), Random.Range(-3.9f, 0.8f)), Quaternion.identity);
        StartCoroutine(Spawn(Random.Range(20f, 30f)));
    }
}
