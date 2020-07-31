using UnityEngine;

public class DesertGrassMovement : MonoBehaviour
{
    private float newX;

    private void Update()
    {
        newX = Mathf.MoveTowards(transform.position.x, transform.position.x + 1, 2 * Time.deltaTime);
        transform.position = new Vector2(newX, transform.position.y);
    }
}
