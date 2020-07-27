using UnityEngine;

public class UnitTouchEvents : MonoBehaviour
{
    [HideInInspector]
    public string unit_class;

    private UnitManager unit_manager;
    private RaycastHit2D[] hitInfo; // Записываем кого коснулся луч

    private void Awake()
    {
        unit_manager = GetComponent<UnitManager>();
    }

    private void Update()
    {
#if UNITY_ANDROID
        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                hitInfo = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);
                // RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
                if (hitInfo != null)
                {
                    foreach (RaycastHit2D hit in hitInfo)
                    {
                        if (hit.transform == transform)
                        {
                            unit_manager.TouchAbilities();
                            break;
                        }
                    }
                }
            }
        }
#endif

#if UNITY_EDITOR_WIN
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            hitInfo = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
            // RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
            if (hitInfo != null)
            {
                foreach (RaycastHit2D hit in hitInfo)
                {
                    if (hit.transform == transform)
                    {
                        unit_manager.TouchAbilities();
                        break;
                    }
                }
            }
        }
#endif
    }
}
