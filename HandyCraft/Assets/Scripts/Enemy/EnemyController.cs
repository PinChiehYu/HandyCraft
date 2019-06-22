using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    private EnemySoundManager sound;
    private CharacterInfo charInfo;
    private Animator animator;

    private Transform playerTrans;

    private bool isDetected;
    private bool isSpeedUp;
    public bool isAttacking { get; private set; }
    private bool isDead;

    private float attackTimer;
    private float speedUpTimer;

    private void Awake()
    {
        motor = GetComponent<EnemyMotor>();
        sound = GetComponent<EnemySoundManager>();
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
        if (isDead || GameManager.Instance.FreezeGame) return;

        float distance = Vector3.Distance(playerTrans.position, transform.position);
        if (distance < attackRange)
        {
            AttackPlayer();
        }
        else if (distance < detectRange)
        {
            DetectPlayer();
        }
        else
        {
            isDetected = false;
            CancelMoving();
        }

        UpdateAnimatorParameter();
    }

    private void CancelMoving()
    {
        isSpeedUp = false;
        motor.SpeedUp(false);
        motor.SetBodyMovement(Vector3.zero);
        speedUpTimer = 0f;
    }

    private void AttackPlayer()
    {
        isDetected = false;
        CancelMoving();
        if (!isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackCooldown)
            {
                isAttacking = true;
                animator.SetTrigger("Attack");
                sound.Attack();
                attackTimer = 0f;
            }
        }
    }

    public void FinishAttack()
    {
        isAttacking = false;
    }

    private void DetectPlayer()
    {
        isAttacking = false;
        Vector3 direction = playerTrans.position - transform.position;
        direction.y = 0;
        if (Physics.Raycast(transform.position + Vector3.up * 1.3f, direction, out RaycastHit hitInfo, direction.magnitude, LayerMask.GetMask("Obstacle")))
        {
            isDetected = false;
            CancelMoving();
        }
        else
        {
            CheckSpeedUp();
            motor.SetBodyMovement(direction);
        }
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

        CancelMoving();
        Vector3 attackDirection = hitPoint - transform.position;
        attackDirection.y = 0;
        charInfo.CurrentHp -= damage;
        motor.SetBodyFacing(attackDirection);
        animator.ResetTrigger("Attack");
        animator.SetTrigger("Hit");
    }

    private void Dead()
    {
        CancelMoving();
        isDetected = false;
        isAttacking = false;
        isDead = true;
        animator.SetTrigger("Dead");
        StartCoroutine(Recover());
    }

    private IEnumerator Recover()
    {
        yield return new WaitForSeconds(recoveryTime);
        charInfo.Reset();
        animator.SetTrigger("Recover");
        yield return new WaitForSeconds(5f);
        isDead = false;
    }
}
