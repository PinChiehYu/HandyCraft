using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    private EnemyMotor motor;

    [SerializeField]
    private AudioSource mouthAudio;
    [SerializeField]
    private AudioClip moveClip;
    [SerializeField]
    private AudioClip attackClip;

    public float yellingDelay = 2f;
    private float yellingTimer;

    private void Awake()
    {
        motor = GetComponent<EnemyMotor>();
        yellingTimer = 0f;
    }

    private void Update()
    {
        Yelling();
    }

    private void Yelling()
    {
        if (motor.State == EnemyState.Moving && !mouthAudio.isPlaying)
        {
            mouthAudio.clip = moveClip;
            mouthAudio.Play();
        }
    }

    public void Attack()
    {
        mouthAudio.PlayOneShot(attackClip, 1f);
    }
}
