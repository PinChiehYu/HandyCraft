using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    private PlayerMotor motor;

    [SerializeField]
    private AudioSource footAudio;
    public AudioClip WalkingSound;
    public AudioClip RunningSound;
    public PlayerState prevState;

    [SerializeField]
    private AudioSource breathAudio;
    public AudioClip breath;
    public float StartBreathingPorportion = 0.3f;
    private float lastEnergy;

    private CharacterInfo charInfo;
    private float maxEnergy;

    private void Awake()
    {
        charInfo = GetComponent<CharacterInfo>();
        motor = GetComponent<PlayerMotor>();
        prevState = PlayerState.Idle;
    }

    private void Start()
    {
        breathAudio.clip = breath;
        maxEnergy = charInfo.MaxEnergy;
    }

    private void Update()
    {
        FootStepSound();
        Breath();
    }

    private void FootStepSound()
    {
        if (prevState != motor.State)
        {
            footAudio.Stop();
        }
        prevState = motor.State;

        if (motor.State == PlayerState.Running)
        {
            footAudio.clip = RunningSound;
        }
        else if (motor.State == PlayerState.Walking)
        {
            footAudio.clip = WalkingSound;
        }
        else
        {
            return;
        }

        if (!footAudio.isPlaying)
        {
            footAudio.Play();
        }
    }

    private void Breath()
    {
        float energy = charInfo.CurrentEnergy;
        if (energy < maxEnergy * StartBreathingPorportion)
        {
            if (!breathAudio.isPlaying)
            {
                breathAudio.Play();
            }
            breathAudio.volume = 1f - energy / (maxEnergy * StartBreathingPorportion);
        }
        lastEnergy = energy;
    }
}
