using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    void Start()
    {
        ChangeSizeAndPosition();
    }

    // Изменяем размер и положения фона
    private void ChangeSizeAndPosition()
    {
        // 800x480
        if (Screen.width <= 800) //&& Screen.height == 480
        {
            transform.localScale = new Vector2(9.45f, 9.45f);
            transform.position = new Vector2(0, 0);
        }

        // 1280x720 or 2560x1440
        else if (Screen.width <= 1280)
        {
            transform.localScale = new Vector2(9.45f, 9.45f);
            transform.position = new Vector2(0, 0);
        }

        // 1280x720 or 2560x1440
        else if (Screen.width <= 1480)
        {
            transform.localScale = new Vector2(10.75f, 10.75f);
            transform.position = new Vector2(0, 0.42f);
        }

        // 1560
        else if (Screen.width <= 1560)
        {

            transform.localScale = new Vector2(11.35f, 11.35f);
            transform.position = new Vector2(0, -0.5f);
        }

        // 1920
        else if (Screen.width <= 1920)
        {
            transform.localScale = new Vector2(9.45f, 9.45f);
            transform.position = new Vector2(0, -0.03f);
        }

        // 2160x1080
        else if (Screen.width <= 2160 || Screen.width <= 2280) //&& Screen.height == 1080
        {
            transform.localScale = new Vector2(11.1f, 11.1f);
            transform.position = new Vector2(0, -0.48f);
        }

        else if (Screen.width <= 2560)
        {
            transform.localScale = new Vector2(9.5f, 9.5f);
            transform.position = new Vector2(0, 0.02f);
        }

        // 2960x1440
        else if (Screen.width <= 2960) //&& Screen.height == 1440
        {
            transform.localScale = new Vector2(10.8f, 10.8f);
            transform.position = new Vector2(0, -0.08f); // Для меню
        }
    }
}
