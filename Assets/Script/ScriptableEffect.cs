using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableEffect : ScriptableObject {

    public enum Effects
    {
        Draw,ChangePower,Charge,Trample,Rampage,ChangeHealth,Discard
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
            case Effects.Trample:
                Debug.LogError(effect.ToString() + " is a passive effect");
                break;
            case Effects.Rampage:
                Debug.LogError(effect.ToString() + " is a passive effect");
                break;
            case Effects.ChangeHealth:
                ChangeHealth(card,value);
                break;
            case Effects.Discard:
                Discard(card);
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

    void ChangeHealth(CardHandler card,int value)
    {
        if (card.playerOwner)
            PlayerHandler.singletonPlayer.HealthLeft += value;
        else
            PlayerHandler.singletonOpponent.HealthLeft += value;
    }

    void Discard(CardHandler card)
    {
        for (int i = 0; i < value; i++)
        {
            if (card.playerOwner)
                PlayerHandler.singletonPlayer.DiscardCardinHand();
            else
                PlayerHandler.singletonOpponent.DiscardCardinHand();
        }
    }

}
