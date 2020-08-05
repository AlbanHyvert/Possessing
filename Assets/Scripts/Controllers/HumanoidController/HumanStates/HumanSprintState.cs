using UnityEngine;

public class HumanSprintState : IHumanoidState
{
    private HumanoidBody _self = null;

    private void CheckStatus(E_BodyStatus status)
    {
        switch (status)
        {
            case E_BodyStatus.POSSESSED:
                //Idle Player Anim
                break;
            case E_BodyStatus.FREE:
                //Idle Npc Anim
                break;
            default:
                break;
        }
    }

    private void IsGrounded(bool value)
    {
        if(value == false)
        {
            _self.ChangeState(E_HumanoidStates.INAIR);
        }
    }

    private void Jumping(Vector2 dir)
    {
        _self.GetRigidbody.AddForce((dir + _self.GetDirection) * _self.JumpForce);
    }

    void IHumanoidState.Enter()
    {
        _self.UpdateStatus += CheckStatus;
        _self.OnIsGrounded += IsGrounded;
        _self.Jumping += Jumping;
        CheckStatus(_self.GetStatus);
    }

    void IHumanoidState.Exit()
    {
        _self.UpdateStatus -= CheckStatus;
        _self.OnIsGrounded -= IsGrounded;
        _self.Jumping -= Jumping;
    }

    void IHumanoidState.Init(HumanoidBody self)
    {
        _self = self;
    }

    void IHumanoidState.Tick()
    {
        Movement();
    }

    private void Movement()
    {
        Vector2 dir = _self.GetDirection;

        if (dir == Vector2.zero)
        {
            _self.ChangeState(E_HumanoidStates.IDLE);
        }
        else if (_self.GetIsSprinting == false)
        {
            _self.ChangeState(E_HumanoidStates.WALK);
        }

        dir *= _self.MovingSpeed * Time.deltaTime;

        _self.transform.Translate(dir);
    }
}
