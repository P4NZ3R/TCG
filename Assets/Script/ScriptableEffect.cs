using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableEffect : ScriptableObject {

    public enum Effects
    {
        Draw,ChangePower
    }
    public Effects effect;
    public int value;

    //
    public void Activate(CardHandler card)
    {
        switch (effect)
        {
            case Effects.Draw:
                Draw(card,value);
                break;
            case Effects.ChangePower:
                ChangePower(value,card);
                break;
            default:
                Debug.LogError("no effect founded");
                break;
        }
    }
    //
    void Draw(CardHandler card,int value)
    {
        if(card.playerOwner)
            GameManager.singleton.Draw(GameManager.Phase.Draw);
        else
            GameManager.singleton.OpDraw(GameManager.Phase.OpDraw);

    }

    void ChangePower(int value,CardHandler card)
    {
        card.ChangePower(value);
    }
}
