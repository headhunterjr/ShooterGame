using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float elapsedTime = 0f;
    private bool timerIsRunning = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
			elapsedTime += Time.deltaTime;
			int minutes = Mathf.FloorToInt(elapsedTime / 60);
			int seconds = Mathf.FloorToInt(elapsedTime % 60);
			timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
		}
    }
	public void StopTimer()
	{
		timerIsRunning = false;
	}

	public void StartTimer()
	{
		timerIsRunning = true;
	}
}
