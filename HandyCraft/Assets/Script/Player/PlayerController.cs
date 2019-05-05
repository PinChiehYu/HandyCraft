using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private IInput _input;
    private PlayerMotor _motor;

    private Transform _foot;
    [SerializeField]
    private LayerMask _groundMask;

    void Start()
    {
        _input = new KeyboardInput(transform);
        _motor = GetComponent<PlayerMotor>();
        _foot = transform.Find("Foot");
    }

    // Update is called once per frame
    void Update()
    {
        _motor.SetBodyMovement(_input.GetMovement());
        _motor.SetBodyRotation(_input.GetBodyRotation());
        _motor.SetHeadRotation(_input.GetHeadRotation());

        if (_input.GetJump() && Physics.CheckBox(_foot.position, new Vector3(0.25f, 0.05f, 0.25f), Quaternion.identity, _groundMask))
        {
            _motor.Jump();
        }
    }

    public void SetInput(IInput input)
    {
        _input = input;
    }
}
