using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class DuckScript : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    AudioSource audioSource;
    Collider2D collider2D;

    [SerializeField] float baseSpeed = 5f;

    bool isDead = false;
    bool missed = false;

    [SerializeField]float speed;
    float speedMult;

    public bool isDodgey;

    [SerializeField] Vector2 moveDirection;

    public AmmoManager ammoManager;
    public AudioClip quackSound;

    public int duckType;
    public string duckName;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        collider2D = GetComponent<Collider2D>();  // Cache Collider2D
        ammoManager = FindObjectOfType<AmmoManager>();

        // Initialize duck type
        DetermineDuckType();

        // Initialize movement
        InitializeMovement();
    }

    void DetermineDuckType()
    {
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
    }

    void InitializeMovement()
    {
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

        if (Random.Range(0, 2) == 0)
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

        speed = baseSpeed * speedMult;

        moveDirection = new Vector2(horizontalMovement, verticalMovement);
        moveDirection.x *= leftRight;
    }

    void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (collider2D.OverlapPoint(mousePosition) && !isDead && !missed)
            {
                rb.gravityScale = 0;
                rb.gameObject.layer = LayerMask.NameToLayer("DeadDuck");

                // update the hit UI with a hit duck
                DuckHit hit_UI = (DuckHit)FindObjectOfType(typeof(DuckHit));
                hit_UI.RegisterHit();

                // tell round manager ducks transform location and duck hit
                RoundManager rm = FindObjectOfType<RoundManager>();
                rm.onDuckDestroy(gameObject);

                StartCoroutine(duckHit());
            }
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
            Destroy(gameObject);
        }
    }

    // gives the duck a random chance of changing direction or angle, otherwise just bounces off walls
    void RandomChangeAngle(Collision2D collision)
    {
        int rNum = Random.Range(0, 10);

        // swaps the angle of flight
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

        // flips entire movement
        if (rNum == 1)
        {
            Debug.Log("Movement reversed");
            sr.flipX = !sr.flipX;
            moveDirection *= -1;
        }
        // checks bound to reverse apropriate movement
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

        // update score manager
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

        yield return new WaitForSeconds(.5f);
        moveDirection = new Vector2(0, -1);

        IPMScoreManager.Instance.ScoreSpawn(transform.position, duckType);
    }

    // gives the duck time to fly off screen before being destroyed time must be less than subround time in round manager
    IEnumerator deathDelay()
    {
        yield return new WaitForSeconds(2.5f);
        RoundManager rm = FindObjectOfType<RoundManager>();
        rm.onDuckDestroy(gameObject);
        Destroy(gameObject);
    }

    // changes ducks velocity and triggers the flyaway animation, then moves off screen
    public void FlyAway()
    {
        if (isDead) return;

        DuckHit hit_UI = (DuckHit)FindObjectOfType(typeof(DuckHit));
        hit_UI.RegisterMiss();
        rb.gameObject.layer = LayerMask.NameToLayer("DeadDuck");
        missed = true;
        animator.SetTrigger("FlyAway");
        moveDirection = new Vector2(0, 1);
        StartCoroutine(deathDelay());
    }

    private void PlaySoundOnce(AudioClip clip)
    {
        if (audioSource.isPlaying && audioSource.clip == clip)
            return;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void updateSpeed(float mult)
    {
        speedMult = mult;
        speed = baseSpeed * speedMult;
    }
}
