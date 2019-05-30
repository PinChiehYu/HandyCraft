using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{
    private VRInputModule _inputModule;
    public float _defaultLength = 5.0f;
    private LineRenderer _lineRenderer;
    [SerializeField]
    private GameObject _dot;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _inputModule = GameObject.Find("VRInputModule").GetComponent<VRInputModule>();
    }

    private void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        PointerEventData data = _inputModule.GetData();
        float targetLength = data.pointerCurrentRaycast.distance == 0 ? _defaultLength : data.pointerCurrentRaycast.distance;
        RaycastHit hit = CreateRaycast(targetLength);

        Vector3 endPosition = transform.position + (transform.forward * targetLength);
        if (hit.collider != null)
        {
            endPosition = hit.point;
        }

        _dot.transform.position = endPosition;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, endPosition);
    }

    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, length);

        return hit;
    }
}
