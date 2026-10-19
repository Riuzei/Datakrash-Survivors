public class PassiveItem : IItem
{
    public ModifierInfo Modifier { get; }
    public int CurrentLevel { get; private set; }

    public PassiveItem(ModifierInfo modifier, int level = 1)
    {
        Modifier = modifier;
        CurrentLevel = level;
    }

    public void LevelUp() => CurrentLevel++;
}