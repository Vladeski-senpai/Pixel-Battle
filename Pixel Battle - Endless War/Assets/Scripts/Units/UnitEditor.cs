#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitData))]
public class UnitEditor : Editor
{
    private UnitData unit_data;
    private Color matColor = Color.white;

    GUIStyle boldStyle, foldoutStyle;

    private bool colorsShow; // Показывается ли спойлер цветов

    public void OnEnable()
    {
        unit_data = (UnitData)target;

        // Толстый текст
        boldStyle = new GUIStyle(EditorStyles.label)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 12
        };

        // Толстый текст
        foldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 12
        };
    }


    public override void OnInspectorGUI()
    {
        // Базовые характеристики юнита
        EditorGUILayout.BeginVertical("Box");

        #region Basic Info
        if (GUILayout.Button("Установить класс", GUILayout.Width(150), GUILayout.Height(22))) unit_data.SetClass();
        EditorGUILayout.Space(3);

        

        EditorGUILayout.LabelField("БАЗОВАЯ ИНФОРМАЦИЯ", boldStyle);
        unit_data.unit_class = EditorGUILayout.TextField("Класс юнита:", unit_data.unit_class);
        unit_data.death_particles_size = EditorGUILayout.IntField("Размер частиц смерти:", unit_data.death_particles_size);
        unit_data.isMelee = EditorGUILayout.Toggle("Ближний бой:", unit_data.isMelee);
        unit_data.canBePushed = EditorGUILayout.Toggle("Может ли быть толкнутым:", unit_data.canBePushed);
        unit_data.shakeHits = EditorGUILayout.Toggle("Тряска при ударах:", unit_data.shakeHits);
        unit_data.shakeShoot = EditorGUILayout.Toggle("Тряска при выстрелах:", unit_data.shakeShoot);
        unit_data.canBleed = EditorGUILayout.Toggle("Может ли кровоточить:", unit_data.canBleed);
        unit_data.canBePoisoned = EditorGUILayout.Toggle("Может ли отравиться:", unit_data.canBePoisoned);
        unit_data.canBeInfected = EditorGUILayout.Toggle("Может ли заразиться:", unit_data.canBeInfected);
        unit_data.canAlwaysMove = EditorGUILayout.Toggle("Двигается всегда:", unit_data.canAlwaysMove);
        unit_data.canSwapLanes = EditorGUILayout.Toggle("Меняет ли линии:", unit_data.canSwapLanes);

        EditorGUILayout.Space(5);
        unit_data.shell_prefab = EditorGUILayout.ObjectField("Префаб снаряда:", unit_data.shell_prefab, typeof(GameObject), false) as GameObject;

        EditorGUILayout.Space(12);
        EditorGUILayout.LabelField("ЗВУК УДАРА/ВЫСТРЕЛА", boldStyle);
        unit_data.hit_sfx = EditorGUILayout.ObjectField("Аудио клип:", unit_data.hit_sfx, typeof(AudioClip), false) as AudioClip;
        unit_data.hit_sfx_volume = EditorGUILayout.Slider("Громкость:", unit_data.hit_sfx_volume, 0f, 1f);
        unit_data.hit_sfx_pitch = EditorGUILayout.Slider("Питч:", unit_data.hit_sfx_pitch, 0.1f, 2f);

        EditorGUILayout.Space(17);
        EditorGUILayout.LabelField("ЗВУК СПАВНА ЮНИТА", boldStyle);
        unit_data.spawn_sfx = EditorGUILayout.ObjectField("Аудио клип:", unit_data.spawn_sfx, typeof(AudioClip), false) as AudioClip;
        unit_data.spawn_sfx_volume = EditorGUILayout.Slider("Громкость:", unit_data.spawn_sfx_volume, 0f, 1f);
        unit_data.spawn_sfx_pitch = EditorGUILayout.Slider("Питч:", unit_data.spawn_sfx_pitch, 0.1f, 2f);

        EditorGUILayout.Space(12);
        EditorGUILayout.LabelField("БАЗОВЫЕ ХАРАКТЕРИСТИКИ", boldStyle);
        unit_data.health = EditorGUILayout.FloatField("Здоровье:", unit_data.health);
        unit_data.damage = EditorGUILayout.FloatField("Урон:", unit_data.damage);
        unit_data.move_speed = EditorGUILayout.FloatField("Скорость движения:", unit_data.move_speed);
        unit_data.attack_speed = EditorGUILayout.FloatField("Скорость атаки:", unit_data.attack_speed);
        unit_data.slow_strength = EditorGUILayout.FloatField("Сила замедления:", unit_data.slow_strength);
        unit_data.slow_frames = EditorGUILayout.IntField("Кадры замедления:", unit_data.slow_frames);
        unit_data.punch_strength = EditorGUILayout.FloatField("Сила толчка:", unit_data.punch_strength);
        unit_data.punch_chance = EditorGUILayout.IntField("Шанс толчка:", unit_data.punch_chance);
        #endregion

        #region Death Particles Colors
        EditorGUILayout.Space(12);
        EditorGUILayout.LabelField("ЦВЕТА ЧАСТИЦ СМЕРТИ", boldStyle);

        matColor = EditorGUILayout.ColorField("Выберите цвет", matColor);

        Color white = new Color(1, 1, 1, 1);

        // Автоматически записываем новые цвета
        if (unit_data.dp_color1.a != 1)
        {
            if (matColor != white)
                unit_data.dp_color1 = matColor;
        }
        else if (unit_data.dp_color2.a != 1)
        {
            if (matColor != white && matColor != unit_data.dp_color1 && matColor != unit_data.dp_color3)
                unit_data.dp_color2 = matColor;
        }
        else if (unit_data.dp_color3.a != 1)
        {
            if (matColor != white && matColor != unit_data.dp_color1 && matColor != unit_data.dp_color2)
                unit_data.dp_color3 = matColor;
        }

        GUILayout.Space(15);

        colorsShow = EditorGUILayout.Foldout(colorsShow, "Цвета частиц", foldoutStyle);

        if (colorsShow)
        {
            // Первый цвет частиц смерти
           
            unit_data.dp_color1 = EditorGUILayout.ColorField("Пепвый цвет", unit_data.dp_color1);
            GUILayout.Space(3);

            // Второй цвет частиц смерти
         

            unit_data.dp_color2 = EditorGUILayout.ColorField("Второй цвет", unit_data.dp_color2);
            GUILayout.Space(3);

            // Третий цвет частиц смерти
         

            unit_data.dp_color3 = EditorGUILayout.ColorField("Третий цвет", unit_data.dp_color3);
        }
        #endregion

        EditorGUILayout.EndVertical();

        #region Perks
        EditorGUILayout.Space(20);

        // Кнопка для создания перков
        if (GUILayout.Button("Добавить перк", GUILayout.Height(22))) unit_data.Perks.Add(new Perks());

        // Меню для перков
        if (unit_data.Perks.Count > 0)
        {
            foreach (Perks perk in unit_data.Perks)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();

                // Кнопка удаления перков
                if (GUILayout.Button("X", GUILayout.Width(23), GUILayout.Height(23)))
                {
                    unit_data.Perks.Remove(perk);
                    break;
                }

                EditorGUILayout.LabelField(" ХАРАКТЕРИСТИКА ПЕРКА");
                EditorGUILayout.EndHorizontal();

                perk.PerkName = EditorGUILayout.TextField("Название:", perk.PerkName);
                perk.PerkChance = EditorGUILayout.FloatField("Шанс срабатывания:", perk.PerkChance);
                perk.PerkDuration = EditorGUILayout.FloatField("Длительность:", perk.PerkDuration);
                perk.PerkValue = EditorGUILayout.FloatField("Доп. значение:", perk.PerkValue);

                EditorGUILayout.EndVertical();
            }
        }
        else EditorGUILayout.LabelField("Перки отсутствуют.");
        #endregion

        if (GUI.changed) SetObjectDirty(unit_data);
    }

    public static void SetObjectDirty(ScriptableObject obj)
    {
        EditorUtility.SetDirty(obj);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
}
#endif
