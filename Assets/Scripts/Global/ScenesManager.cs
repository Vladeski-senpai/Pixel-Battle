using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager scenes_manager;
    public GameObject loading_obj;
    public Slider slider;  // Слайдер загрузки
    public Text loading_text;

    private Animator anim;  // Аниматор чёрного фона
    private int scene_index;

    private void Awake()
    {

        // Если первый запуск игры
        if (GlobalData.GetInt("FirstLaunch") == 0)
        {
            FirstLaunch();
        }

        GlobalTranslateSystem.language = GlobalData.GetString("Language");
        Application.targetFrameRate = 60; // Устанавливаем максимальный фпс
    }

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        scenes_manager = this;
        loading_text.text = GlobalTranslateSystem.TranslateShortText("Loading");
    }

    private void FirstLaunch()
    {
        GlobalData.SetInt("FirstLaunch", 1);
        GlobalData.SetInt("PlayerLvl", 1);
        GlobalData.SetString("Language", "en");
        GlobalData.SetInt("CurrentLevel", 1);
        GlobalData.SetInt("MaxLevel", 1);
        GlobalData.SetInt("Sound", 1);
        GlobalData.SetInt("Music", 1);
        GlobalData.SetInt("Undead", 1);
        GlobalData.SetInt("Spiderling", 1);
        GlobalData.SetInt("Turret", 1);

        GlobalData.SetInt("Warrior", 1);
        GlobalData.SetString("Slot1", "Warrior");
        GlobalStats.SetStats("StatsUnitsUnlocked", 1); // Добавляем в статистику +1 разблокированный юнит
    }

    // Запускаем анимацию экрана загрузки и записываем номер переключаемой сцены
    public void LoadLevel(int sceneIndex)
    {
        scene_index = sceneIndex;
        anim.SetTrigger("Start");
        if (scene_index == 1) loading_obj.SetActive(true);
        //StartCoroutine(LoadAsynchronously());
    }
    // Загружаем следующий уровень
    private IEnumerator LoadAsynchronously()
    {

        AsyncOperation loading = SceneManager.LoadSceneAsync(scene_index);
        
        while (!loading.isDone)
        {
            float progress = loading.progress / 0.9f * 100; // Делим на 0.9 т.к. слайдер доходит только до 90%
            slider.value = progress;
            yield return null;
        }
    }
}
