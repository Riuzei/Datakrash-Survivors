public class OverState : GameState
{
    public OverState() : base(EGameState.Over) {}

    public override void EnterState(EGameState previousState = default)
    {
        base.EnterState(previousState);
        GameplayManager.Instance.gameOverObj.SetActive(true);
        GameplayManager.Instance.IsTicking = false;
        AudioManager.Instance.PitchDownMusic(2f, 0.7f);   
    }

}
