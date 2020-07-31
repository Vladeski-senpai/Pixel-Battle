using UnityEngine;

public class NewDeathParticles : MonoBehaviour
{
    public ParticleSystem[] ps;

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    // Устанавливаем цвета частицам
    public void SetColor(float[] colors)
    {
        for (int i = 0; i < ps.Length; i++)
        {
            int r = i * 2 + i;
            var main = ps[i].main;

            main.startColor = new Color(colors[r], colors[r + 1], colors[r + 2], 255);
        }    
    }
}
