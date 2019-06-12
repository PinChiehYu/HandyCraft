using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IAttackable
{
    // Start is called before the first frame update
    [SerializeField]
    private float detectRange;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float speedupDelay;
    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private float recoveryTime;

    private EnemyMotor motor;
    private CharacterInfo charInfo;
    private Animator animator;

    private Transform playerTrans;

    [SerializeField]
    private bool isDetected;
    [SerializeField]
    private bool isSpeedUp;
    [SerializeField]
    private bool isAttacking;
    [SerializeField]
    private bool isDead;

    private float attackTimer;
    private float speedUpTimer;

    private void Awake()
    {
        motor = GetComponent<EnemyMotor>();
        charInfo = GetComponent<CharacterInfo>();
        animator = GetComponent<Animator>();
        isDetected = false;
        isSpeedUp = false;
        isAttacking = false;
        isDead = false;
        attackTimer = attackCooldown;
    }

    private void Start()
    {
        charInfo.OnDie += Dead;
        playerTrans = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (!(isAttacking || isDead))
        {
            float distance = Vector3.Distance(playerTrans.position, transform.position);
            if (distance < attackRange)
            {
                motor.SetBodyMovement(Vector3.zero);
                isDetected = false;
                isSpeedUp = false;
                isAttacking = true;
            }
            else if (distance < detectRange)
            {
                DetectPlayer();
            }
            else
            {
                isDetected = false;
                isAttacking = false;
                CancelSpeedUp();
                motor.SetBodyMovement(Vector3.zero);
            }
        }
        else if (isAttacking)
        {
            AttackPlayer();
        }

        UpdateAnimatorParameter();
    }

    private void AttackPlayer()
    {
        attackTimer += Time.deltaTime;
        CancelSpeedUp();
        motor.SetBodyMovement(Vector3.zero);
        if (attackTimer > attackCooldown)
        {
            animator.SetTrigger("Attack");
            attackTimer = 0f;
        }
    }

    public void FinishAttack()
    {
        isAttacking = false;
        Debug.Log("Finish Attack");
    }

    private void CancelSpeedUp()
    {
        isSpeedUp = false;
        motor.SpeedUp(false);
        speedUpTimer = 0f;
    }

    private void DetectPlayer()
    {
        Vector3 direction = playerTrans.position - transform.position;
        direction.y = 0;
        motor.SetBodyMovement(direction);
        CheckSpeedUp();
    }

    private void CheckSpeedUp()
    {
        if (isDetected)
        {
            speedUpTimer += Time.deltaTime;
            if (speedUpTimer > speedupDelay)
            {
                motor.SpeedUp(true);
                isSpeedUp = true;
            }
        }
        else
        {
            isDetected = true;
            speedUpTimer = 0f;
        }
    }

    private void UpdateAnimatorParameter()
    {
        animator.SetBool("IsDetected", isDetected);
        animator.SetBool("IsSpeedUp", isSpeedUp);
    }

    public void GetAttack(int damage, Transform bodyPart, Vector3 hitPoint)
    {
        if (isDead) return;

        Vector3 stepBack = hitPoint - transform.position;
        stepBack.y = 0;
        charInfo.CurrentHp -= damage;
        motor.SetBodyFacing(stepBack);
        animator.SetTrigger("Hit");
    }

    private void Dead()
    {
        motor.SetBodyMovement(Vector3.zero);
        isDead = true;
        animator.SetTrigger("Dead");
        StartCoroutine(Recover());
    }

    private IEnumerator Recover()
    {
        yield return new WaitForSeconds(recoveryTime);
        charInfo.Reset();
        animator.SetTrigger("Recover");
        yield return new WaitForSeconds(3f);
        isDead = false;
    }
}
