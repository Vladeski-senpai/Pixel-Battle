using UnityEngine;
using UnityEngine.UI;

public class InventoryUnitButton : MonoBehaviour
{
    private InventoryManager inventory_manager;
    private GameObject outline;
    private ButtonClickAnimation btn_animation;
    private Image avatar;

    private void Awake()
    {
        outline = transform.GetChild(0).gameObject;
        avatar = transform.GetChild(1).GetComponent<Image>();
        btn_animation = GetComponent<ButtonClickAnimation>();
        btn_animation.size = 0.86f; // Меняем размер увеличенной кнопки
        btn_animation.anim_speed = 0.14f; // Меняем скорость анимации

        if (name.Substring(3) == "Empty")
        {
            GetComponent<Button>().interactable = false;
            avatar.gameObject.SetActive(false);
            enabled = false;
        }

        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    private void Start()
    {
        inventory_manager = InventoryManager.inventory_manager; // Кэшируем скрипт
    }

    private void TaskOnClick()
    {
        inventory_manager.SetUnit(name.Substring(3), outline, btn_animation);
    }
}
