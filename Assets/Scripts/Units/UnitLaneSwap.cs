using System.Collections;
using UnityEngine;

public class UnitLaneSwap : MonoBehaviour
{
    public float MoveSpeed;

    private UnitFightManager fight_manager;
    private UnitManager unit_manager;
    private float newY;
    private int lane_num;

    private void Start()
    {
        unit_manager = GetComponent<UnitManager>();
        fight_manager = GetComponent<UnitFightManager>();
        StartCoroutine(ChangeLane(Random.Range(3f, 5f)));
    }

    private void Update()
    {
        if (!unit_manager.IsDead && !fight_manager.IsStunned)
        {
            // Идём на верхнюю линию
            if (lane_num == 1)
            {
                newY = Mathf.MoveTowards(transform.position.y, 1.6f, MoveSpeed * Time.deltaTime);
                transform.position = new Vector2(transform.position.x, newY);
            }
            else if (lane_num == 2)
            {
                newY = Mathf.MoveTowards(transform.position.y, -0.35f, MoveSpeed * Time.deltaTime);
                transform.position = new Vector2(transform.position.x, newY);
            }
            else if (lane_num == 3)
            {
                newY = Mathf.MoveTowards(transform.position.y, -2.45f, MoveSpeed * Time.deltaTime);
                transform.position = new Vector2(transform.position.x, newY);
            }
        }
    }

    // Меняем линию
    private IEnumerator ChangeLane(float time)
    {
        yield return new WaitForSeconds(time);

        int prev_lane = lane_num;

        do
        {
            lane_num = Random.Range(1, 4);
        }
        while (prev_lane == lane_num);

        StartCoroutine(ChangeLane(Random.Range(3f, 4f)));
    }
}
