using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private IInput input;
    private PlayerMotor motor;

    private Transform foot;
    [SerializeField]
    private LayerMask groundMask;

    private HandController leftHand;
    private HandController rightHand;

    private CharacterInfo charInfo;

    private Canvas optionUICanvas;
    private Canvas weapondUICanvas;

    private bool isOpenedOptionUI;
    private bool isOpenedWeapondUI;

    private float maxStrength;
    private float strength;

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
        input = GameManager.Instance.GetInputSource();
        leftHand = transform.Find("LeftController").GetComponent<HandController>();
        rightHand = transform.Find("RightController").GetComponent<HandController>();
        GameManager.Instance.PPController.BindPlayer(charInfo);
    }

    // Update is called once per frame
    private void Update()
    {
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

        ////for testing////
        /*
        timer += Time.deltaTime;
        if (timer > 1f)
        {
            charInfo.ReceiveDamage(1);
            timer = 0f;
        }
        */
        //////////////////
    }

    private void UpdateMovement()
    {
        motor.SetBodyMovement(input.GetMovement());
        motor.SetBodyRotation(input.GetBodyRotation());

        if (input.GetJump() && Physics.CheckBox(foot.position, new Vector3(0.25f, 0.05f, 0.25f), Quaternion.identity, groundMask))
        {
            motor.Jump();
        }
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
                weapondUICanvas.enabled = false;
                isOpenedWeapondUI = false;
            }

            isOpenedOptionUI = !isOpenedOptionUI;
            if (isOpenedOptionUI)
            {
                //_optionUICanvas.enabled = true;
            }
        }
        else if (inputs == Inputs.OpenWeapondUI)
        {
            if (isOpenedOptionUI)
            {
                optionUICanvas.enabled = false;
                isOpenedOptionUI = false;
            }

            isOpenedWeapondUI = !isOpenedWeapondUI;
            if (isOpenedWeapondUI)
            {
                //_weapondUICanvas.enabled = true;
            }
        }
    }

    float cooldown;
    private void CheckSwitchWeapond()
    {
        cooldown += Time.deltaTime;
        if (isOpenedWeapondUI && cooldown > 0.5f)
        {
            float way = input.GetMovement().x;
            if (Mathf.Abs(way) < 0.5f)
            {
                return;
            }
            cooldown = 0f;

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

    void OnGUI()
    {
        GUI.Label(new Rect(10, 30, 200, 20), "OptionUI: " + isOpenedOptionUI.ToString());
        GUI.Label(new Rect(10, 50, 200, 20), "WeapondUI: " + isOpenedWeapondUI.ToString());
    }
}
