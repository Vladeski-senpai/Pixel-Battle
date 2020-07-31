using UnityEngine;

public class ShellBlockParticle : MonoBehaviour
{
    private void Start()
    {
        if (AudioManager.instance.PlayValueSound()) GetComponent<AudioSource>().Play();
    }

    private void Death()
    {
        Destroy(gameObject, 0.5f);
    }
}
