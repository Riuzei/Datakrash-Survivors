using UnityEngine;

public class UpgradeState : GameState
{
    public UpgradeState() : base(EGameState.Upgrade) {}

    public override void EnterState(EGameState previousState = default)
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.levelUp);
        base.EnterState(previousState);
        ProgressionManager.Instance.upgradeMenuObj.SetActive(true);
        GameplayManager.Instance.IsTicking = false;
    }
    public override void ExitState()
    {
        ProgressionManager.Instance.upgradeMenuObj.SetActive(false);
        GameplayManager.Instance.IsTicking = true;
        Debug.Log("Exiting Upgrade State");
    }
}
