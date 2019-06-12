using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Weapond
{
    private new Rigidbody rigidbody;
    private new Collider collider;

    private Vector3 localPosition;
    private Vector3 localRotation;

    private Vector3 anchorPoint;
    private Vector3 stopPosition;

    public float returnAngle = 35f;
    public float throwingForce = 5f;
    private float rotateForce;
    private Vector3 rotatingAxis;
    public float rotateSpeed;
    private float currentAngle;

    private bool isInHand;
    private bool isReturning;
    private bool isStuck;
    private float timer, coolDown;

    public int damage = 90;

    public override void ChangeToOtherWeapond()
    {
        Destroy(gameObject);
    }

    public override void ChangeToThisWeapond()
    {
        localPosition = transform.localPosition;
        localRotation = transform.localEulerAngles;
    }

    protected override void Fire(Vector3 velocity, Vector3 angularVelocity)
    {
        if (isInHand && coolDown > 0.1f)
        {
            Throw(velocity, angularVelocity);
        }
        else if (!isReturning && (coolDown > 1f || (isStuck && coolDown > 0.1f)))
        { 
            ReturnToHand();
        }
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        isInHand = true;
        isReturning = false;
        isStuck = false;
        currentAngle = 0f;
        coolDown = 1f;
        timer = 0f;
    }

    private void Update()
    {
        coolDown += Time.deltaTime;
        if (isReturning)
        {
            Debug.Log("Is Returning");
            if (timer < 1f)
            {
                rigidbody.position = BezierQuadraticCurvePoint(timer, stopPosition, GetAnchorPoint(), hand.position);
                if (timer < 0.9f)
                {
                    currentAngle = (currentAngle - rotateSpeed * 15f * Time.deltaTime + 360f) % 360f;
                    rigidbody.rotation = Quaternion.FromToRotation(Vector3.right, rotatingAxis) * Quaternion.AngleAxis(currentAngle, Vector3.right);
                }
                else
                {
                    rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, hand.rotation, 0.1f);
                }

                timer += Time.deltaTime;
            }
            else
            {
                ResetAxe();
            }
        }
        else if (!isInHand && !isStuck)
        {
            currentAngle = (currentAngle + rotateSpeed * rotateForce * Time.deltaTime) % 360f;
            rigidbody.rotation = Quaternion.FromToRotation(Vector3.right, rotatingAxis) * Quaternion.AngleAxis(currentAngle, Vector3.right);
        }
    }

    private void Throw(Vector3 velocity, Vector3 angularVelocity)
    {
        transform.parent = null;
        rotatingAxis = transform.right;
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        rigidbody.AddForce(velocity * throwingForce, ForceMode.Impulse);
        currentAngle = (Quaternion.FromToRotation(transform.right, Vector3.right) * transform.rotation).eulerAngles.x;
        rotateForce = angularVelocity.magnitude;
        Debug.Log("Throw out:" + angularVelocity.magnitude.ToString());
        isInHand = false;
        coolDown = 0f;
    }

    private void ReturnToHand()
    {
        rigidbody.useGravity = false;
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
        }
        stopPosition = transform.position;
        GetAnchorPoint();
        currentAngle = (Quaternion.FromToRotation(transform.right, Vector3.right) * transform.rotation).eulerAngles.x;
        isReturning = true;
        isStuck = false;
    }

    private void ResetAxe()
    {
        rigidbody.isKinematic = true;
        transform.SetParent(hand);
        transform.localPosition = localPosition;
        transform.localRotation = Quaternion.Euler(localRotation);
        isInHand = true;
        isReturning = false;
        isStuck = false;
        coolDown = 0f;
        timer = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (!isInHand && !isStuck)
        {
            rigidbody.velocity = Vector3.zero;
            isStuck = true;
            coolDown = 0f;

            if (!isReturning)
            {
                gameObject.AddComponent<FixedJoint>().connectedBody = other.GetComponent<Rigidbody>();
            }
        }

        if (other.CompareTag("Enemy"))
        {
            other.GetComponentInParent<IAttackable>().GetAttack(damage, other.transform, transform.position);
        }
    }

    private Vector3 BezierQuadraticCurvePoint(float time, Vector3 hand, Vector3 anchor, Vector3 axe)
    {
        float u = 1 - time;
        float tt = time * time;
        float uu = u * u;
        Vector3 result = (uu * hand) + (2 * u * time * anchor) + (tt * axe);
        return result;
    }

    private Vector3 GetAnchorPoint()
    {
        float distance = (hand.position - stopPosition).magnitude;
        Vector3 direction = (hand.position - stopPosition).normalized;
        return stopPosition + Quaternion.AngleAxis(-1f * returnAngle, Vector3.up) * direction * distance / 2f + Vector3.up;
    }
}