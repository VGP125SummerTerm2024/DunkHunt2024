using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class DogAI : MonoBehaviour
{
    // GameObjects
    public GameObject dog; // For location/movement
    public float speed = 3.0f;
    private Vector2 moveRight = Vector2.right;
    public float jumpHeight = 5.0f;

    // Components
    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb;

    // Animations
    public AnimationClip Laugh;
    public AnimationClip Jump;
    public AnimationClip Sniff;

    // Triggers
    public GameObject jumpTrigger;

    // Layermasks
    public string backgroundSortingLayer = "Background";
    public string foregroundSortingLayer = "Foreground";

    // AudioClips
    public AudioSource audioSource;
    public AudioClip bark;
    public AudioClip laugh;
    public AudioClip hitDuck;

    // Sprites
    public Sprite oneDuckSprite;
    public Sprite twoDuckSprite;
    public Sprite laughSprite;

    private bool isMoving = true;
    private Vector3 originalPosition;

    public int duckHitCount = 0;
    public SpawnManager duckMGR;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1.0f;
        originalPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            DogMove();
        }

        // Debug controls for testing
        if (Input.GetKey(KeyCode.P)) { DogLaugh(); }
        if (Input.GetKey(KeyCode.O)) { DogHoldDuck(); }
        if (Input.GetKey(KeyCode.I)) { DogHoldTwoDuck(); }
    }

    public void DogLaugh()
    {
        // anim.SetTrigger("laugh") change name to trigger name
        duckHitCount = 0;
        sr.sprite = laughSprite;
        PlaySoundOnce(laugh);
        anim.Play("Laugh");
        StartCoroutine(MoveUpPauseAndReturn());
    }

    public void DogHoldDuck()
    {
        duckHitCount = 1;
        sr.sprite = oneDuckSprite;
        PlaySoundOnce(hitDuck);
        StartCoroutine(MoveUpPauseAndReturn());
    }

    public void DogHoldTwoDuck()
    {
        duckHitCount = 2;
        sr.sprite = twoDuckSprite;
        PlaySoundOnce(hitDuck);
        StartCoroutine(MoveUpPauseAndReturn());
    }

    private void DogJump()
    {
        // anim.SetTrigger("Jump") change name to trigger name
        PlaySoundOnce(bark);
        float jumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * jumpHeight);
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        StartCoroutine(JumpCoroutine());
        //duckMGR.DuckSpawner();
    }

    private IEnumerator JumpCoroutine()
    {
        while (rb.velocity.y > 0)
        {
            yield return null;
        }
        // anim.SetTrigger("jumpChange") change name to trigger name
        sr.sortingLayerName = backgroundSortingLayer;
        yield return new WaitForSeconds(.6f);
        duckMGR.DuckSpawner();    
    }
    private void DogMove()
    {
        anim.Play("Sniff");
        Vector2 move = moveRight.normalized * speed * Time.deltaTime;
        transform.position += (Vector3)move;
    }

    private IEnumerator MoveUpPauseAndReturn()
    {
        float initialGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);

        // Wait for the dog to reach the peak
        yield return new WaitForSeconds(0.5f);

        rb.velocity = Vector2.zero;

        // Pause at the peak
        yield return new WaitForSeconds(1.5f);

        // Apply downward force
        rb.gravityScale = 0.1f;

        // Wait until the dog lands
        while (!IsGrounded())
        {
            yield return null;
        }

        // Ensure the dog has landed
        // anim.SetTrigger("HoldDuck") change name to trigger name (or idle?)
        rb.gravityScale = initialGravityScale;
        rb.velocity = Vector2.zero;
        transform.position = originalPosition;
    }

    private bool IsGrounded()
    {
        // You can implement your own grounded check logic here
        return Mathf.Approximately(rb.velocity.y, 0f);
        // anim.SetTrigger("HoldDuck") change name to trigger name (or idle?)
    }

    private void PlaySoundOnce(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void ResetMovement()
    {
        isMoving = true; // Example of resetting the movement flag
        duckHitCount = 0; // Reset the duck hit count after popping out
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("JumpTrigger"))
        {
            // anim.SetTrigger("Alert") change name to trigger name
            isMoving = false;
            DogJump();
            Destroy(collision.gameObject);
        }
    }
}
