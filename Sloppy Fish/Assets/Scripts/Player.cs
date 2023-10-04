using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;
    private Vector3 direction;
    public float gravity = -9.8f;
    public float strength = 5f;

    [SerializeField] private AudioSource jumpSoundEffect;

    public GameManager gameManager; // Reference to the GameManager

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
     //   gameManager = FindObjectOfType<GameManager>(); // Find and store the GameManager reference
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            jumpSoundEffect.Play();
            direction = Vector3.up * strength;
        }

        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;
    }

 

    private void AnimateSprite()
    {
        spriteIndex++;

        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }

        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            gameManager.GameOver(); // Call the GameManager's GameOver method through the reference
        }
        else if (other.gameObject.CompareTag("Scoring"))
        {
            gameManager.IncreaseScore(); // Call the GameManager's IncreaseScore method through the reference
        }
    }
}
