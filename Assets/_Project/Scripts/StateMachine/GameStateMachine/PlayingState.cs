public class PlayingState : GameState
{
    public PlayingState() : base(EGameState.Playing) {}

    public override void EnterState(EGameState previousState = default)
    {
        base.EnterState(previousState);
        GameplayManager.Instance.pauseMenuObj.SetActive(false);
        GameplayManager.Instance.IsTicking = true;
    }
}
