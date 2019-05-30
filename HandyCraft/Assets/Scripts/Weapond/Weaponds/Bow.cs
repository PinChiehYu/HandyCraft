using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Bow : Weapond
{
    public Transform _attachedArrow;
    public SkinnedMeshRenderer _bowMesh;
    public float _blendMultiplier = 255f;
    public GameObject _arrowPrefab;

    public float _maxShootSpeed;
    public AudioClip _fireSound;
    private bool isNocked { get { return _attachedArrow.gameObject.activeSelf; } }

    float time;
    void Update()
    {
        float distance = Vector3.Distance(transform.position, _attachedArrow.position);
        _bowMesh.SetBlendShapeWeight(0, Mathf.Max(0, distance * _blendMultiplier));

        if (!isNocked)
        {
            time += Time.deltaTime;
        }
        if (time > 5f)
        {
            time = 0f;
            NockArrow();
        }
    }

    public override void ChangeToOtherWeapond()
    {
        Destroy(gameObject);
    }

    public override void ChangeToThisWeapond()
    {   
    }

    protected override void Fire()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isNocked && other.GetComponent<Arrow>())
        {
            Destroy(other.gameObject);
            NockArrow();
        }
    }

    public void ShootArrow()
    {
        GameObject arrow = Instantiate(_arrowPrefab, transform.position, transform.rotation);
        float distance = Vector3.Distance(transform.position, _attachedArrow.position);

        arrow.GetComponent<Rigidbody>().velocity = arrow.transform.forward * distance * _maxShootSpeed;
        arrow.GetComponent<Arrow>().Launch();
        //AudioSource.PlayClipAtPoint(_fireSound, transform.position);

        UnnockArrow();
    }

    public void NockArrow()
    {
        _attachedArrow.gameObject.SetActive(true);
    }

    public void UnnockArrow()
    {
        _attachedArrow.localPosition = Vector3.zero;
        _attachedArrow.gameObject.SetActive(false);
        _bowMesh.SetBlendShapeWeight(0, 0);
    }
}
