using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    #region Public Fields
    public GameObject UnitType; // Тип юнита (дальник/ближник)

    public string UnitClass { get; private set; }

    [HideInInspector]
    public float
       health,
       damage,
       move_speed,
       attack_speed;
    #endregion

    #region Private Fields
    private Rigidbody2D rb;
    private Vector2 direction = Vector2.right;

    private bool isMelee = true;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (CompareTag("Enemy")) direction = Vector2.left; // Направление движения
        if (UnitType.name != "melee") isMelee = false; // Тип юнита
    }

    private void Start()
    {
        Destroy(gameObject, 20);
    }

    private void FixedUpdate()
    {
        Move();
    }

    // Движение юнита
    private void Move()
    {
        rb.MovePosition((Vector2)transform.position + (direction * move_speed * Time.deltaTime));
    }
}
