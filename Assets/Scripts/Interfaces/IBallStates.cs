public interface IBallStates
{
    void Init(BallBody self);
    void Enter();
    void Tick();
    void Exit();
}
