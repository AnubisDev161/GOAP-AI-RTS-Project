using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float duration = 10f; // Total duration of the timer
    public bool start_at_begin = true;
    private float timeRemaining; // Time left on the timer
    private bool isRunning = false; // Whether the timer is active
    Unit unit; 

    private void Start()
    {
        unit = GetComponent<Unit>();
        if (start_at_begin)
        {
            StartTimer(); // Start the timer when the game begins}
          

        }
    }

    private void Update()
    {
        if (isRunning)
        {
            // Update the timer
            timeRemaining -= Time.deltaTime;
            
            // Check if the timer has reached zero
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isRunning = false;

                OnTimerEnd(); // Call the timer end event
            }
            //print("Time left:" + timeRemaining.ToString());
        }
    }

    public void StartTimer()
    {
        timeRemaining = duration; // Reset the timer
        isRunning = true; // Start the timer
    }

    private void OnTimerEnd()
    {
        // Add logic for when the timer ends (e.g., game over, spawn enemies, etc.)
    }

    public void timer_force_time_out()
    {
        timeRemaining = 0f;
    }
}

