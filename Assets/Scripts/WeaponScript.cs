using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponScript : MonoBehaviour
{
	public Button reloadButton;
	public GameObject bulletPrefab;
	public AudioClip gunshotSound;
	public AudioClip emptyGunshotSound;
	private int magazineCapacity;

	// Start is called before the first frame update
	void Start()
    {
		magazineCapacity = 10;
	}

	// Update is called once per frame
	void Update()
    {
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
		GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
	}
	void ReloadMagazine()
	{
		if (CounterScript.coinCounter >= 3)
		{
			magazineCapacity = 10;
			CounterScript.coinCounter -= 3;
		}
	}
}
