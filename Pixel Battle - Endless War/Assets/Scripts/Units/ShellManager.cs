using UnityEngine;

public class ShellManager : MonoBehaviour
{
    public int Health { get; set; } // Здоровье стрелы

    private UnitManager
        shell_owner, // Владелец снаряда
        enemy; // Обнаруженный противник

    private string unit_class;
    private float
        shell_speed,
        newX;

    private int direction = 1;
    private bool isAlly;

    private void Start()
    {
        // Направление движения снаряда
        if (!isAlly) direction = -1;

        SetShellSpeed();
        Destroy(gameObject, 7);
    }

    private void Update()
    {
        newX = Mathf.MoveTowards(transform.position.x, transform.position.x + direction, shell_speed * Time.deltaTime);
        transform.position = new Vector2(newX, transform.position.y);
    }

    // Устанавливаем статы
    public void SetStats(bool isAlly, string unit_class, UnitManager shell_owner)
    {
        this.isAlly = isAlly;
        this.unit_class = unit_class;
        this.shell_owner = shell_owner;
        Health = 1;
    }

    // Устанавливаем скорость снаряда
    private void SetShellSpeed() // СДЕЛАТЬ СКОРОСТЬ В ОБЪЕКТЕ РОДИТЕЛЯ!!!!!!!!!!!!!!!!
    {
        switch (unit_class)
        {
            case "Ninja":
                shell_speed = 11;
                break;

            case "Elf Maiden":
            case "Snowguy Range":
                shell_speed = 13;
                break;

            case "Turret":
                shell_speed = 16;
                break;

            case "Gunman":
                shell_speed = 17;
                break;

            case "Witch":
                shell_speed = 11;
                break;

            case "Mutant":
                shell_speed = 13;
                break;

            default:
                shell_speed = 15;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Если союзный снаряд обнаружил вражеского юнита
        if (isAlly && col.CompareTag("Enemy"))
        {
            enemy = col.GetComponent<UnitManager>(); // Кэшируем противника

            // Проводим атаку
            shell_owner.Attack(true, enemy);

            Health--;
            CheckDeath();
        }

        // Если вражеский снаряд обнаружил союзного юнита
        else if (!isAlly && col.CompareTag("Ally"))
        {
            enemy = col.GetComponent<UnitManager>(); // Кэшируем противника

            // Проводим атаку
            shell_owner.Attack(true, enemy);

            Health--;
            CheckDeath();
        }
    }

    private void CheckDeath()
    {
        if (Health <= 0) Destroy(gameObject);
    }
}
