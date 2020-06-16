using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryLvlButton : MonoBehaviour
{
    public Text txt_lvl_num;

    private InventoryManager inventory_manager;
    private AudioSource audio_s;

    private int
        current_level,
        max_level;

    private bool isOn; // Включён ли звук

    private void Start()
    {
        audio_s = GetComponent<AudioSource>();
        GetComponent<Button>().onClick.AddListener(TaskOnClick);

        if (GlobalData.GetInt("Sound") != 0) isOn = true;

        inventory_manager = InventoryManager.inventory_manager;
        max_level = GlobalData.GetInt("MaxLevel");
    }

    private void TaskOnClick()
    {
        // Звук нажатия
        if (isOn) audio_s.Play();

        current_level = GlobalData.GetInt("CurrentLevel");

        if (name.Substring(3) == "Next")
        {
            if (current_level < max_level)
            {
                current_level++;
                GlobalData.SetInt("CurrentLevel", current_level);
            }
        }
        else
        {
            if (current_level > 1)
            {
                current_level--;
                GlobalData.SetInt("CurrentLevel", current_level);
            }
        }

        inventory_manager.ChangeLvlText(current_level);
    }
}
