using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour, IAttackable
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

    private EnemyMotor motor;
    private CharacterInfo charInfo;
    private Animator animator;

    private Transform playerTrans;

    private bool isDetected;
    private bool isSpeedUp;
    private bool isAttacking;
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
        if (isAttacking || isDead) return;

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
            isAttacking = false;
            CancelSpeedUp();
            motor.SetBodyMovement(Vector3.zero);
        }

        UpdateAnimatorParameter();
    }

    private void AttackPlayer()
    {
        attackTimer += Time.deltaTime;
        isAttacking = true;
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
        Vector3 stepBack = transform.position - hitPoint;
        stepBack.y = 0;
        stepBack.Normalize();
        charInfo.CurrentHp -= damage;
        animator.SetTrigger("Hit");
    }

    private void Dead()
    {
        StartCoroutine(DeadProcess());
    }

    private IEnumerator DeadProcess()
    {
        isDead = true;
        motor.SetBodyMovement(Vector3.zero);
        yield return new WaitForSeconds(1.5f);
    }
}
