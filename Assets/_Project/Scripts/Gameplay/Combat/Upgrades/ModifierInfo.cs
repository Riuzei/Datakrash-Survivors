using UnityEngine;

public class ModifierInfo : ScriptableObject
{
    public Sprite icon;
    public Upgrade[] upgrades;
    public virtual int MaxLevel => upgrades.Length + 1;
    public enum Unit { Percent, Flat }
    [System.Serializable]
    public struct Upgrade
    {
        public EUpgradeType type;
        [TextArea] public string description;
        public float value;
        public Unit unit;
    }
}

 

 