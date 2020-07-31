using UnityEngine;

public class PoisonParticle : MonoBehaviour
{
    public Sprite[] sprites;

    private void Start()
    {
        int rand = Random.Range(0, 7);

        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[rand];
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
