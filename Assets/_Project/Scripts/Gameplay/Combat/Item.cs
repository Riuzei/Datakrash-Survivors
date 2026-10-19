public interface IItem
{
    ModifierInfo Modifier { get; }
    int CurrentLevel { get; }
    void LevelUp();

}