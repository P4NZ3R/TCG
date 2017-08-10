using UnityEngine;

[CreateAssetMenu]
public class ScriptableCard : ScriptableObject
{
    public bool isCollectable=true;
    public string nome;
    public Sprite image;
    public Type type;
    public Elements cost1;
    public Elements cost2;
    public Elements cost3;
    [TextArea]
    public string description;

    public long power;

    public enum Elements
    {
        Null,R,G,B
    }

    public enum Type
    {
        Creature,Spell,Constant
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
