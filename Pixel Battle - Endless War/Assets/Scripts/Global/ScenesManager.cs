using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager scenes_manager;
    public Text loading_text;
    public Slider slider;  // Слайдер загрузки

    private Animator anim;  // Аниматор чёрного фона
    private int scene_index;

    private void Awake()
    {
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

    // Запускаем анимацию экрана загрузки и записываем номер переключаемой сцены
    public void LoadLevel(int sceneIndex)
    {
        scene_index = sceneIndex;
        anim.SetTrigger("fade");
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
