using UnityEngine;
using UnityEngine.UI;

public class InventoryArenaRecordInfo : MonoBehaviour
{
    private void Start()
    {
        if (GlobalData.GetString("Language") == "ru")
        {
            transform.GetChild(0).GetComponent<Text>().text = "Рекорд " + GlobalData.GetInt("MaxWave");
        }
        else
        {
            transform.GetChild(0).GetComponent<Text>().text = "Record " + GlobalData.GetInt("MaxWave");
        }
    }
}
