using UnityEngine;

public class ShellHitParticle : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Animator>().SetInteger("value", Random.Range(1, 5));

        if (AudioManager.instance.PlayValueSound()) GetComponent<AudioSource>().Play();
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
