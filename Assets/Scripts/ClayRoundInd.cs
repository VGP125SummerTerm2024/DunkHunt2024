using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayRoundInd : MonoBehaviour
{
    [SerializeField] GameObject number1;
    [SerializeField] GameObject number2;

    [SerializeField] Sprite[] numbers;

    [SerializeField] ClayRoundManager rm;


    // Start is called before the first frame update
    void Start()
    {
        display();
    }

    public void display()
    {
        int round = rm.getRoundNumber();
        if (round < 10)
        {
            number1.GetComponent<SpriteRenderer>().sprite = numbers[round];
            number2.GetComponent<SpriteRenderer>().sprite = numbers[0];
        }
        else
        {
            number1.GetComponent<SpriteRenderer>().sprite = numbers[round % 10];
            number2.GetComponent<SpriteRenderer>().sprite = numbers[round / 10];
        }
        StartCoroutine(fade());
    }

    public void removeDisplay()
    {
        gameObject.SetActive(false);
    }

    IEnumerator fade()
    {
        yield return new WaitForSeconds(2f);
        removeDisplay();
    }
}

