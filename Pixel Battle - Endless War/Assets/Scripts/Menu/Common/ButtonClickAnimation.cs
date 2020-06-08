using UnityEngine;
using UnityEngine.UI;

public class ButtonClickAnimation : MonoBehaviour
{
    [HideInInspector]
    public float
        size = 0.22f,
        anim_speed = 0.04f; // Скорость анимации

    [HideInInspector]
    public bool isAnimated; // Анимируется ли кнопка

    private float
        startX_size, // Начальный размер кнопки по х
        startY_size, // Начальный размер кнопки по y
        currentX, // Текущий размер кнопки по х
        currentY; // Текущий размер кнопки по у

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);

        startX_size = transform.localScale.x;
        startY_size = transform.localScale.y;
        currentX = startX_size;
        currentY = startY_size;
    }

    private void Update()
    {
        if (isAnimated)
        {
            currentX += anim_speed;
            currentY += anim_speed;

            if (currentX > startX_size + size)
            {
                currentX = startX_size + size;
                currentY = startY_size + size;
                isAnimated = false;
            }

            transform.localScale = new Vector2(currentX, currentY);
        }
        else
        {
            currentX -= anim_speed;
            currentY -= anim_speed;

            if (currentX < startX_size)
            {
                currentX = startX_size;
                currentY = startY_size;
            }

            transform.localScale = new Vector2(currentX, currentY);
        }
    }

    private void TaskOnClick()
    {
        isAnimated = true;
    }

    public void ResetAnimation()
    {
        currentX = startX_size;
        currentY = startY_size;
        isAnimated = false;
    }
}
