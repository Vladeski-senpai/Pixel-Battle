using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(UnitData))]
public class UnitEditor : Editor
{
    private UnitData unit_data;

    public void OnEnable()
    {
        unit_data = (UnitData)target;
    }

    public override void OnInspectorGUI()
    {
        // Базовые характеристики юнита
        EditorGUILayout.BeginVertical("Box");
        // Кнопка для создания перков

        if (GUILayout.Button("Установить класс", GUILayout.Width(150), GUILayout.Height(22))) unit_data.SetClass();
        EditorGUILayout.Space(3);
        EditorGUILayout.LabelField("БАЗОВАЯ ИНФОРМАЦИЯ");
        unit_data.unit_class = EditorGUILayout.TextField("Класс юнита:", unit_data.unit_class);
        unit_data.isMelee = EditorGUILayout.Toggle("Ближний бой:", unit_data.isMelee);
        unit_data.canBleed = EditorGUILayout.Toggle("Может кровоточить:", unit_data.canBleed);
        unit_data.canPoisoned = EditorGUILayout.Toggle("Может отравиться:", unit_data.canPoisoned);
        unit_data.canInfected = EditorGUILayout.Toggle("Может заразиться:", unit_data.canInfected);

        EditorGUILayout.Space(5);
        unit_data.shell_prefab = EditorGUILayout.ObjectField("Префаб снаряда:", unit_data.shell_prefab, typeof(GameObject), false) as GameObject;

        EditorGUILayout.Space(12);
        EditorGUILayout.LabelField("ЗВУК УДАРА/ВЫСТРЕЛА");
        unit_data.hit_sfx = EditorGUILayout.ObjectField("Аудио клип:", unit_data.hit_sfx, typeof(AudioClip), false) as AudioClip;
        unit_data.hit_sfx_volume = EditorGUILayout.Slider("Громкость:", unit_data.hit_sfx_volume, 0f, 1f);
        unit_data.hit_sfx_pitch = EditorGUILayout.Slider("Питч:", unit_data.hit_sfx_pitch, 0.1f, 2f);

        EditorGUILayout.Space(17);
        EditorGUILayout.LabelField("ЗВУК СПАВНА ЮНИТА");
        unit_data.spawn_sfx = EditorGUILayout.ObjectField("Аудио клип:", unit_data.spawn_sfx, typeof(AudioClip), false) as AudioClip;
        unit_data.spawn_sfx_volume = EditorGUILayout.Slider("Громкость:", unit_data.spawn_sfx_volume, 0f, 1f);
        unit_data.spawn_sfx_pitch = EditorGUILayout.Slider("Питч:", unit_data.spawn_sfx_pitch, 0.1f, 2f);

        EditorGUILayout.Space(12);
        EditorGUILayout.LabelField("БАЗОВЫЕ ХАРАКТЕРИСТИКИ");
        unit_data.health = EditorGUILayout.FloatField("Здоровье:", unit_data.health);
        unit_data.damage = EditorGUILayout.FloatField("Урон:", unit_data.damage);
        unit_data.move_speed = EditorGUILayout.FloatField("Скорость движения:", unit_data.move_speed);
        unit_data.attack_speed = EditorGUILayout.FloatField("Скорость атаки:", unit_data.attack_speed);

        EditorGUILayout.EndVertical();

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

        if (GUI.changed) SetObjectDirty(unit_data);
    }

    public static void SetObjectDirty(ScriptableObject obj)
    {
        EditorUtility.SetDirty(obj);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }
}
