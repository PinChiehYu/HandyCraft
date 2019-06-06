using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
    public Transform headTransform;

    // Update is called once per frame
    private void Update()
    {
        Vector3 localPosition = headTransform.localPosition;
        localPosition.y = 0f;
        transform.localPosition = localPosition;
    }
}
