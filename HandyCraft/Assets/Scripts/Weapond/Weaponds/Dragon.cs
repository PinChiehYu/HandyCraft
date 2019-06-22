using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dragon : MonoBehaviour
{

    public float speed = 5;

    [Space]
    [Header("Shader Settings")]
    public float initialDissolveValue;
    public float finalDissolveValue;
    public float dissolveSpeed = 10;
    public float firstDragonOffset = 3;

    [Space]
    public float destroyTime = 5;

    private Renderer[] renderers;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();

        foreach (Material m in renderers[0].materials)
        {
            m.SetFloat("_TimeOffset", firstDragonOffset);
        }

        foreach(Renderer r in renderers)
        {
            Material[] materials = r.materials;

            foreach (Material m in materials)
            {
                m.SetFloat("_SplitValue", initialDissolveValue);
                m.DOFloat(initialDissolveValue, "_SplitValue", 1).SetDelay(destroyTime).OnComplete(() => Destroy(gameObject));
            }
        }
    }

    private void Update()
    {
        transform.localPosition += transform.forward * Time.deltaTime * speed;

        foreach (Renderer r in renderers)
        {
            Material[] materials = r.materials;
            foreach (Material m in materials)
            {
                m.SetFloat("_SplitValue", m.GetFloat("_SplitValue") + (dissolveSpeed * Time.deltaTime * speed));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Enemy"))
        {
            other.GetComponentInParent<IAttackable>().GetAttack(100, other.transform, transform.position);
        }
    }
}
