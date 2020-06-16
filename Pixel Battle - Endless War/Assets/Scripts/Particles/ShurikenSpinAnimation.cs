using UnityEngine;

public class ShurikenSpinAnimation : MonoBehaviour
{
    public int shuriken_id;

    private float angle, newY;

    private void Awake()
    {
        if (AudioManager.instance.IsOn()) GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
        angle = Mathf.Lerp(angle, angle - 1, 10 * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, angle * 150);

        if (shuriken_id == 1)
        {
            newY = Mathf.MoveTowards(transform.position.y, transform.position.y + 0.04f, 7 * Time.deltaTime);
            transform.position = new Vector2(transform.position.x, newY);
        }
        else if (shuriken_id == 2)
        {
            newY = Mathf.MoveTowards(transform.position.y, transform.position.y - 0.04f, 7 * Time.deltaTime);
            transform.position = new Vector2(transform.position.x, newY);
        }
    }
}
