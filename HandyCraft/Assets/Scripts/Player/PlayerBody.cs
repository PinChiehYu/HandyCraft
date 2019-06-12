using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    public Transform headTransform;

    // Update is called once per frame
    private void Update()
    {
        Vector3 localPosition = headTransform.localPosition;
        localPosition.y = 0f;
        transform.localPosition = localPosition;
        Vector3 localRotation = headTransform.localRotation.eulerAngles;
        localRotation.x = 0f;
        localRotation.z = 0f;
        transform.localRotation = Quaternion.Euler(localRotation);
    }
}
