using UnityEngine;

public class HumanIdleState : IHumanoidState
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

        if(_self.GetAnimator != null)
        {
            _self.GetAnimator.SetBool("isJumping", true);
        }
    }

    void IHumanoidState.Enter()
    {
        _self.UpdateStatus += CheckStatus;
        _self.OnIsGrounded += IsGrounded;
        _self.Jumping += Jumping;

        if (_self.GetAnimator != null)
        {
            _self.GetAnimator.SetFloat("xSpeed", 0);
            _self.GetAnimator.SetBool("isJumping", false);
        }

        CheckStatus(_self.GetStatus);

        IsGrounded(_self.GetIsGrounded);
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
        if (_self.GetDirection != Vector2.zero && _self.GetIsSprinting == false)
        {
            _self.ChangeState(E_HumanoidStates.WALK);
        }
        else if (_self.GetDirection != Vector2.zero && _self.GetIsSprinting == true)
        {
            _self.ChangeState(E_HumanoidStates.SPRINT);
        }
    }
}
