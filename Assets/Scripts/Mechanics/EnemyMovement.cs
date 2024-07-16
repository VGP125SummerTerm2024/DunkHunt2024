using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D), typeof(SpriteRenderer))]

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;

    [SerializeField] float baseSpeed = 4000f;

    public float speed;

    public bool isDodgey;

    [SerializeField] Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        //animator = GetComponent<Animator>();

        int leftRight;
        float verticalMovement;
        float horizontalMovement;

        if (Random.Range(0, 2) == 0)
        {
            sr.flipX = true;
            leftRight = -1;
        }
        else
            leftRight = 1;

        if (Random.Range(0,2) == 0)
        {
            verticalMovement = .3f;
            horizontalMovement = .7f;
        }
        else
        {
            verticalMovement = .7f;
            horizontalMovement = .3f;
        }

        //Later multiply by round multiplier
        speed = baseSpeed;

        moveDirection = new Vector2(horizontalMovement, verticalMovement);
        moveDirection.x *= leftRight;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = moveDirection * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bound"))
        {
            RandomChangeAngle(collision);
        }
    }

    void RandomChangeAngle(Collision2D collision)
    {
        int rNum = Random.Range(0, 10);

        if (rNum == 0)
        {
            Debug.Log("Angle Reversed");
            float temp = moveDirection.x;
            if (moveDirection.x < 0 && moveDirection.y < 0 || moveDirection.x > 0 && moveDirection.y > 0)
            {
                moveDirection.x = moveDirection.y;
                moveDirection.y = temp;
            }
            else
            {
                moveDirection.x = moveDirection.y * -1;
                moveDirection.y = temp * -1;
            }
        }
        
        if (rNum == 1)
        {
            Debug.Log("Movement reversed");
            moveDirection *= -1;
        }
        else
        {
            if (collision.gameObject.name == "LeftBound" || collision.gameObject.name == "RightBound")
            {
                sr.flipX = !sr.flipX;
                moveDirection.x *= -1;
            }
            else if (collision.gameObject.name == "TopBound")
            {
                //set animation to flying down
                Debug.Log("Duck flys down");
                moveDirection.y *= -1;
            }
            else if (collision.gameObject.name == "BottomBound")
            {
                //set animation to fly up
                Debug.Log("Duck flys up");
                moveDirection.y *= -1;
            }
        }
    }
}
