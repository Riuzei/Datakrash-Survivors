public sealed class GameStateMachine : BaseStateMachine<EGameState>
{
     public GameStateMachine()
     {
        Initialize();
     }
     
     protected override void Initialize()
     {  
        stateByEnum.Add(EGameState.Playing, new PlayingState());
        stateByEnum.Add(EGameState.Pause, new PauseState());
        stateByEnum.Add(EGameState.Over, new OverState());
        stateByEnum.Add(EGameState.Upgrade, new UpgradeState());
        SetInitialState(EGameState.Playing);
     }
}
