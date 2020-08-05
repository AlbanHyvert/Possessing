public interface IHumanoidState
{
    void Init(HumanoidBody self);
    void Enter();
    void Tick();
    void Exit();
}