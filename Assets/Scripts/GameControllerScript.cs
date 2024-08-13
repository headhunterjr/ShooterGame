using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Linq;
using System;

public class GameControllerScript : MonoBehaviour
{
    public GameObject winText;
    public GameObject gameOverText;
    public AudioClip winSound;
    public AudioClip gameOverSound;
    public TextMeshProUGUI zombieCounterText;
    public GameObject zombie;
    public GameObject player;

    private AudioSource audioSource;
    private GameObject[] walls;
    private GameObject[] zombies;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 1.0f;
        startTime = Time.time;
        zombieCounterText.text = $"Kill Count: {CounterScript.zombieCounter}\nCoins: {CounterScript.coinCounter}";
		walls = GameObject.FindGameObjectsWithTag("Wall");
        InvokeRepeating("SpawnZombies", 1f, 3f);

        ZombieScript.zombieDestroyed += OnBulletHit;
	}
    private void OnBulletHit(object sender, EventArgs e)
    {
        SpawnZombies();
    }
    // Update is called once per frame
    void Update()
    {
        float timePassed = Time.time - startTime;
        zombieCounterText.text = $"Kill Count: {CounterScript.zombieCounter}\nCoins: {CounterScript.coinCounter}";
        if (CounterScript.zombieCounter >= 30)
        {
            winText.SetActive(true);
            audioSource.PlayOneShot(winSound, 0.07f);
            Time.timeScale = 0f;
			StartCoroutine(LoadMainMenu(4f));
		}
        if (player.IsDestroyed() || timePassed >= 30f)
        {
            audioSource.PlayOneShot(gameOverSound, 0.07f);
            gameOverText.SetActive(true);
            Time.timeScale = 0f;
            StartCoroutine(LoadMainMenu(4f));
		}
    }
    void SpawnZombies()
    {
        zombies = GameObject.FindGameObjectsWithTag("Zombie");
        if (zombies.Length < 10)
        {
			GameObject randomWall = walls[UnityEngine.Random.Range(0, walls.Length)];
			Vector2 spawnPosition = randomWall.transform.position;
			Instantiate(zombie, spawnPosition, Quaternion.identity);
		}
	}
	private void OnDestroy()
	{
		ZombieScript.zombieDestroyed -= OnBulletHit;
	}
    IEnumerator LoadMainMenu(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
