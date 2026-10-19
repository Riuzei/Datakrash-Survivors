public class PauseState : GameState
{
    public PauseState() : base(EGameState.Pause){}
    public override void EnterState(EGameState previousState = default)
    {
        base.EnterState(previousState);
        GameplayManager.Instance.pauseMenuObj.SetActive(true);
        GameplayManager.Instance.IsTicking = false;
    }

}
