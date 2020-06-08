using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GuildExitButton : MonoBehaviour
{
    private GuildManager guild_manager;
    private ButtonClickAnimation btn_animation;

    private void Awake()
    {  
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
        btn_animation = GetComponent<ButtonClickAnimation>();
    }

    private void Start()
    {
        guild_manager = GuildManager.guild_manager;
    }

    private void TaskOnClick()
    {
        if (gameObject.activeSelf)
            StartCoroutine(CloseMenu());
    }

    private IEnumerator CloseMenu()
    {
        yield return new WaitForSeconds(0.2f);
        guild_manager.CloseMenu();
    }

    // Сбрасываем анимацию при отключении объекта
    private void OnEnable()
    {
        btn_animation.ResetAnimation();
    }
}
