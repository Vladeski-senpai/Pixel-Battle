using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager inventory_manager;
    public Text txt_lvl_num;
    public Sprite[] sprites;
    public GameObject
        arena_obj,
        classic_obj;

    public string ChoosedUnit { get; private set; }

    private ButtonClickAnimation btn_animation;
    private AudioSource audio_s;
    private GameObject outline;
    
    private bool 
        isOn, // Включён ли звук
        isAnimated;

    private void Awake()
    {
        inventory_manager = this;
        audio_s = GetComponent<AudioSource>();

        if (GlobalData.GetInt("Sound") != 0) isOn = true;
    }

    private void Start()
    {
        // Если режим Арена
        if (GlobalData.GetInt("GameMode") != 0)
        {
            classic_obj.SetActive(false);
            arena_obj.SetActive(true);
        }

        ChangeLvlText(GlobalData.GetInt("CurrentLevel"));
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

            // Звук нажатия
            if (isOn)
            {
                audio_s.pitch = 1.19f;
                audio_s.Play();
            }
        }
        // Если выбранный юнит
        else
        {
            // Звук нажатия
            if (isOn)
            {
                audio_s.pitch = 1;
                audio_s.Play();
            }

            ChoosedUnit = null;
        }
    }

    // Обнуляем выбранного юнита
    public void ResetChoosedUnit()
    {
        // Выключаем новую обводку
        if (outline != null)
            outline.SetActive(false);
        
        ChoosedUnit = null;
    }
    
    public void ChangeLvlText(int current_level)
    {
        // Меняем размер шрифта в зависимости от длины чисел уровня
        if (System.Math.Ceiling(System.Math.Log10(current_level) + 1) <= 2)
            txt_lvl_num.fontSize = 152;
        else
            txt_lvl_num.fontSize = 108;

        txt_lvl_num.text = current_level.ToString();
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
