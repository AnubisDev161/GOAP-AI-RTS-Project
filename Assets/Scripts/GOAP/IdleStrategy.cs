using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IdleStrategy : IActionStrategy
{
    public bool canPerform => true; // agent can always idle
    public bool complete { get; private set; }

    readonly CountDowntimer timer;

    public IdleStrategy(float duration)
    {
        timer = new CountDowntimer(duration);
        timer.OnTimerStart += () => complete = false;
        timer.OnTimerStop += () => complete = true;
    }

    public void Start() => timer.Start();
    public void Update(float deltaTime)
    {
        timer.Tick(deltaTime);
    }
   
}

public class CountDowntimer
{
    float currentTimeInterval;
    float timerInterval;
    bool isRunning = false;

    public event Action OnTimerStart;
    public event Action OnTimerStop;
    public CountDowntimer(float duration) 
    {
        timerInterval = duration;
    }

    public void Tick(float deltaTime)
    {
        if(isRunning)
        {
            currentTimeInterval += Time.deltaTime;
            if (currentTimeInterval >= timerInterval)
            {
                currentTimeInterval = 0;
                Stop();
            }
        }
    }

    public void Start()
    {
        isRunning = true;
        OnTimerStart?.Invoke(); // Trigger event
    }

    public void Stop()
    {
        isRunning = false;
        OnTimerStop?.Invoke(); // Trigger event
    }
}

