using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableEffect : ScriptableObject {

    public enum Effects
    {
        Draw,ChangePower,Charge,Trample,Rampage,ChangeHealth,ChangeHealthOp,Discard,Summon,SummonOp,DrawOp,AddCreatureInDeck,AddCreatureInDeckOp,DestroySelf
    }
    public Effects effect;
    public int value;
    public ScriptableCard linkedCard;

    //
    public void Activate(CardHandler card)
    {
        switch (effect)
        {
            case Effects.Draw:
                Draw(card,value);
                break;
            case Effects.DrawOp:
                DrawOp(card,value);
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
            case Effects.ChangeHealthOp:
                ChangeHealthOp(card,value);
                break;
            case Effects.Discard:
                Discard(card);
                break;
            case Effects.Summon:
                Summon(card);
                break;
            case Effects.SummonOp:
                SummonOp(card);
                break;
            case Effects.AddCreatureInDeck:
                AddCreatureOnDeck(card);
                break;
            case Effects.AddCreatureInDeckOp:
                AddCreatureOnDeckOp(card);
                break;
            case Effects.DestroySelf:
                DestroySelf(card);
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

    void DrawOp(CardHandler card,int value)
    {
        if(card.playerOwner)
            GameManager.singleton.OpDraw(GameManager.Phase.Draw);
        else
            GameManager.singleton.Draw(GameManager.Phase.OpDraw);

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

    void ChangeHealthOp(CardHandler card,int value)
    {
        if (card.playerOwner)
            PlayerHandler.singletonOpponent.HealthLeft += value;
        else
            PlayerHandler.singletonPlayer.HealthLeft += value;
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

    void Summon(CardHandler card)
    {
        CardHandler linkedCardHandler = Instantiate(PlayerHandler.singletonPlayer.prefabCard).GetComponent<CardHandler>();
        linkedCardHandler.SetCard(linkedCard,card.playerOwner,false,false);
        if (card.playerOwner)
            PlayerHandler.singletonPlayer.SummonCreature(linkedCardHandler);
        else
            PlayerHandler.singletonOpponent.SummonCreature(linkedCardHandler);
    }

    void SummonOp(CardHandler card)
    {
        CardHandler linkedCardHandler = Instantiate(PlayerHandler.singletonPlayer.prefabCard).GetComponent<CardHandler>();
        linkedCardHandler.SetCard(linkedCard,!card.playerOwner,false,false);
        if (card.playerOwner)
            PlayerHandler.singletonOpponent.SummonCreature(linkedCardHandler);
        else
            PlayerHandler.singletonPlayer.SummonCreature(linkedCardHandler);
    }

    void AddCreatureOnDeck(CardHandler card)
    {
        if (card.playerOwner)
            PlayerHandler.singletonPlayer.AddCardInDeck(linkedCard);
        else
            PlayerHandler.singletonOpponent.AddCardInDeck(linkedCard);
    }

    void AddCreatureOnDeckOp(CardHandler card)
    {
        if (card.playerOwner)
            PlayerHandler.singletonOpponent.AddCardInDeck(linkedCard);
        else
            PlayerHandler.singletonPlayer.AddCardInDeck(linkedCard);
    }

    void DestroySelf(CardHandler card)
    {
        if (card.playerOwner)
            PlayerHandler.singletonPlayer.DestroyCreature(card);
        else
            PlayerHandler.singletonOpponent.DestroyCreature(card);
    }
}
