using UnityEngine;

public class DefaultSoundParticles : MonoBehaviour
{
    private void Start()
    {
        if (AudioManager.instance.PlayValueSound()) GetComponent<AudioSource>().Play();
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
