using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    public static ParticlesManager instance;

    public GameObject
        death_particles,
        big_death_particles,
        coin_particle,
        shell_hit,
        shell_block_particle,
        blood_particle,
        poison_particle,
        execution_particle,
        critical_hit_particle,
        smoke_explosion; // Старый эффект смерти юнитов

    [Space]
    public Transform particles_trashcan;

    private GameObject temp; // Сюда записываем создаваемую частицу

    private string prev_code; // Сюда записываем имя предыдущей вызываемой частицы (для оптимизации)

    private void Awake()
    {
        instance = this;
    }
    
    // Создаём частицы смерти
    public void DeathParticles(int size, float[] colors, float posX, float posY)
    {
        GameObject dp;

        if (size == 0)
            dp = Instantiate(death_particles, new Vector2(posX, posY), Quaternion.identity);
        else
            dp = Instantiate(big_death_particles, new Vector2(posX, posY), Quaternion.identity);

        dp.GetComponent<NewDeathParticles>().SetColor(colors);
    }

    // Создаём большие частицы смерти
    public void BigDeathParticles(float[] colors, float posX, float posY)
    {

    }

    // Создаём частицы
    public void SpawnParticles(string code, float posX, float posY)
    {
        // Если создаём новую частицу
        if (prev_code != code)
        {
            prev_code = code;

            switch (code)
            {
                // Частицы попадания снаряда
                case "Shell Hit": temp = shell_hit; break;

                // Частицы заблокированного снаряда
                case "Shell Block": temp = shell_block_particle; break;

                // Частицы крови
                case "Blood": temp = blood_particle; break;

                // Частицы яда
                case "Poison": temp = poison_particle; break;

                // Частицы казни (брызги крови)
                case "Execution": temp = execution_particle; break;

                // Частица критического урона (красный череп)
                case "Critical": temp = critical_hit_particle; break;

                // Старый частицы смерти юнита
                case "Death": temp = smoke_explosion; break;
            }
        }

        Instantiate(temp, new Vector2(posX, posY), Quaternion.identity, particles_trashcan);
    }

    // Создаём монетку после убийства юнита
    public void SpawnCoin(int amount, float posX, float posY)
    {
        GameObject coin = Instantiate(coin_particle, new Vector2(posX, posY), Quaternion.identity, particles_trashcan);
        coin.GetComponent<CoinParticles>().CoinsReward = amount;
    }
}
