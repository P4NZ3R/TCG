using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableEffect : ScriptableObject {

    public enum Effects
    {
        Draw,SelfDamage
    }
    public Effects effect;
    public int value;

    //
    public void Activate(CardHandler card)
    {
        switch (effect)
        {
            case Effects.Draw:
                Draw(value);
                break;
            case Effects.SelfDamage:
                SelfDamage(value,card);
                break;
            default:
                Debug.LogError("no effect founded");
                break;
        }
    }
    //
    void Draw(int value)
    {
        //TODO controllo se e il giocatore o l oppo a usare l effetto
        GameManager.singleton.Draw();
    }

    void SelfDamage(int value,CardHandler card)
    {
        card.Damage(value);
        Debug.LogError("selfDamage");
    }
}
