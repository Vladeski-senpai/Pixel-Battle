using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextTyper : MonoBehaviour
{
    private Text txt;
    private string story;
    private float delay = 0.2f;

    private void Start()
    {
        txt = GetComponent<Text>();
        ChangeText(delay);
    }

    //Update text and start typewriter effect
    public void ChangeText(float _delay = 0f)
    {
        StopCoroutine(PlayText()); //stop Coroutime if exist
        //txt.text = " "; //clean text
        Invoke("Start_PlayText", _delay); //Invoke effect
    }

    void Start_PlayText()
    {
        story = txt.text;
        txt.text = "";
        txt.enabled = true;
        StartCoroutine(PlayText());
    }

    private IEnumerator PlayText()
    {
        foreach (char c in story)
        {
            txt.text += c;
            yield return new WaitForSeconds(0.025f);
        }
    }
}
