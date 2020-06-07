using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager inventory_manager;
    public Sprite[] sprites;

    public string ChoosedUnit { get; private set; }

    private ButtonClickAnimation btn_animation;
    private GameObject outline;
    
    private bool isAnimated;

    private void Awake()
    {
        inventory_manager = this;
    }

    // Записываем юнита
    public void SetUnit(string choosed_unit, GameObject outline, ButtonClickAnimation btn_animation)
    {
        // Выключаем старую обводку, если она есть
        if (this.outline != null)
            this.outline.SetActive(false);

        // Если новый юнит
        if (choosed_unit != ChoosedUnit)
        {
            ChoosedUnit = choosed_unit;
            this.outline = outline;
            this.outline.SetActive(true); // Включаем новую обводку
            this.btn_animation = btn_animation;
        }
        // Если выбранный юнит
        else ChoosedUnit = null;
    }

    // Обнуляем выбранного юнита
    public void ResetChoosedUnit()
    {
        // Выключаем новую обводку
        if (outline != null)
            outline.SetActive(false);
        ChoosedUnit = null;
    }
    
    // Запускаем анимацию занятого слота
    public void PlayAnim()
    {
        if (!isAnimated)
        {
            isAnimated = true;
            btn_animation.isAnimated = true;
            StartCoroutine(ChangeColor());
        }
    }

    private IEnumerator ChangeColor()
    {
        outline.GetComponent<Image>().color = new Color32(255, 105, 105, 255);
        yield return new WaitForSeconds(0.5f);
        outline.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        isAnimated = false;
    }
}
