using UnityEngine;

public class GuildScrollMenu : MonoBehaviour
{
    [HideInInspector]
    public float state;

    private float
        startY,
        currentY,
        targetY;

    private void Start()
    {
        startY = GetComponent<RectTransform>().transform.position.y;
        currentY = startY;
    }

    private void Update()
    {
        targetY = Mathf.Lerp(transform.position.y, currentY, 5 * Time.deltaTime);
        transform.position = new Vector2(transform.position.x, targetY);
    }

    public void Move(int i)
    {
        state += i;

        if (state < 0)
            state = 0;
        else if (state > 2)
            state = 2;

        if (state == 0)
            currentY = startY;
        else if (state == 1)
            currentY = startY * 2.3f;
        else if (state == 2)
            currentY = startY * 3.65f;
        else if (state == 3)
            currentY = startY * 5.8f;
    }
}
