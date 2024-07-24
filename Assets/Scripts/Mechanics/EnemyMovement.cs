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
        animator = GetComponent<Animator>();

        int leftRight;
        float verticalMovement;
        float horizontalMovement;

        if (Random.Range(0, 2) == 0)
            leftRight = -1;
        else
        {
            sr.flipX = true;
            leftRight = 1;
        }

        if (Random.Range(0,2) == 0)
        {
            animator.SetBool("Flip", true);
            verticalMovement = .4f;
            horizontalMovement = .6f;
        }
        else
        {
            verticalMovement = .6f;
            horizontalMovement = .4f;
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
            animator.SetBool("Flip", !animator.GetBool("Flip"));
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
            sr.flipX = !sr.flipX;
            moveDirection *= -1;
        }
        else
        {
            if (collision.gameObject.name == "LeftBound" || collision.gameObject.name == "RightBound")
            {
                sr.flipX = !sr.flipX;
                moveDirection.x *= -1;
            }
            else if (collision.gameObject.name == "TopBound" || collision.gameObject.name == "BottomBound")
            {
                moveDirection.y *= -1;
            }
        }
    }
}
