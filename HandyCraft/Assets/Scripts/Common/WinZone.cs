using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    public GameObject fireWork;

    private void Start()
    {
        fireWork.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.FreezeGame) return;

        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Win();
            fireWork.SetActive(true);
        }
    }
}
