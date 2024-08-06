using UnityEngine;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Randomly set the initial direction and speed for the target
        direction = new Vector2(Random.Range(-horizontalSpeed, horizontalSpeed), verticalSpeed).normalized;

        // Initialize starting position and maximum height
        startY = transform.position.y;
        maxY = startY + 10f; // Adjust this value based on your desired maximum height
    }

    void Update()
    {
        // Move the clay target
        rb.velocity = direction * speed;

        // Adjust scale based on vertical position
        float scale = Mathf.Lerp(maxScale, minScale, (transform.position.y - startY) / (maxY - startY));
        transform.localScale = new Vector3(scale, scale, 1);

        // Check if the clay target is off the screen
        if (!IsVisible())
        {
            Destroy(gameObject);
        }
    }

    private bool IsVisible()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}
