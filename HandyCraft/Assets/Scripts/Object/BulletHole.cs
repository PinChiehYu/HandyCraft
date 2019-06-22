using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHole : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float lastingTime;

    // Update is called once per frame
    void Update()
    {
        lastingTime -= Time.deltaTime;
        if (lastingTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
