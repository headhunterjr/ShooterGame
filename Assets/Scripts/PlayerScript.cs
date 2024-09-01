using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float movementSpeed;
    public float jumpForce;
	public float timeToDie;
	public GameObject deductHealthMessage;
	public GameObject weapon;
	public Transform groundCheck;
	public LayerMask groundLayer;
	public AudioClip jumpSound;
	public GameObject bulletSpawnPoint;

	private float weaponOffsetValue = 1f;
	private float bulletSpawnPointOffsetValue = 3f;
	private float collisionWithZombieTimer = 0f;
	private bool isColliding = false;
	private bool isGrounded;
	private bool isMoving;
	private bool canDoubleJump = false;
	Rigidbody2D rb;
    SpriteRenderer sp;
	SpriteRenderer weaponSp;
	AudioSource audioSource;
    float xInput;
    float health;
    // Start is called before the first frame update
    void Start()
    {
		timeToDie *= 5;
		audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
		weaponSp = weapon.GetComponent<SpriteRenderer>();
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
		DeductHealth();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (isGrounded)
			{
				canDoubleJump = true;
				Jump();
			}
			else if (canDoubleJump)
			{
				canDoubleJump = false;
				Jump();
			}
		}
		if (Input.GetMouseButtonDown(0))
		{
			TurnToShoot();
		}
	}
	private void FixedUpdate()
	{
		//RaycastHit2D hit = Physics2D.Raycast(transform.position, movement, 0.1f);
		xInput = Input.GetAxis("Horizontal");
        transform.Translate(xInput * movementSpeed, 0, 0);
        rb.velocity = new Vector2(xInput * movementSpeed, rb.velocity.y);
		if (rb.velocity != Vector2.zero)
		{
			isMoving = true;
		}
		else
		{
			isMoving = false;
		}
		if (isMoving && !audioSource.isPlaying)
		{
			audioSource.Play();
		}
		if (!isMoving)
		{
			audioSource.Stop();
		}
		RaycastHit2D hit = Physics2D.Raycast(transform.position, rb.velocity, 2.5f);
		if (hit.collider != null && hit.collider.CompareTag("Wall"))
		{
			rb.velocity = Vector2.zero;
		}
		Flip();
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer);
	}
    void Flip()
    {
		if (rb.velocity.x < 0f)
		{
			sp.flipX = true;
			weaponSp.flipX = true;
		}
		else if (rb.velocity.x > 0f)
		{
			sp.flipX = false;
			weaponSp.flipX = false;
		}
		float weaponOffset = sp.flipX ? -weaponOffsetValue : weaponOffsetValue;
		weapon.transform.localPosition = new Vector2(weaponOffset, weapon.transform.localPosition.y);
		float bulletSpawnPointOffset = sp.flipX ? -bulletSpawnPointOffsetValue : bulletSpawnPointOffsetValue;
		bulletSpawnPoint.transform.localPosition = new Vector2(bulletSpawnPointOffset, bulletSpawnPoint.transform.localPosition.y);
	}
	void Jump()
    {
		audioSource.PlayOneShot(jumpSound, 0.5f);
		rb.velocity = Vector2.up * jumpForce;
    }
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Wall")
		{
            rb.velocity = Vector2.zero;
		}
        else if (collision.gameObject.tag == "Zombie")
        {
			isColliding = true;
        }
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Zombie")
		{
			isColliding = false;
		}
	}
	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Zombie")
		{
			isColliding = true;
		}
	}
	void DeductHealth()
    {
		if (isColliding)
		{
			collisionWithZombieTimer += Time.deltaTime;
			if (collisionWithZombieTimer >= timeToDie / 50)
			{
				health -= 10;
				deductHealthMessage.SetActive(true);
				collisionWithZombieTimer = 0f; 
			}
		}
		else
		{
			collisionWithZombieTimer = 0f;
		}
	}
	void TurnToShoot()
	{
		Vector3 mousePosition = Input.mousePosition;
		Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		if (worldMousePosition.x < transform.position.x && !sp.flipX)
		{
			sp.flipX = true;
			weaponSp.flipX = true;
		}
		else if (worldMousePosition.x > transform.position.x && sp.flipX)
		{
			sp.flipX = false;
			weaponSp.flipX = false;
		}
		float weaponOffset = sp.flipX ? -weaponOffsetValue : weaponOffsetValue;
		weapon.transform.localPosition = new Vector2(weaponOffset, weapon.transform.localPosition.y);

		float bulletSpawnPointOffset = sp.flipX ? -bulletSpawnPointOffsetValue : bulletSpawnPointOffsetValue;
		bulletSpawnPoint.transform.localPosition = new Vector2(bulletSpawnPointOffset, bulletSpawnPoint.transform.localPosition.y);
	}
}
