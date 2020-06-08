using UnityEngine;
using UnityEngine.UI;

public class MenuGoldAndGemsInfo : MonoBehaviour
{
    public Text
        txt_gold,
        txt_gems;

    private void Start()
    {
        UpdateInfo();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GlobalData.SetInt("Gold", 0);
            GlobalData.SetInt("Gems", 0);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GlobalData.SetInt("Gold", 10000);
            GlobalData.SetInt("Gems", 10);
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
            GlobalData.SetString("Language", "en");
            GlobalData.SetInt("Warrior", 1);
        }
    }

    // Обновляем информацию о золоте и гемах
    public void UpdateInfo()
    {
        txt_gold.text = GlobalData.GetInt("Gold").ToString();
        txt_gems.text = GlobalData.GetInt("Gems").ToString();
    }
}
