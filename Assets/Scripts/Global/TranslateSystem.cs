using UnityEngine;
using UnityEngine.UI;

public class TranslateSystem : MonoBehaviour
{
    [Header("Translate Short Texts")]
    public Text[] txt_short;

    [Header("Translate Long Texts")]
    [Space]
    public Text[] txt_long;

    private void Start()
    {
        Translate();
    }

    private void Translate()
    {
        for (int i = 0; i < txt_short.Length; i++)
        {
            txt_short[i].text = GlobalTranslateSystem.TranslateShortText(txt_short[i].name.Substring(3));
        }

        for (int i = 0; i < txt_long.Length; i++)
        {
            txt_long[i].text = GlobalTranslateSystem.TranslateLongText(txt_long[i].name.Substring(3));
        }
    }
}
