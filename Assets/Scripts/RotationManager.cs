using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RotationManager: MonoBehaviour
{
    [SerializeField] private float gyroForwardMin = -.6f;
    [SerializeField] private float gyroForwardMax = .6f;

    private RotationState currentRotation;
    public RotationState CurrentRotation
    {
        get => currentRotation;
    }
    
    public EventHandler<RotationState> OnRotationChange;

    void Awake()
    {
        Input.gyro.enabled = true;
    }

    void Update()
    {
#if !UNITY_EDITOR
        switch(GetCurrentRotation())
        {
            case RotationState.Down:
                if (currentRotation != RotationState.Down)
                {
                    currentRotation = RotationState.Down;
                    //bg.color = Color.red;
                    OnRotationChange?.Invoke(this, currentRotation);
                }
                break;
            case RotationState.Up:
                if (currentRotation != RotationState.Up)
                {
                    currentRotation = RotationState.Up;
                    //bg.color = Color.green;
                    OnRotationChange?.Invoke(this, currentRotation);
                }
                break;
            case RotationState.Forward:
                if (currentRotation != RotationState.Forward)
                {
                    currentRotation = RotationState.Forward;
                    //bg.color = Color.blue;
                    OnRotationChange?.Invoke(this, currentRotation);
                }
                break;
        }
#else

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            OnRotationChange?.Invoke(this, RotationState.Down);
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            OnRotationChange?.Invoke(this, RotationState.Up);
        }
        //gravity.text = ""+Input.gyro.gravity.y;
#endif
    }

    private RotationState GetCurrentRotation()
    {
        if (Input.gyro.gravity.y > gyroForwardMin && Input.gyro.gravity.y < gyroForwardMax)
        {
            if (Input.gyro.gravity.z < 0)
            {
                return RotationState.Up;
            }
            else
            {
                return RotationState.Down;
            }
        }
        else
        {
            return RotationState.Forward;
        }
    }

    public enum RotationState
    {
        Down,
        Up,
        Forward
    }


}
