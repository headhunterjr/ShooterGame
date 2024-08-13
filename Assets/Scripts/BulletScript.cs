using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
	public float bulletSpeed;
	public GameObject weapon;
	public float lifeTime;
	public GameObject player;

	private Vector2 targetDirection;
	private bool isMoving;
	private float lifeTimer;
	private void Start()
	{
		lifeTimer = 0f;
		isMoving = true;
		Vector3 mousePosition = Input.mousePosition;
		Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

		targetDirection = (worldMousePosition - (Vector2)transform.position).normalized;
	}
	void Update()
	{
		lifeTimer += Time.deltaTime;
		if (lifeTimer >= lifeTime)
		{
			Destroy(gameObject);
		}
	}
	void FixedUpdate()
	{
		if (isMoving)
		{
			transform.position += (Vector3)(targetDirection * bulletSpeed * Time.deltaTime);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			Destroy(gameObject);
		}
	}
	private void Awake()
	{
		Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
	}
}
