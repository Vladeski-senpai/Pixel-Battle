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

    // Обновляем информацию о золоте и гемах
    public void UpdateInfo()
    {
        txt_gold.text = GlobalData.GetInt("Gold").ToString();
        txt_gems.text = GlobalData.GetInt("Gems").ToString();
    }
}
