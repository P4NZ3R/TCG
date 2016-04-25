using UnityEngine;

public class CardHandler : ScriptableObject
{
    public struct Require
    {
        public int water;
        public int fire;
        public int wind;
        public int earth;
    }
    public string nome;
    public string description;

    public Require require;
    public float castTime;
    public float fatique;
    public int PP;
    public int damage;
    public int healthRegen;
    public int armor;
    //if armor>0 bonustime active
    public float bonusTime;//per quanto tempo rimane attiva l armatura o altro

}
