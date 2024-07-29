using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))] // can switch to box if needed/wanted

public class DogAI : MonoBehaviour
{
    // Add reference to where the score is saved/generated
    // Add reference to ammo count/end game

    // GameObjects
    public GameObject dog; // For location/movement
    public float speed = 3.0f;
    private Vector2 moveRight = Vector2.right;
    private Vector2 moveUp = Vector2.up;
    public float jumpHeight = 5.0f;
    public float peekHeight = 10.0f;

    // Components
    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb;

    // Animations
    public AnimationClip Laugh;
    public AnimationClip Jump;
    public AnimationClip Sniff;
    public AnimationClip HoldOneDuck;
    public AnimationClip HoldTwoDuck;
    public AnimationClip Run;
    public AnimationClip Alert;
    public AnimationClip Clap;
    public AnimationClip InjuredScold;
    public AnimationClip InjuredStare;
    public AnimationClip InjuredWalk;

    // Triggers
    public GameObject jumpTrigger;

    // Layermasks
    public string backgroundSortingLayer = "Background"; 
    public string foregroundSortingLayer = "Foreground";

    //AudioClips
    public AudioSource audioSource;
    public AudioClip bark;
    public AudioClip laugh;
    public AudioClip hitDuck;


    private bool isMoving = true;



    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1.0f;

        // jumpTrigger should be assigned in the Inspector or dynamically found in the scene if needed

    }

    void Update()
    {
        // Could make dog state 1,2,3 to determine or integrate to other scripts to update the functions below 
        if (isMoving)
        {
            DogMove();
        }
        // added to debug... remove when ready!!!!
        if (Input.GetKey(KeyCode.P))
        {
            DogLaugh();
        }
        if (Input.GetKey(KeyCode.O))
        {
            DogHoldDuck();
        }
        if (Input.GetKey(KeyCode.I))
        {
            DogHoldTwoDuck();
        }
        //if (Input.GetKey(KeyCode.U))
        //{

        //}
    }

    private void DogLaugh()
    {
        // Animation plays when out of ammo/miss/endgame

        // anim.Play(Laugh.name);

        float jumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * peekHeight);
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        audioSource.PlayOneShot(laugh);
        anim.Play("Laugh");
        StartCoroutine(WaitForAnimationToFinish());
    }

    private void DogHoldDuck()
    {
        // Moves up/down holding duck when duck is hit after duck has hit the ground
        // Animation plays when duck is hit

        float jumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * peekHeight);
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);

        anim.Play("HoldOneDuck");
        audioSource.PlayOneShot(hitDuck);

        StartCoroutine(WaitForAnimationToFinish());
    }

    private void DogHoldTwoDuck()
    {
        // Moves up/down holding duck when duck is hit after duck has hit the ground
        // Animation plays when duck is hit

        float jumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * peekHeight);
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);

        anim.Play("HoldTwoDuck");
        audioSource.PlayOneShot(hitDuck);

        StartCoroutine(WaitForAnimationToFinish());
    }

    private IEnumerator WaitForAnimationToFinish()
    {
        // Get the length of the laugh anim
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        yield return new WaitForSeconds(animationLength);

        // Apply downward velocity after the animation completes
        float fallVelocity = -Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * peekHeight);
        rb.velocity = new Vector2(rb.velocity.x, fallVelocity);
    }

    private void DogJump()
    {
        // Moves from foreground to background layer

        // anim.Play(Jump.name);
        
        audioSource.PlayOneShot(bark);
        float jumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics2D.gravity.y) * jumpHeight);
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        StartCoroutine(JumpCoroutine());
    }

    //Jump coroutine
    private IEnumerator JumpCoroutine()
    {
        while (rb.velocity.y > 0)
        {
            //add anim event trigger for jump phase 1 (upward)
            yield return null; // Wait until the dog starts falling
        }
        //add anim event trigger for jump phase 2 (downward)
        sr.sortingLayerName = backgroundSortingLayer;
    }


    private void DogMove()
    {
        // Walks out at start
        
        anim.Play("Sniff");
        Vector2 move = moveRight.normalized * speed * Time.deltaTime;
        transform.position += (Vector3)move;
    }

    //Triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("JumpTrigger"))
        {
            isMoving = false;
            DogJump();
            Destroy(collision.gameObject);
        }
    }
}
