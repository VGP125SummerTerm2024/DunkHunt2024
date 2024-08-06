using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D), typeof(SpriteRenderer))]

public class DuckScript : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    AudioSource audioSource;

    [SerializeField] float baseSpeed = 4000f;

    bool isDead = false;

    public float speed;

    public bool isDodgey;

    [SerializeField] Vector2 moveDirection;

    public AmmoManager ammoManager;
    public AudioClip quackSound;

    public int duckType;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        ammoManager = FindObjectOfType<AmmoManager>();
        audioSource = GetComponent<AudioSource>();

        if (sr.sprite.name == "Duck_Green_FlyDiagonal1")
        {
            duckType = 1;
        }
        else if (sr.sprite.name == "Duck_Blue_FlyDiagonal1")
        {
            duckType = 2;
        }
        else if (sr.sprite.name == "Duck_Red_FlyDiagonal1")
        {
            duckType = 3;
        }

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

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GetComponent<Collider2D>().OverlapPoint(mousePosition) && !isDead)
            {
                rb.gravityScale = 0;
                rb.gameObject.layer = LayerMask.NameToLayer("DeadDuck");
                StartCoroutine(duckHit());
            }
            ammoManager.UpdateAmmo();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bound") && !isDead)
        {
            PlaySoundOnce(quackSound);
            RandomChangeAngle(collision);
        }
        else if (collision.gameObject.CompareTag("DuckGroundTrigger"))
        {
            // tell round manager ducks transform location and duck hit
            Destroy(gameObject);
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

    IEnumerator duckHit()
    {
        // Add duck hit to round manager
        animator.SetTrigger("DuckHit");
        isDead = true;
        moveDirection = new Vector2(0, 0);
        yield return new WaitForSeconds(.5f);
        moveDirection = new Vector2(0, -1);

        if (duckType == 1)
        {
            IPMScoreManager.Instance._BlackDuck();
        }
        else if (duckType == 2)
        {
            IPMScoreManager.Instance._BlueDuck();
        }
        else if (duckType == 3)
        {
            IPMScoreManager.Instance._RedDuck();
        }
    }

    private void PlaySoundOnce(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
