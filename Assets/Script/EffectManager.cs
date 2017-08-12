using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {
    public static EffectManager singleton;

    int value=1;
    ScriptableCard linkedCard;

    //
    public void Awake()
    {
        singleton = this;
    }

    public void Activate(CardHandler card,ScriptableCard.Effect _effect)
    {
        this.value = _effect.value;
        this.linkedCard = _effect.linkedCard;
        switch (_effect.effectType)
        {
            case ScriptableCard.EffectsType.Draw:
                Draw(card,value);
                break;
            case ScriptableCard.EffectsType.DrawOp:
                DrawOp(card,value);
                break;
            case ScriptableCard.EffectsType.ChangePower:
                ChangePower(card,value);
                break;
            case ScriptableCard.EffectsType.Charge:
                Charge(card);
                break;
            case ScriptableCard.EffectsType.Trample:
                Debug.LogError(_effect.effectType.ToString() + " is a passive effect");
                break;
            case ScriptableCard.EffectsType.Rampage:
                Debug.LogError(_effect.effectType.ToString() + " is a passive effect");
                break;
            case ScriptableCard.EffectsType.ChangeHealth:
                ChangeHealth(card,value);
                break;
            case ScriptableCard.EffectsType.ChangeHealthOp:
                ChangeHealthOp(card,value);
                break;
            case ScriptableCard.EffectsType.Discard:
                Discard(card);
                break;
            case ScriptableCard.EffectsType.Summon:
                Summon(card);
                break;
            case ScriptableCard.EffectsType.SummonOp:
                SummonOp(card);
                break;
            case ScriptableCard.EffectsType.AddCreatureInDeck:
                AddCreatureInDeck(card);
                break;
            case ScriptableCard.EffectsType.AddCreatureInDeckOp:
                AddCreatureInDeckOp(card);
                break;
            case ScriptableCard.EffectsType.DestroySelf:
                DestroySelf(card);
                break;
            case ScriptableCard.EffectsType.RevealCardInHand:
                RevealInHand(card);
                break;
            case ScriptableCard.EffectsType.AddCreatureInHand:
                AddCreatureInHand(card);
                break;
            case ScriptableCard.EffectsType.AddCreatureInHandOp:
                AddCreatureInHandOp(card);
                break;
            default:
                Debug.LogError("no effect founded");
                break;
        }
    }
    //
    void Draw(CardHandler card,int value)
    {
        for (int i = 0; i < value; i++)
        {
            if(card.playerOwner)
                GameManager.singleton.Draw(GameManager.Phase.Draw);
            else
                GameManager.singleton.OpDraw(GameManager.Phase.OpDraw);
        }

    }

    void DrawOp(CardHandler card,int value)
    {
        for (int i = 0; i < value; i++)
        {
            if(card.playerOwner)
                GameManager.singleton.OpDraw(GameManager.Phase.Draw);
            else
                GameManager.singleton.Draw(GameManager.Phase.OpDraw);
        }

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
        for (int i = 0; i < value; i++)
        {
            CardHandler linkedCardHandler = Instantiate(PlayerHandler.singletonPlayer.prefabCard).GetComponent<CardHandler>();
            linkedCardHandler.SetCard(linkedCard,card.playerOwner,false,false);
            if (card.playerOwner)
                PlayerHandler.singletonPlayer.SummonCreature(linkedCardHandler);
            else
                PlayerHandler.singletonOpponent.SummonCreature(linkedCardHandler);
        }
    }

    void SummonOp(CardHandler card)
    {
        for (int i = 0; i < value; i++)
        {
            CardHandler linkedCardHandler = Instantiate(PlayerHandler.singletonPlayer.prefabCard).GetComponent<CardHandler>();
            linkedCardHandler.SetCard(linkedCard,!card.playerOwner,false,false);
            if (card.playerOwner)
                PlayerHandler.singletonOpponent.SummonCreature(linkedCardHandler);
            else
                PlayerHandler.singletonPlayer.SummonCreature(linkedCardHandler);
        }
    }

    void AddCreatureInDeck(CardHandler card)
    {
        for (int i = 0; i < value; i++)
        {
            if (card.playerOwner)
                PlayerHandler.singletonPlayer.AddCardInDeck(linkedCard);
            else
                PlayerHandler.singletonOpponent.AddCardInDeck(linkedCard);
        }
    }

    void AddCreatureInDeckOp(CardHandler card)
    {
        for (int i = 0; i < value; i++)
        {
            if (card.playerOwner)
                PlayerHandler.singletonOpponent.AddCardInDeck(linkedCard);
            else
                PlayerHandler.singletonPlayer.AddCardInDeck(linkedCard);
        }
    }

    void DestroySelf(CardHandler card)
    {
        if (card.playerOwner)
            PlayerHandler.singletonPlayer.DestroyCreature(card);
        else
            PlayerHandler.singletonOpponent.DestroyCreature(card);
    }

    void RevealInHand(CardHandler card)
    {
        card.SetCover(false);
    }

    void AddCreatureInHand(CardHandler card)
    {
        GameObject go = Instantiate(PlayerHandler.singletonPlayer.prefabCard);
        go.name = "card: "+card.ScriptCard.nome;
        CardHandler cardHandler = go.GetComponent<CardHandler>();
        for (int i = 0; i < value; i++)
        {
            if (card.playerOwner)
            {
                cardHandler.SetCard(linkedCard,!PlayerHandler.singletonPlayer.isEnemy,PlayerHandler.singletonPlayer.isBot,!PlayerHandler.singletonPlayer.isBot);
                PlayerHandler.singletonPlayer.AddCardInHand(cardHandler);
            }
            else
            {
                cardHandler.SetCard(linkedCard,!PlayerHandler.singletonOpponent.isEnemy,PlayerHandler.singletonOpponent.isBot,!PlayerHandler.singletonOpponent.isBot);
                PlayerHandler.singletonOpponent.AddCardInHand(cardHandler);
            }
        }
    }

    void AddCreatureInHandOp(CardHandler card)
    {
        GameObject go = Instantiate(PlayerHandler.singletonPlayer.prefabCard);
        go.name = "card: "+card.ScriptCard.nome;
        CardHandler cardHandler = go.GetComponent<CardHandler>();
        for (int i = 0; i < value; i++)
        {
            if (card.playerOwner)
            {
                cardHandler.SetCard(linkedCard,!PlayerHandler.singletonOpponent.isEnemy,PlayerHandler.singletonOpponent.isBot,!PlayerHandler.singletonOpponent.isBot);
                PlayerHandler.singletonOpponent.AddCardInHand(cardHandler);
            }
            else
            {
                cardHandler.SetCard(linkedCard, !PlayerHandler.singletonPlayer.isEnemy, PlayerHandler.singletonPlayer.isBot, !PlayerHandler.singletonPlayer.isBot);
                PlayerHandler.singletonPlayer.AddCardInHand(cardHandler);
            }
        }
    }
}
