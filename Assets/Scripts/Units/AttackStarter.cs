using UnityEngine;

public class AttackStarter : MonoBehaviour
{
    private UnitManager unit_manager;

    private void Awake()
    {
        unit_manager = transform.parent.GetComponent<UnitManager>();
    }

    // Начинаем атаку (из аниматора)
    private void StartAttack()
    {
        unit_manager.StartAttack();
    }

    // Отключаем анимацию атаки
    private void DisableAttackAnimation()
    {
        unit_manager.animator.SetBool("attacking", false);
    }
}
