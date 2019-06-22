using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public Transform root;
    [SerializeField]
    private bool isOpen;
    private bool isProcessing;
    private IEnumerator process;
    private AudioSource audio;

    public float rotateSpeed;

    private void Awake()
    {
        isOpen = false;
        isProcessing = false;
        process = SwitchDoor(false);
        audio = GetComponent<AudioSource>();
    }

    public void Pick(Transform hand)
    {
        Debug.Log("Open");
        if (isProcessing) return;
        isOpen = !isOpen;
        StopCoroutine(process);
        process = SwitchDoor(isOpen);
        StartCoroutine(process);
        audio.PlayOneShot(audio.clip, 1f);
    }

    public void Interact(Vector3 velocity, Vector3 angularVelocity)
    {
        return;
    }

    public bool Release(Vector3 velocity, Vector3 angularVelocity)
    {
        return true;
    }

    private IEnumerator SwitchDoor(bool open)
    {
        float remain = 90f;
        float sign = isOpen ? -1f : 1f;

        isProcessing = true;
        while (remain - rotateSpeed * Time.deltaTime > 0f)
        {
            root.Rotate(0f, rotateSpeed * sign * Time.deltaTime, 0f, Space.World);
            remain -= rotateSpeed * Time.deltaTime;
            yield return null;
        }
        root.Rotate(0f, remain * sign, 0f);
        isProcessing = false;
    }
}
