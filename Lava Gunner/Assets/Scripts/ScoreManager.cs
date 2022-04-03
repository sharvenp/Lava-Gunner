using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public TextMeshProUGUI scoreText;

    private bool timerRunning;
    private float time;

	private void Start()
	{
        time = 0f;
        StartTimer();
	}

	private void Update()
    {
        if (timerRunning)
		{
            time += Time.deltaTime;
            scoreText.text = GetFormatTime();
        }
    }

    private string GetFormatTime()
	{
        return $"{time:0.00}";
	}
    
    public void StartTimer()
	{
        timerRunning = true;
	}

    public void PauseTimer()
	{
        timerRunning = false;
	}

    public void StopTimer()
	{
        time = 0f;
        timerRunning = false;
	}
}
