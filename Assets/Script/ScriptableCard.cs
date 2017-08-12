using UnityEngine;

[CreateAssetMenu]
public class ScriptableCard : ScriptableObject
{
    public enum EffectsType
    {
        Draw,DrawOp,ChangePower,Charge,Trample,Rampage,ChangeHealth,ChangeHealthOp,Discard,DiscardOp,Summon,SummonOp,AddCreatureInHand,AddCreatureInHandOp,AddCreatureInDeck,AddCreatureInDeckOp,DestroySelf,RevealCardInHand
    }
    public bool isCollectable=true;
    public string nome;
    public Sprite image;
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
        Battlefield,Hand,Deck
    }

    //
    public Effect[] effects;

    [System.Serializable]
    public class Effect
    {
        public GameManager.Phase phase;
        public EffectsType effectType;
        public Type type;
        public int value;
        public ScriptableCard linkedCard;
    }
}
