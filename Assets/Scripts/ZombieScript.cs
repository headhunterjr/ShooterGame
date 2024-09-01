using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
	public AudioClip zombieSound;
	public float health;
	public delegate void OnZombieDestroyed(object sender, EventArgs e);
	public static event OnZombieDestroyed zombieDestroyed;
	public float moveSpeed;
	public int bulletsToKill;

	private AudioSource audioSource;
	private GameObject player;
	private Transform targetPosition;

    // Start is called before the first frame update
    void Start()
    {
		health = 100;
		audioSource = GetComponent<AudioSource>();
		player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            targetPosition = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
		if (player != null && !player.IsDestroyed() && targetPosition != null)
		{
			MoveToPlayer();
		}
		else
		{
			moveSpeed = 0f;
		}
		if (health <= 0)
		{
			OnBulletHit();
			Destroy(gameObject);
		}
	}
	private void FixedUpdate()
	{
        
	}
    private void MoveToPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position,  targetPosition.position, moveSpeed);
    }
	private void Awake()
	{
		InvokeRepeating("PlayZombieSound", 0f, 5f);
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Bullet")
		{
			Destroy(collision.gameObject);
			health -= 100 / bulletsToKill + 1;
		}
	}
	public void OnBulletHit()
	{
		if (zombieDestroyed != null)
		{
			EventArgs e = new EventArgs();
			zombieDestroyed(this, e);
		}
	}
	void PlayZombieSound()
	{
		audioSource.PlayOneShot(zombieSound, 0.2f);
	}
	private void OnDestroy()
	{
		CounterScript.zombieCounter++;
		CounterScript.coinCounter++;
	}
}
