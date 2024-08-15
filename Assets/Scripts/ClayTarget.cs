using UnityEngine;
using System.Collections;

public class ClayTarget : MonoBehaviour
{
    public float speed = 5f; // Speed of the clay target
    public float verticalSpeed = 2f; // Vertical speed component
    public float horizontalSpeed = 2f; // Horizontal speed component
    public Vector2 direction; // Movement direction
    public float minScale = 0.5f; // Minimum scale value when the target is at its highest point
    public float maxScale = 1f; // Maximum scale value when the target is at its lowest point
    private Rigidbody2D rb;
    private float startY;
    private float maxY;

    public bool isDead = false;
    public AmmoManager ammoManager;
    private Animator animator;
    public int clayType = 1;

    public AudioSource audioSource;
    public AudioClip flyClip;

    public DuckHit DuckHitUI;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Ensure the Animator is referenced
        ammoManager = FindObjectOfType<AmmoManager>();
        DuckHitUI = FindObjectOfType<DuckHit>();
        if (ammoManager == null)
        {
            Debug.LogError("AmmoManager not found in the scene!");
        }

        // Randomly set the initial direction and speed for the target
        float angleRange = horizontalSpeed * 0.5f;
        direction = new Vector2(Random.Range(-angleRange, angleRange), verticalSpeed).normalized;

        // Initialize starting position and maximum height
        startY = transform.position.y;
        maxY = startY + 10f; // Adjust this value based on your desired maximum height

        PlaySoundOnce(flyClip);
    }

    void Update()
    {
        if (!isDead)
        {
            // Move the clay target
            rb.velocity = direction * speed;
            

            // Adjust scale based on vertical position
            float scale = Mathf.Lerp(maxScale, minScale, (transform.position.y - startY) / (maxY - startY));
            transform.localScale = new Vector3(scale, scale, 1);



            // Check for mouse click
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (GetComponent<Collider2D>().OverlapPoint(mousePosition))
                {
                    OnHit();
                   // DuckHitUI.RegisterHit();
                }

            }
        }

        // Check if the clay target is off the screen
        if (!IsVisible())
        {
            Destroy(gameObject);
        }
    }

    private void OnHit()
    {
        rb.gravityScale = 0;
        rb.gameObject.layer = LayerMask.NameToLayer("DeadDuck");
        StartCoroutine(clayHit());
        ammoManager.UpdateAmmo();
        Destroy(gameObject);
    }

    private IEnumerator clayHit()
    {
        animator.SetTrigger("ClayHit");
        isDead = true;
        direction = Vector2.zero;

        if (clayType == 1)
        {
            IPMScoreManager.Instance._BlackDuck();
        }

        yield return new WaitForSeconds(0.5f);
        direction = new Vector2(0, -1); // Make the clay target fall down

        IPMScoreManager.Instance.ScoreSpawn(transform.position, clayType);
    }

    private bool IsVisible()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }


    private void PlaySoundOnce(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
