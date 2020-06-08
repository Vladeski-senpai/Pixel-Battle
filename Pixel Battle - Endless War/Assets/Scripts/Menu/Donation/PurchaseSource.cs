using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class PurchaseSource : MonoBehaviour
{
    public Animator fade_out;

    public Text
        girl_text, // Слова девки
        note_text; // Примечание

    private MenuGoldAndGemsInfo money_info; // Текст золота и гемов в меню

    private int
        gold,
        gems;

    private void Start()
    {
        money_info = GetComponent<MenuGoldAndGemsInfo>();
        note_text.text = GlobalTranslateSystem.TranslateLongText("AdNote"); // Примечания о рекламе в меню Пожертвований

        if (PlayerPrefs.GetString("Language") == "ru")
        {
            girl_text.text = "Привет!\n\n" + "Если Вам понравилась игра и Вы хотите поддержать разработчиков, пожалуйста выберите нужную сумму левее :)";
        }
        else
        {
            girl_text.text = "Hello!\n\n" + "If you enjoy the game and want to support the developers, please select the appropriate amount on the left :)";
        }
    }

    // Если покупка завершена успешно
    public void OnPurchaseComplete(Product product)
    {
        switch (product.definition.id)
        {
            // Первый товар за 0.99$
            case "first_product":
                gold = 20000;
                gems = 10;
                break;

            // Второй товар за 1.99$
            case "second_product":
                gold = 40000;
                gems = 20;
                break;

            // Третий товар за 2.99$
            case "third_product":
                gold = 60000;
                gems = 30;
                break;
        }

        // Добавляем купленное золото
        GlobalStats.AddInstantGold(gold);

        // Добавляем купленные гемы
        GlobalStats.AddInstantGems(gems);

        // Обновляем текст на странице
        money_info.UpdateInfo();

        // Вызываем белый эффект
        fade_out.SetTrigger("activate");
    }

    public void OnPurchaseFailure(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("Purchase of product " + product.definition.id + " failed because " + reason);
    }
}
