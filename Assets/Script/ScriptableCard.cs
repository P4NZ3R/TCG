using UnityEngine;

[CreateAssetMenu]
public class ScriptableCard : ScriptableObject
{
    public string nome;
    public Sprite image;
    public Type type;
    public Cost[] cost;
    [TextArea]
    public string description;

    public long health;
    public long damage;

    public enum Elements
    {
        R,G,B
    }

    public enum Type
    {
        Pillar,Creature,Spell,Constant
    }

    [System.Serializable]
    public class Cost
    {
        public Elements element;
        public long amount;
    }
}
