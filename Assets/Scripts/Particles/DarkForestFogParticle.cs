using UnityEngine;

public class DarkForestFogParticle : MonoBehaviour
{
    private float speed;

    private void Start()
    {
        speed = Random.Range(0.9f, 1.5f);

        Destroy(gameObject, 30);
    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x + 1 * speed * Time.deltaTime, transform.position.y);
    }
}
