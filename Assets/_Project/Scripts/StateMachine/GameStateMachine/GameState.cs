public abstract class GameState : BaseState<EGameState>
{
    protected GameState(EGameState key) : base(key) {}
}
public enum EGameState 
{ 
    Playing, 
    Pause, 
    Over, 
    Upgrade 
}