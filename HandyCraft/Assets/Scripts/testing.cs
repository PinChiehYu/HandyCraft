using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{

    // Update is called once per frame
    public Vector3 angle = Vector3.zero;
    public float timer;
    public bool start = false;
    Vector3 rotatingAxis;
    void Update()
    {
        if (!start) return;
        angle.x = (angle.x + 100f * Time.deltaTime) % 360f;
        Debug.DrawRay(transform.position, transform.right);
        transform.rotation = Quaternion.FromToRotation(Vector3.right, rotatingAxis) * Quaternion.Euler(angle);
    }

    public void Trigger()
    {
        start = !start;
        rotatingAxis = transform.right;
        angle.x = (Quaternion.FromToRotation(transform.right, Vector3.right) * transform.rotation).eulerAngles.x;
    }
}
