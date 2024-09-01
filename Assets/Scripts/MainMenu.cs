using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public Toggle soundToggle;
    public AudioClip clickSound;
    public AudioClip menuMusic;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
		audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(menuMusic, 0.1f);
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(clickSound, 1.5f);
            }
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void MuteSounds()
    {
        AudioListener.volume = soundToggle.isOn ? 1.0f : 0.0f;
    }
    public void OpenLevel1()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
	public void OpenLevel2()
	{
		SceneManager.LoadScene(2, LoadSceneMode.Single);
	}
	public void OpenLevel3()
	{
		SceneManager.LoadScene(3, LoadSceneMode.Single);
	}
	public void OpenLevel4()
	{
		SceneManager.LoadScene(4, LoadSceneMode.Single);
	}
}
