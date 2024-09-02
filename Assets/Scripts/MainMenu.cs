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
    public Button[] levelButtons;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
		audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(menuMusic, 0.7f);
		InitializeLevelButtons();
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(clickSound);
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
    public void OpenLevel(int levelIndex)
    {
        if (IsLevelUnlocked(levelIndex))
        {
			SceneManager.LoadScene(levelIndex, LoadSceneMode.Single);
		}
    }
	private void InitializeLevelButtons()
	{
		int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);
		for (int i = 0; i < levelButtons.Length; i++)
		{
			if (i + 1 <= unlockedLevels)
			{
				levelButtons[i].interactable = true;
				levelButtons[i].onClick.AddListener(() => OpenLevel(i + 1));
			}
			else
			{
				levelButtons[i].interactable = false;
                levelButtons[i].GetComponent<Image>().color = Color.red;
			}
		}
	}
	private bool IsLevelUnlocked(int levelIndex)
	{
		int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);
		return levelIndex <= unlockedLevels;
	}
	public void ResetProgress()
	{
		PlayerPrefs.DeleteKey("UnlockedLevels");
		InitializeLevelButtons();
	}
}
