using UnityEngine;

public class UnitEvasionEffect : MonoBehaviour
{
    public sbyte isAlly = 1;
    public bool isNew;

    private UnitEvasionEffect current_obj;
    private SpriteRenderer sprite;
    private float
        posX,
        speed = 1f;

    private byte transparency = 190;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        if (transform.parent != null && transform.parent.parent.CompareTag("Enemy")) isAlly = -1;
    }

    private void Update()
    {
        if (!isNew)
        {
            posX = Mathf.MoveTowards(transform.position.x, transform.position.x - isAlly, speed * Time.deltaTime);
            transform.position = new Vector2(posX, transform.position.y);

            if (transparency > 4)
            {
                sprite.color = new Color32(255, 255, 255, transparency);
                transparency -= 4;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    // Создаём спрайт
    public void SpawnSprite()
    {
        current_obj = Instantiate(this, transform.position, transform.rotation) as UnitEvasionEffect;
        if (transform.localScale.x > 0)
            current_obj.transform.localScale = new Vector2(transform.parent.parent.localScale.x, transform.parent.parent.localScale.y);
        else
            current_obj.transform.localScale = new Vector2(transform.parent.parent.localScale.x * -1, transform.parent.parent.localScale.y);
        current_obj.isNew = false;
    }
}
