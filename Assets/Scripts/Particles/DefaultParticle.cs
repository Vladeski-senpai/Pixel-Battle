using UnityEngine;

// Простой скрипт уничтожающий объект частицы после завершения анимации
public class DefaultParticle : MonoBehaviour
{
    private void Death()
    {
        Destroy(gameObject);
    }
}
