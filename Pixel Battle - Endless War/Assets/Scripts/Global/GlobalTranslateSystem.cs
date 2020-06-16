public static class GlobalTranslateSystem
{
    public static string language;
    
    // Переводим только короткие текста
    public static string TranslateShortText(string code)
    {
        switch (language)
        {
            // Если язык Русский
            case "ru":
                switch (code)
                {
                    case "Tap to play":
                        return "Нажмите, чтобы играть";

                    case "Menu":
                        return "Меню";

                    case "Guild":
                        return "Гильдия";

                    case "Settings":
                        return "Настройки";

                    case "Stats":
                        return "Статистика";

                    case "Restart":
                        return "Повторить";

                    case "Back":
                        return "Назад";

                    case "Play":
                        return "Играть";

                    case "Loading":
                        return "Загрузка";

                    case "Pause":
                        return "Пауза";

                    case "Resume":
                        return "Возобновить";
                }
                break;

            // Если язык Английский
            case "en":
                return code; // Возвращаем само название
        }

        return null;
    }

    // Переводим только тексты статистики
    public static string TranslateStatsText(string code)
    {
        switch (language)
        {
            // Если язык Русский
            case "ru":
                switch (code)
                {
                    case "Enemies defeated":
                        return "Противников убито";

                    case "Units summoned":
                        return "Призвано юнитов";

                    case "Victories":
                        return "Побед";

                    case "Gold earned":
                        return "Золота получено";

                    case "Gems collected":
                    case "Gems received":
                        return "Гемов получено";

                    case "Units unlocked":
                        return "Юнитов открыто";

                    case "Defeats":
                        return "Поражений";

                    case "Arena Record":
                        return "Рекорд Арены";

                    case "Donation":
                        return "Пожертвования";
                }
                break;

            // Если язык Английский
            case "en":
                return code; // Возвращаем само название
        }

        return null;
    }

    // Переводим длинные тексты
    public static string TranslateLongText(string code)
    {
        switch (language)
        {
            case "ru":
                switch (code)
                {
                    case "RateApp":
                        return "Поделитесь пожалуйста Вашим мнением об игре на Play Market. Спасибо!";

                    case "Warrior1":
                        return "Жители сказали что гоблины где-то здесь...";

                    case "Warrior2":
                        return "Бегите отсюда!\nЯ с ними разберусь!";

                    case "Warrior3":
                        return "Мне срочно нужно подкрепление!";

                    case "Tutorial1":
                        return "Воину нужна Ваша помощь! Нажмите эту кнопку, чтобы выбрать юнита.";

                    case "Tutorial2":
                        return "Теперь выберите все три линии.";

                    case "Tutorial3":
                        return "Отлично! Поддержка была выслана.\n" +
                            "Вы можете призвать юнитов за счёт маны. Чтобы увеличить её скорость восстановления, " +
                            "нажмите эту кнопку, когда у Вас будет достаточно маны для этого.";
                }
                break;

            case "en":
                switch (code)
                {
                    case "RateApp":
                        return "Please share your opinion about the game on the Play Market. Thank you!";

                    case "Warrior1":
                        return "Villagers said they saw few goblins around here...";

                    case "Warrior2":
                        return "Run away from here!\nI’ll deal with them!";

                    case "Warrior3":
                        return "I need a backup, now!";

                    case "Tutorial1":
                        return "The Warrior needs your help! Click this button to select a unit.";

                    case "Tutorial2":
                        return "Select all three lanes to summon units.";

                    case "Tutorial3":
                        return "Great! Reinforcement has been sent.\n"+
                            "You can summon units at the expense of mana. To increase its recovery speed, " +
                            "click this button when you have enough mana for this.";
                }
                break;
        }

        return null;
    }
}
