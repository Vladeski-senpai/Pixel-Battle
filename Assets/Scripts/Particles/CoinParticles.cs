using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinParticles : MonoBehaviour
{
    public Text coins_text; // Текст с золотом

    [HideInInspector]
    public int CoinsReward { get; set; }  // Награда золотом

    // Start is called before the first frame update
    private void Start()
    {
        coins_text.text = "+" + CoinsReward;
        Destroy(gameObject, 0.8f);
    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + 1 * 1.2f * Time.deltaTime);
    }

    private IEnumerator Move()
    {
        Vector2 Gotoposition = new Vector2(transform.position.x, transform.position.y + 2);
        float elapsedTime = 0;
        float waitTime = 0.9f;
        Vector2 currentPos = transform.position;

        while (elapsedTime < waitTime)
        {
            transform.position = Vector2.Lerp(currentPos, Gotoposition, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        // Make sure we got there
        Destroy(gameObject);
        yield return null;
    }
}
