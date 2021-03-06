﻿using UnityEngine;
using UnityEngine.UI;

public class GuildPurchaseButton : MonoBehaviour
{
    public Animator fade_obj;

    private GuildManager guild_manager;
    private AudioSource audio_s;
    private Button button;

    private int
        gold,
        gems;

    private bool isOn; // Включён ли звук

    private void Start()
    {
        guild_manager = GuildManager.guild_manager;
        audio_s = GetComponent<AudioSource>();
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        if (GlobalData.GetInt("Sound") != 0) isOn = true;

        UpdateButton();
    }

    private void TaskOnClick()
    {
        // Звук нажатия
        if (isOn) audio_s.Play();

        fade_obj.SetTrigger("activate");
        guild_manager.BuyOrUpgradeUnit();
        UpdateButton();
    }

    // Обновляем кнопку
    private void UpdateButton()
    {
        gold = GlobalData.GetInt("Gold");
        gems = GlobalData.GetInt("Gems");

        // Если юнит куплен
        if (guild_manager.unit_lvl > 0)
        {
            // Если хватает на апгрейд юнита, включаем кнопку
            if (gems >= guild_manager.unit_lvl)
                button.interactable = true;
            else
                button.interactable = false;
        }
        else
        {
            // Если хватает на покупку юнита, включаем кнопку
            if (gold >= guild_manager.GetUnitGoldCost())
                button.interactable = true;
            else
                button.interactable = false;
        }
    }

    private void OnEnable()
    {
        if (guild_manager != null)
            UpdateButton();
    }
}
