using UnityEngine;

public class RandomSpriteChanger : MonoBehaviour
{
    public Sprite[] sprites;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
