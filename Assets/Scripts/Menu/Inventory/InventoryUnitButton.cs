using UnityEngine;
using UnityEngine.UI;

public class InventoryUnitButton : MonoBehaviour
{
    private InventoryManager inventory_manager;
    private ButtonClickAnimation btn_animation;
    private GameObject outline;
    private Button button;
    private Image avatar;

    private void Awake()
    {
        outline = transform.GetChild(0).gameObject;
        avatar = transform.GetChild(1).GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
        btn_animation = GetComponent<ButtonClickAnimation>();
        btn_animation.size = 0.86f; // Меняем размер увеличенной кнопки
        btn_animation.anim_speed = 0.14f; // Меняем скорость анимации
        CheckButtonCondition();

        if (name.Substring(3) == "Empty")
        {
            GetComponent<Button>().interactable = false;
            avatar.gameObject.SetActive(false);
            enabled = false;
        }
    }

    private void Start()
    {
        inventory_manager = InventoryManager.inventory_manager; // Кэшируем скрипт
    }

    private void TaskOnClick()
    {
        inventory_manager.SetUnit(name.Substring(3), outline, btn_animation);
    }

    private void CheckButtonCondition()
    {
        // Если юнит заблокирован
        if (GlobalData.GetInt(name.Substring(3)) == 0)
        {
            button.interactable = false;
            avatar.color = new Color32(120, 120, 120, 255);
        }
    }
}
