using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanInAirState : IHumanoidState
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
        if(value == true)
        {
            _self.ChangeState(E_HumanoidStates.IDLE);
        }
    }

    void IHumanoidState.Enter()
    {
        _self.UpdateStatus += CheckStatus;
        _self.OnIsGrounded += IsGrounded;

        CheckStatus(_self.GetStatus);
        IsGrounded(_self.GetIsGrounded);
    }

    void IHumanoidState.Exit()
    {
        _self.UpdateStatus -= CheckStatus;
        _self.OnIsGrounded -= IsGrounded;
    }

    void IHumanoidState.Init(HumanoidBody self)
    {
        _self = self;
    }

    void IHumanoidState.Tick()
    {
        
    }
}
