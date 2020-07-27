using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private Animator animator;
    private int rand;

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Трясём камеру в при нанесении/получении урона по базе
    /// </summary>
    public void Shake()
    {
        rand = Random.Range(1, 4);
        animator.SetTrigger("shake " + rand);
    }

    /// <summary>
    /// Трясём камеру при смерти юнита
    /// </summary>
    public void SmallShake()
    {
        rand = Random.Range(1, 5);
        animator.SetTrigger("d_shake " + rand);
    }
}
