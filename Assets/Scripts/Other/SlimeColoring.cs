using UnityEngine;

public class SlimeColoring : MonoBehaviour
{
    private SpriteRenderer image;

    private void Start()
    {
        image = GetComponent<SpriteRenderer>();
        int one = Random.Range(0, 6);

        switch (one)
        {
            case 0:
                image.color = new Color32(255, Rand(), 72, 220);
                break;

            case 1:
                image.color = new Color32(255, 72, Rand(), 220);
                break;

            case 2:
                image.color = new Color32(Rand(), 255, 72, 220);
                break;

            case 3:
                image.color = new Color32(72, 255, Rand(), 220);
                break;

            case 4:
                image.color = new Color32(Rand(), 72, 255, 220);
                break;

            case 5:
                image.color = new Color32(72, Rand(), 255, 220);
                break;
        }
    }

    private byte Rand()
    {
        return System.Convert.ToByte(Random.Range(72, 256));
    }
}
