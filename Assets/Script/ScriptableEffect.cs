using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableEffect : ScriptableObject {

    public enum Effects
    {
        Draw,ChangePower,Charge
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
                ChangePower(card,value);
                break;
            case Effects.Charge:
                Charge(card);
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

    void ChangePower(CardHandler card,int value)
    {
        card.ChangePower(value);
    }

    void Charge(CardHandler card)
    {
        card.transform.SetAsLastSibling();
        if (card.playerOwner)
        {
            PlayerHandler.singletonPlayer.creatures.Remove(card);
            PlayerHandler.singletonPlayer.creatures.Insert(0, card);
        }
        else
        {
            PlayerHandler.singletonOpponent.creatures.Remove(card);
            PlayerHandler.singletonOpponent.creatures.Insert(0, card);
        }
    }
}
