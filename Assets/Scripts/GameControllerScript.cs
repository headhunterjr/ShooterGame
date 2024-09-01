using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Linq;
using System;
using JetBrains.Annotations;

public class GameControllerScript : MonoBehaviour
{
    public GameObject winText;
    public GameObject gameOverText;
    public AudioClip winSound;
    public AudioClip gameOverSound;
    public TextMeshProUGUI zombieCounterText;
    public TextMeshProUGUI coinCounterText;
    public TextMeshProUGUI objectiveText;
    public GameObject zombie;
    public GameObject player;
    public int zombieKillsRequired;
    public int secondsToWin;
    public float timeBetweenSpawns;
    public TimerScript timer;

    private AudioSource audioSource;
    private GameObject[] walls;
    private GameObject[] zombies;
    private float startTime;
    private bool gameEnded = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //Time.timeScale = 1.0f;
        startTime = Time.time;
		walls = GameObject.FindGameObjectsWithTag("Wall").Where(o => o.activeSelf).ToArray();
        InvokeRepeating("SpawnZombies", 1f, timeBetweenSpawns);

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
        objectiveText.text = $"Objective: Kill {zombieKillsRequired} zombies in {secondsToWin} seconds";
        zombieCounterText.text = $"Kill Count: {CounterScript.zombieCounter}";
        coinCounterText.text = $"Coins: {CounterScript.coinCounter}";
		zombies = GameObject.FindGameObjectsWithTag("Zombie");
        if (!gameEnded)
        {
			if (CounterScript.zombieCounter >= zombieKillsRequired)
			{
				gameEnded = true;
				winText.SetActive(true);
				audioSource.PlayOneShot(winSound, 0.25f);
				foreach (GameObject z in zombies)
				{
					CounterScript.zombieCounter--;
					CounterScript.coinCounter--;
					Destroy(z);
				}
				timer.StopTimer();
                CancelInvoke("SpawnZombies");
				StartCoroutine(LoadMainMenu(4f));
			}
			if (player.IsDestroyed() || timePassed >= secondsToWin)
			{
				gameEnded = true;
				gameOverText.SetActive(true);
				audioSource.PlayOneShot(gameOverSound, 0.2f);
				foreach (GameObject z in zombies)
				{
					CounterScript.zombieCounter--;
					CounterScript.coinCounter--;
					Destroy(z);
				}
				timer.StopTimer();
				CancelInvoke("SpawnZombies");
				StartCoroutine(LoadMainMenu(4f));
			}
		}
    }
    void SpawnZombies()
    {
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
