using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckHitGround : MonoBehaviour
{
    [SerializeField] private DogAI dog;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DuckGroundTrigger"))
        {

        }
    }
}
