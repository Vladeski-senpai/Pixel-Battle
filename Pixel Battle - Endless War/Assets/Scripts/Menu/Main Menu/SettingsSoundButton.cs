using UnityEngine;
using UnityEngine.UI;

public class SettingsSoundButton : MonoBehaviour
{
    public Sprite[] sprites;

    private Image image;
    private Text txt;

    private int value;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
        image = GetComponent<Image>();
        txt = transform.GetChild(0).GetComponent<Text>();

        CheckButtonCondition();
    }

    private void TaskOnClick()
    {
        switch (name.Substring(3))
        {
            case "Music":
                // Если музыка выключена, включаем её
                if (value == 0)
                    GlobalData.SetInt("Music", 1);
                else
                    GlobalData.SetInt("Music", 0);
                break;

            case "Sound":
                // Если звуки выключены, включаем их
                if (value == 0)
                    GlobalData.SetInt("Sound", 1);
                else
                    GlobalData.SetInt("Sound", 0);
                break;
        }

        if (value == 0)
            GlobalData.SetInt(name.Substring(3), 1);
        else
            GlobalData.SetInt(name.Substring(3), 0);

        CheckButtonCondition();
    }

    public void CheckButtonCondition()
    {
        int id = 0; // Если 0, картинка "выключена"

        switch (name.Substring(3))
        {
            case "Music":
                value = GlobalData.GetInt("Music");
                // Если музыка включена
                if (value != 0)
                {
                    txt.text = "Music ON";
                    id = 1;
                }
                else txt.text = "Music OFF";
                break;

            case "Sound":
                value = GlobalData.GetInt("Sound");
                // Если звуки включены
                if (value != 0)
                {
                    txt.text = "Sound ON";
                    id = 1;
                }
                else txt.text = "Sound OFF";
                break;
        }

        image.sprite = sprites[id];
    }
}
