using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInAirState : IBallStates
{
    private BallBody _self = null;

    private void IsGrounded(bool value)
    {
        if (value == true)
        {
            _self.ChangeState(E_BallState.IDLE);
        }
    }

    public void Enter()
    {
        _self.OnIsGrounded += IsGrounded;
    }

    public void Exit()
    {
        _self.OnIsGrounded -= IsGrounded;
    }

    public void Init(BallBody self)
    {
        _self = self;
    }

    public void Tick()
    {

    }
}
