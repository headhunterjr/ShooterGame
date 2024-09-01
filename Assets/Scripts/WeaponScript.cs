using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponScript : MonoBehaviour
{
	public GameObject bulletPrefab;
	public GameObject bulletSpawnPosition;
	public AudioClip gunshotSound;
	public AudioClip emptyGunshotSound;
	public TextMeshProUGUI ammoCounter;
	public GameObject player;
	public int magazineCapacity;
	public int reloadCost;

	private int maxMagazineCapacity;

	// Start is called before the first frame update
	void Start()
    {
		maxMagazineCapacity = magazineCapacity;
	}

	// Update is called once per frame
	void Update()
    {
		ammoCounter.text = $"Ammo: {magazineCapacity}";
		if (Input.GetMouseButtonDown(0))
		{
			if (magazineCapacity > 0)
			{
				Invoke("CreateBullet", 0.1f);
				magazineCapacity--;
				GetComponent<AudioSource>().PlayOneShot(gunshotSound, 0.1f);
			}
			else
			{
				GetComponent<AudioSource>().PlayOneShot(emptyGunshotSound, 0.5f);
			}
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			ReloadMagazine();
		}
	}
	void CreateBullet()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 direction = (mousePos - bulletSpawnPosition.transform.position).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition.transform.position, Quaternion.Euler(0f, 0f, angle));
		BulletScript bulletScript = bullet.GetComponent<BulletScript>();
		bulletScript.player = player;
		bulletScript.SetDirection(direction);
	}
	void ReloadMagazine()
	{
		if (CounterScript.coinCounter >= reloadCost)
		{
			magazineCapacity = maxMagazineCapacity;
			CounterScript.coinCounter -= reloadCost;
		}
	}
}
