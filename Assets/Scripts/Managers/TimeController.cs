using System;
using System.Threading.Tasks;
using UnityEngine;

public class TimeController : IDisposable
{
    private const float PLAYWEEK_DURATION = 300f;
    private int _month = 1;
    public bool IsTimePaused;

    private bool _isRunning = true;





    // Time state events

    // In-game time events 
    public event Action OnNewPlayweekEvent;
    public event Action OnNewMonthEvent;


    public TimeController()
    {

    }


    public async Task Init()
    {
        Subscribe();

        _isRunning = true;
        await StartTimeCount();
    }

    private void Subscribe()
    {

    }

    private void UnSubscribe()
    {

    }

    private async Task StartTimeCount()
    {
        int currentPlayweek = 1;

        while (_isRunning)
        {
            float elapsedTime = 0f;

            while (elapsedTime < PLAYWEEK_DURATION)
            {
                if (!_isRunning)
                    return;
                    
                if (!IsTimePaused)
                    elapsedTime += 0.1f;

                await Task.Delay(100);
            }

            currentPlayweek++;
            Debug.Log("New playweek: " + currentPlayweek);
            OnNewPlayweekEvent?.Invoke();

            if (currentPlayweek % 4 == 0)
            {
                _month++;
                Debug.Log("Month " + _month + " has started");
                OnNewMonthEvent?.Invoke();
            }  
        }
    }


    public void Disable()
    {
        _isRunning = false;
        UnSubscribe();
    }

    public void Dispose()
    {
        _isRunning = false;
        UnSubscribe();
    }
}