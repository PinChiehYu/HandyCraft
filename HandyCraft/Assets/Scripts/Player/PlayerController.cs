using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private IInput _input;
    private PlayerMotor _motor;

    private Transform _foot;
    [SerializeField]
    private LayerMask _groundMask;

    private HandController _leftHand;
    private HandController _rightHand;

    private Canvas _optionUICanvas;
    private Canvas _weapondUICanvas;

    private bool _isOpenedOptionUI;
    private bool _isOpenedWeapondUI;

    private void Awake()
    {
        _motor = GetComponent<PlayerMotor>();
        _foot = transform.Find("Foot");
        _leftHand = transform.Find("LeftController").GetComponent<HandController>();
        _rightHand = transform.Find("RightController").GetComponent<HandController>();

        _isOpenedOptionUI = false;
        _isOpenedWeapondUI = false;
    }

    private void Start()
    {
        _input = GameManager.Instance.GetInputSource();
    }

    // Update is called once per frame
    void Update()
    {
        CheckUIOperation();
        if (!_isOpenedOptionUI && !_isOpenedWeapondUI)
        {
            UpdateMovement();
        }
        else
        {
            StopMovement();
            CheckSwitchWeapond();
        }
    }

    private void UpdateMovement()
    {
        _motor.SetBodyMovement(_input.GetMovement());
        _motor.SetBodyRotation(_input.GetBodyRotation());

        if (_input.GetJump() && Physics.CheckBox(_foot.position, new Vector3(0.25f, 0.05f, 0.25f), Quaternion.identity, _groundMask))
        {
            _motor.Jump();
        }
    }

    private void StopMovement()
    {
        _motor.SetBodyMovement(Vector3.zero);
    }

    private void CheckUIOperation()
    {
        Inputs inputs = _input.GetUIOperation();
        if (inputs == Inputs.OpenOptionUI)
        {
            if (_isOpenedWeapondUI)
            {
                _weapondUICanvas.enabled = false;
                _isOpenedWeapondUI = false;
            }

            _isOpenedOptionUI = !_isOpenedOptionUI;
            if (_isOpenedOptionUI)
            {
                //_optionUICanvas.enabled = true;
            }
        }
        else if (inputs == Inputs.OpenWeapondUI)
        {
            if (_isOpenedOptionUI)
            {
                _optionUICanvas.enabled = false;
                _isOpenedOptionUI = false;
            }

            _isOpenedWeapondUI = !_isOpenedWeapondUI;
            if (_isOpenedWeapondUI)
            {
                //_weapondUICanvas.enabled = true;
            }
        }
    }

    float cooldown;
    private void CheckSwitchWeapond()
    {
        cooldown += Time.deltaTime;
        if (_isOpenedWeapondUI && cooldown > 0.5f)
        {
            float way = _input.GetMovement().x;
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
        _rightHand.SwitchWeapond(info.RightPrefab, info.RightLocalPosition, info.RightLocalRotation);
        _leftHand.SwitchWeapond(info.LeftPrefab, info.LeftLocalPosition, info.LeftLocalRotation);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 30, 200, 20), "OptionUI: " + _isOpenedOptionUI.ToString());
        GUI.Label(new Rect(10, 50, 200, 20), "WeapondUI: " + _isOpenedWeapondUI.ToString());
    }
}
