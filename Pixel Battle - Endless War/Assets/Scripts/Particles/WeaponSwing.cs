using UnityEngine;

public class WeaponSwing : MonoBehaviour
{
    public Animator[] swing_effect;

    private int swing_num = 1;

    // Эффект взмаха оружием
    public void PlaySwingXF()
    {
        swing_effect[0].SetTrigger("play");
    }

    // Эффект взмаха для двух оружий
    public void PlayDoubleWingXF()
    {
        if (swing_num == 1)
        {
            swing_num = 2;
            swing_effect[0].SetTrigger("play");
        }
        else
        {
            swing_num = 1;
            swing_effect[1].SetTrigger("play");
        }
    }
}
