using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private IInput _input;
    private PlayerMotor _motor;

    void Start()
    {
        _input = new KeyboardInput(transform);
        _motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        _motor.SetBodyMovement(_input.GetMovement());
        _motor.SetBodyRotation(_input.GetBodyRotation());
        _motor.SetHeadRotation(_input.GetHeadRotation());
    }

    public void SetInput(IInput input)
    {
        _input = input;
    }
}
