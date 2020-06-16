using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FORDEVELOPING : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Time.timeScale == 1)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (Time.timeScale == 1)
                Time.timeScale = 0.1f;
            else if (Time.timeScale == 0.1f)
                Time.timeScale = 1;
        }
    }
}
