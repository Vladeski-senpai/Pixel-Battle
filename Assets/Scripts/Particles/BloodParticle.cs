using UnityEngine;

public class BloodParticle : MonoBehaviour
{
    private void Start()
    {
        float rand_scale = Random.Range(4.8f, 6.1f);
        transform.localScale = new Vector3(rand_scale, rand_scale, 1);

        GetComponent<Animator>().SetInteger("value", Random.Range(1, 3));

        // Min - 180, 0, 0, Max - 255, 100, 100. G и B должны быть одинакового значения, иначе цвет будет другой
        int a = Random.Range(200, 240);
        int b = Random.Range(0, 75);

        // Меняем на случайный красный цвет
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(System.Convert.ToByte(a), System.Convert.ToByte(b), System.Convert.ToByte(b), 255);
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
