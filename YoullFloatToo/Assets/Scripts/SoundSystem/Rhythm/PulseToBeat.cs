using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PulseToBeat : MonoBehaviour
{
    [SerializeField] private int targetIntervalIndex = 0; // Specify the target interval index in the Inspector
    [SerializeField] private float _pulseSize = 1.15f;
    [SerializeField] private float _pulseReturnSpeed = 5f;
    private Vector3 _startSize;
    private BeatManager beatManager; // Store a reference to the BeatManager

    private void InitBeatManager()
    {
        if (beatManager == null)
        {
            beatManager = BeatManager.Instance;
        }
        
        // Subscribe to events when the GameObject is enabled
        if (beatManager != null)
        {
            if (targetIntervalIndex >= 0 && targetIntervalIndex < beatManager._intervals.Length)
            {
                beatManager._intervals[targetIntervalIndex].onInterval.AddListener(Pulse);
            }
            else
            {
                Debug.LogError("Invalid target interval index");
            }
        }
        else
        {
            Debug.LogError("Beat Manager is null");
        }
    }

    private void Start()
    {
        _startSize = transform.localScale;

        // Find the BeatManager and store a reference
        beatManager = BeatManager.Instance;
        InitBeatManager();
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _startSize, Time.deltaTime * _pulseReturnSpeed);
    }

    private void Pulse()
    {
        transform.localScale = _startSize * _pulseSize;
    }
}