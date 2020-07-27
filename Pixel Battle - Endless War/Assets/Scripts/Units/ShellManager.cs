using System.Collections;
using UnityEngine;

public class ShellManager : MonoBehaviour
{
    public int Health { get; set; } // Здоровье стрелы
    public bool isAlly { get; set; }

    [HideInInspector]
    public int spike_id;

    private UnitManager
        shell_owner, // Владелец снаряда
        enemy; // Обнаруженный противник

    private string unit_class;
    private float shell_speed, newX, newY;
    private int direction = 1;
    private bool
        canHit = true; // Можем ли нанести урон цели (true - да)

    private void Start()
    {
        // Направление движения снаряда
        if (!isAlly) direction = -1;

        SetShellSpeed();
        Destroy(gameObject, 7);
    }

    private void Update()
    {
        if (spike_id == 0)
        {
            newX = Mathf.MoveTowards(transform.position.x, transform.position.x + direction, shell_speed * Time.deltaTime);
            transform.position = new Vector2(newX, transform.position.y);
        }
        else
        {
            if (spike_id == 1)
            {
                newX = Mathf.MoveTowards(transform.position.x, transform.position.x - 1, shell_speed * Time.deltaTime);
                transform.position = new Vector2(newX, transform.position.y);
            }
            else if (spike_id == 2)
            {
                newY = Mathf.MoveTowards(transform.position.y, transform.position.y + 1, shell_speed * Time.deltaTime);
                transform.position = new Vector2(transform.position.x, newY);
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else
            {
                newX = Mathf.MoveTowards(transform.position.x, transform.position.x + 1, shell_speed * Time.deltaTime);
                transform.position = new Vector2(newX, transform.position.y);
                transform.rotation = Quaternion.Euler(0, 0, -180);
            }
        }
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

            case "Dark Slime":
                shell_speed = 10;
                break;

            default:
                shell_speed = 15;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (enemy == null && !isAlly && col.CompareTag("Shield") && canHit)
        {
            Health = 0;
            // Создаём частицы попадания снаряда
            ParticlesManager.instance.SpawnParticles("Shell Hit",
                col.transform.position.x + Random.Range(-0.25f, 0.2f),
                col.transform.position.y + Random.Range(-0.05f, 0.45f));

            CameraShake.instance.SmallShake(); // Трясём камеру
            CheckDeath();
        }

        // Если союзный снаряд обнаружил вражеского юнита
        if (enemy == null && isAlly && col.CompareTag("Enemy") && canHit)
        {
            enemy = col.GetComponent<UnitManager>(); // Кэшируем противника
            shell_owner.Attack(true, enemy); // Проводим атаку
            Health--; // Отнимаем здоровье у снаряда

            CheckDeath();
            StartCoroutine(HitCD());
        }

        // Если вражеский снаряд обнаружил союзного юнита
        else if (enemy == null && !isAlly && col.CompareTag("Ally") && canHit)
        {
            enemy = col.GetComponent<UnitManager>(); // Кэшируем противника
            shell_owner.Attack(true, enemy); // Проводим атаку
            Health--; // Отнимаем здоровье у снаряда

            CheckDeath();
            StartCoroutine(HitCD());
        }
    }

    private void CheckDeath()
    {
        if (Health <= 0) Destroy(gameObject);
    }

    private IEnumerator HitCD()
    {
        canHit = false;
        yield return new WaitForSeconds(0.01f);
        canHit = true;
    }
}
