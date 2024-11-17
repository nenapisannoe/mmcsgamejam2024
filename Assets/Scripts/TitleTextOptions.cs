using UnityEngine;
using TMPro;
using System.Collections;
public class TitleTextOptions : MonoBehaviour
{
    [SerializeField] private float frequency;
    [SerializeField] private TextMeshProUGUI text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor()
    {
        WaitForSeconds wait = new WaitForSeconds(frequency);
        while (true)
        {
            text.color = Random.ColorHSV(0, 1, 0.7f, 1, 0.7f, 1);
            yield return wait;
        }
    }
}
