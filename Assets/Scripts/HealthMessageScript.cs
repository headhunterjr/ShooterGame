using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMessageScript : MonoBehaviour
{
	private float deductHealthMessageTime = 0.5f;
    private float messageTimer = 0f;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        messageTimer += Time.deltaTime;
        if (messageTimer >= deductHealthMessageTime)
        {
            gameObject.SetActive(false);
            messageTimer = 0f;
        }
    }
}
