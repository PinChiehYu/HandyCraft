using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private ViveInput input;
    private PlayerMotor motor;

    private Transform foot;
    [SerializeField]
    private LayerMask groundMask;

    private HandController leftHand;
    private HandController rightHand;

    private CharacterInfo charInfo;

    [SerializeField]
    private GameObject optionUICanvas;
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private GameObject weapondUICanvas;
    [SerializeField]
    private GameObject UIPointer;

    private bool isOpenedOptionUI;
    private bool isOpenedWeapondUI;

    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();
        charInfo = GetComponent<CharacterInfo>();
        foot = transform.Find("Foot");

        isOpenedOptionUI = false;
        isOpenedWeapondUI = false;
    }

    private void Start()
    {
        input = GameManager.Instance.GetInputSource() as ViveInput;
        leftHand = transform.Find("LeftController").GetComponent<HandController>();
        rightHand = transform.Find("RightController").GetComponent<HandController>();
        GameManager.Instance.PPController.BindPlayer(charInfo);
        GameManager.Instance.OnWin += Win;
        charInfo.OnDie += Dead;
    }

    private void Update()
    {
        if (GameManager.Instance.FreezeGame) return;

        CheckUIOperation();
        if (!isOpenedOptionUI && !isOpenedWeapondUI)
        {
            UpdateMovement();
        }
        else
        {
            StopMovement();
            CheckSwitchWeapond();
        }

        invincibleTimer += Time.deltaTime;
    }

    private void UpdateMovement()
    {
        motor.SetBodyMovement(input.GetMovement());
        motor.SetBodyRotation(input.GetBodyRotation());

        TriggerSpeedUp();
    }

    private void TriggerSpeedUp()
    {
        if (input.GetTriggerSpeedUp())
        {
            charInfo.CurrentEnergy -= Time.deltaTime;
            if (charInfo.CurrentEnergy > 0)
            {
                motor.SpeedUp(true);
                return;
            }
        }

        motor.SpeedUp(false);
    }

    private void StopMovement()
    {
        motor.SetBodyMovement(Vector3.zero);
    }

    private void CheckUIOperation()
    {
        Inputs inputs = input.GetUIOperation();
        if (inputs == Inputs.OpenOptionUI)
        {
            if (isOpenedWeapondUI)
            {
                weapondUICanvas.SetActive(false);
                isOpenedWeapondUI = false;
            }

            isOpenedOptionUI = !isOpenedOptionUI;
            optionUICanvas.SetActive(isOpenedOptionUI);
            UIPointer.SetActive(isOpenedOptionUI);
        }
        else if (inputs == Inputs.OpenWeapondUI)
        {
            if (isOpenedOptionUI)
            {
                optionUICanvas.SetActive(false);
                isOpenedOptionUI = false;
                UIPointer.SetActive(false);
            }

            isOpenedWeapondUI = !isOpenedWeapondUI;
            weapondUICanvas.SetActive(isOpenedWeapondUI);
        }
    }

    private float switchWeapondTimer;
    private void CheckSwitchWeapond()
    {
        switchWeapondTimer += Time.deltaTime;
        if (isOpenedWeapondUI && switchWeapondTimer > 0.5f)
        {
            float way = input.GetSwitchDirection();
            if (Mathf.Abs(way) < 0.5f)
            {
                return;
            }
            switchWeapondTimer = 0f;

            WeapondInfo info;
            if (way > 0f)
            {
                info = GameManager.Instance.WeapondManager.SwitchToNextWeapond();
            }
            else
            {
                info = GameManager.Instance.WeapondManager.SwitchToPreviosWeapdon();
            }

            SwitchWeapond(info);
        }
    }

    private void SwitchWeapond(WeapondInfo info)
    {
        rightHand.SwitchWeapond(info.RightPrefab, info.RightLocalPosition, info.RightLocalRotation);
        leftHand.SwitchWeapond(info.LeftPrefab, info.LeftLocalPosition, info.LeftLocalRotation);
    }

    public void GetAttack()
    {
        charInfo.CurrentHp -= 10;
    }

    private float invincibleTimer;
    public void OnCollisionEnter(Collision collision)
    {
        if (invincibleTimer > 1.5f && collision.transform.root.CompareTag("Enemy")
            && collision.gameObject.layer != LayerMask.GetMask("Body") && collision.transform.root.GetComponent<EnemyController>().isAttacking)
        {
            GetAttack();
            invincibleTimer = 0f;
        }
    }

    private void Win()
    {
        motor.SetBodyMovement(Vector3.zero);
        weapondUICanvas.SetActive(false);
        optionUICanvas.SetActive(true);
        text.text = "Win!";
        UIPointer.SetActive(true);
    }

    private void Dead()
    {
        GameManager.Instance.Lose();
        motor.SetBodyMovement(Vector3.zero);
        weapondUICanvas.SetActive(false);
        optionUICanvas.SetActive(true);
        text.text = "Died!";
        rightHand.SwitchWeapond(null, Vector3.zero, Vector3.zero);
        UIPointer.SetActive(true);
    }

    /*
    void OnGUI()
    {
        GUI.Label(new Rect(10, 30, 200, 20), "OptionUI: " + isOpenedOptionUI.ToString());
        GUI.Label(new Rect(10, 50, 200, 20), "WeapondUI: " + isOpenedWeapondUI.ToString());
    }
    */
}
