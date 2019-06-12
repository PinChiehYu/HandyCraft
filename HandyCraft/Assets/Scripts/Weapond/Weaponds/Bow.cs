using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Bow : Weapond
{
    public Transform attachedArrow;
    public Animator animator;
    private Vector3 arrowOriginePosition;
    public float animationParam;

    public GameObject arrowPrefab;
    public GameObject ultPrefab;

    public float reloadPeriod;
    public float maxShootSpeed;
    public AudioClip _fireSound;
    private bool isNocked { get { return attachedArrow.gameObject.activeSelf; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        arrowOriginePosition = attachedArrow.localPosition;
    }

    float nockTimer;
    private void Update()
    {
        UpdateBowStretching();

        if (!isNocked)
        {
            nockTimer += Time.deltaTime;
        }
        if (nockTimer > reloadPeriod)
        {
            nockTimer = 0f;
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

    protected override void Fire(Vector3 velocity, Vector3 angularVelocity)
    {
    }

    private void UpdateBowStretching()
    {
        float distance = Vector3.Distance(arrowOriginePosition, attachedArrow.localPosition);
        animator.Play(0, 0, distance / animationParam); // min/max:0.255/-0.225 length:0.48
    }

    public void ShootArrow(bool isUlt)
    {
        GameObject arrow;
        if (isUlt)
        {
            arrow = Instantiate(ultPrefab, transform.TransformPoint(arrowOriginePosition), transform.rotation);
        }
        else
        {
            arrow = Instantiate(arrowPrefab, transform.TransformPoint(arrowOriginePosition), transform.rotation);
        }

        float distance = Vector3.Distance(arrowOriginePosition, attachedArrow.localPosition);
        Debug.Log("Arrow Pulling Distance:" + distance.ToString());
        arrow.GetComponent<Rigidbody>().velocity = arrow.transform.forward * distance * maxShootSpeed;
        if (isUlt)
        {
            arrow.GetComponent<DragonStrike>().Launch();
        }
        else
        {
            arrow.GetComponent<Arrow>().Launch();
        }
        //AudioSource.PlayClipAtPoint(_fireSound, transform.position);

        UnnockArrow();
    }

    public void NockArrow()
    {
        attachedArrow.gameObject.SetActive(true);
    }

    public void UnnockArrow()
    {
        attachedArrow.localPosition = arrowOriginePosition;
        attachedArrow.gameObject.SetActive(false);
        animator.Play(0, 0, 0);
    }
}
