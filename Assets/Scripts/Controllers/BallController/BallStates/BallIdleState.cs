using UnityEngine;

public class BallIdleState : IBallStates
{
    private BallBody _self = null;

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
        _self.OnIsGrounded += IsGrounded;
        _self.Jumping += Jumping;

        IsGrounded(_self.GetIsGrounded);
    }

    void IBallStates.Exit()
    {
        _self.OnIsGrounded -= IsGrounded;
        _self.Jumping -= Jumping;
    }

    void IBallStates.Init(BallBody self)
    {
        _self = self;
    }

    void IBallStates.Tick()
    {
        if (_self.GetDirection != Vector2.zero)
        {
            _self.ChangeState(E_BallState.ROLL);
        }
    }
}