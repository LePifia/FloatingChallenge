using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance;

    public float _bpm;
    public AudioSource _audioSource;
    public Intervals[] _intervals;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (_audioSource)
        {
            foreach (Intervals interval in _intervals)
            {
                float sampledTime = (_audioSource.timeSamples/ (_audioSource.clip.frequency*interval.GetIntervalLength(_bpm)));
                interval.CheckForNewInterval(sampledTime);
            }
        }
    }
}

[System.Serializable]
public class Intervals
{
    [SerializeField] private float _steps = 1f;
    public UnityEvent onInterval;
    private int _lastInterval;

    public float Steps
    {
        get { return _steps; }
        set { _steps = value; }
    }

    public float GetIntervalLength(float bpm)
    {
        return 60f / (bpm * _steps);
    }

    public void CheckForNewInterval(float interval)
    {
        if (Mathf.FloorToInt(interval) != _lastInterval)
        {
            _lastInterval = Mathf.FloorToInt(interval);
            onInterval.Invoke();
        }
    }
}
