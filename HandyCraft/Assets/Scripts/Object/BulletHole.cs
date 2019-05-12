using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHole : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _lastingTime;

    // Update is called once per frame
    void Update()
    {
        _lastingTime -= Time.deltaTime;
        if (_lastingTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
