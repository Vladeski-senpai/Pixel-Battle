using UnityEngine;

public class UnitTouchEvents : MonoBehaviour
{
    [HideInInspector]
    public string unit_class;

    private RaycastHit2D hitInfo; // Записываем кого коснулся луч

    private int turrets = 1;

    private void Update()
    {
#if UNITY_ANDROID
        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);
                // RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
                if (hitInfo.transform == transform)
                {
                    if (turrets > 0)
                    {
                        turrets--;
                        GetComponent<UnitManager>().turret.SetActive(false); // Отключаем спрайт турели тинкера
                        AdditionalUnitsSpawner.instance.SpawnUnit("Turret", transform.position.x - 0.217f, transform.position.y + 0.12f);
                    }
                }
            }
        }
#endif

#if UNITY_EDITOR_WIN
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
            // RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
            if (hitInfo)
            {
                if (hitInfo.transform == transform)
                {
                    if (turrets > 0)
                    {
                        turrets--;
                        GetComponent<UnitManager>().turret.SetActive(false); // Отключаем спрайт турели тинкера
                        AdditionalUnitsSpawner.instance.SpawnUnit("Turret", transform.position.x - 0.217f, transform.position.y + 0.12f);
                    }
                }
            }
        }
#endif
    }
}
