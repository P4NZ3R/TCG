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

    public long power;

    public enum Elements
    {
        R,G,B
    }

    public enum Type
    {
        Creature,Spell,Constant
    }

    [System.Serializable]
    public class Cost
    {
        public Elements element;
        public long amount;
    }

    //
    public Effect[] effects;

    [System.Serializable]
    public class Effect
    {
        public GameManager.Phase phase;
        public ScriptableEffect effect;
    }
}
