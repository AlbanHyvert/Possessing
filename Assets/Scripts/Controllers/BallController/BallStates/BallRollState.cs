using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRollState : IBallStates
{
    private BallBody _self = null;
    private float _currentSpeed = 0;
    private float _accelerationSpeed = 0;
    private float _desallerationSpeed = 0;
    private float _speed = 0;
    private bool _savedOldDir = false;
    private Vector2 _oldDir = Vector2.zero;

    private void IsGrounded(bool value)
    {
        if (value == false)
        {
            _self.ChangeState(E_BallState.INAIR);
        }
    }

    private void Jumping(Vector2 dir)
    {
        _self.GetRigidbody.AddForce((dir + _self.GetDirection) * _self.GetJumpForce);
    }

    void IBallStates.Enter()
    {
        _currentSpeed = 0;

        _self.OnIsGrounded += IsGrounded;
        _self.Jumping += Jumping;

        IsGrounded(_self.GetIsGrounded);
    }

    void IBallStates.Exit()
    {
        _self.OnIsGrounded -= IsGrounded;
        _self.Jumping -= Jumping;

        _currentSpeed = 0;
    }

    void IBallStates.Init(BallBody self)
    {
        _self = self;
        _speed = self.GetSpeed;
        _accelerationSpeed = self.GetAccSpeed;
        _desallerationSpeed = self.GetDesaccSpeed;
    }

    void IBallStates.Tick()
    {
        Vector2 dir = _self.GetDirection;

        if (dir != Vector2.zero)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, _speed, _accelerationSpeed * Time.deltaTime);
        }
        else
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0, _desallerationSpeed * Time.deltaTime);
        }

        if(_currentSpeed - 0.2f <= 0 && dir == Vector2.zero)
        {
            _self.ChangeState(E_BallState.IDLE);
        }

        dir *= _currentSpeed * Time.deltaTime;

        _self.transform.Translate(dir);
    }
}
