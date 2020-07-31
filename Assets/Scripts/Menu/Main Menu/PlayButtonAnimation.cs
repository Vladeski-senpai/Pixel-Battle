using UnityEngine;
using UnityEngine.UI;

public class PlayButtonAnimation : MonoBehaviour
{
    private Text txt;
    private byte alpha = 255;
    private bool isFading = true; // Исчезает ли текст (true - да)

    private void Start()
    {
        txt = GetComponent<Text>();
        txt.text = GlobalTranslateSystem.TranslateShortText("Tap to play");
    }

    private void Update()
    {
        // Если повышаем прозрачность
        if (isFading)
        {
            alpha -= 2;
            if (alpha <= 50)
            {
                isFading = false;
                alpha = 50;
            }
        }
        // Если снижаем прозрачность
        else
        {
            alpha += 2;
            if (alpha >= 254)
            {
                isFading = true;
                alpha = 255;
            }
        }

        txt.color = new Color32(255, 255, 255, alpha);
    }
}
