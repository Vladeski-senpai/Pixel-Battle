using UnityEngine;

public class DeathParticles : MonoBehaviour
{
    void Start()
    {
        if (AudioManager.instance.PlayDeathSound())
        {
            GetComponent<AudioSource>().Play();
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
