using UnityEngine;

public static class GlobalData
{
    // Возвращаем float из сохранения
    public static float GetFloat(string name)
    {
        return PlayerPrefs.GetFloat(name);
    }

    // Записываем float сохранение
    public static void SetFloat(string name, float value)
    {
        PlayerPrefs.SetFloat(name, value);
    }

    // Возвращаем int из сохранения
    public static int GetInt(string name)
    {
        return PlayerPrefs.GetInt(name);
    }

    // Записываем int сохранение
    public static void SetInt(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);
    }

    // Возвращаем string из сохранения
    public static string GetString(string name)
    {
        return PlayerPrefs.GetString(name);
    }

    // Записываем string сохранение
    public static void SetString(string name, string value)
    {
        PlayerPrefs.SetString(name, value);
    }
}
