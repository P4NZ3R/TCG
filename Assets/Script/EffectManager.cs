using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {
    public static EffectManager singleton;

    //
    public void Awake()
    {
        singleton = this;
    }

    public void Activate(CardHandler card,ScriptableCard.Effect _effect)
    {
        switch (_effect.effectType)
        {
            case ScriptableCard.EffectsType.Draw:
                Draw(card,_effect.value);
                break;
            case ScriptableCard.EffectsType.DrawOp:
                DrawOp(card,_effect.value);
                break;
            case ScriptableCard.EffectsType.ChangePower:
                ChangePower(card,_effect.value);
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
                ChangeHealth(card,_effect.value);
                break;
            case ScriptableCard.EffectsType.ChangeHealthOp:
                ChangeHealthOp(card,_effect.value);
                break;
            case ScriptableCard.EffectsType.Discard:
                Discard(card,_effect.value);
                break;
            case ScriptableCard.EffectsType.DiscardOp:
                DiscardOp(card,_effect.value);
                break;
            case ScriptableCard.EffectsType.Summon:
                Summon(card,_effect.value,_effect.linkedCard);
                break;
            case ScriptableCard.EffectsType.SummonOp:
                SummonOp(card,_effect.value,_effect.linkedCard);
                break;
            case ScriptableCard.EffectsType.AddCreatureInDeck:
                AddCreatureInDeck(card,_effect.value,_effect.linkedCard);
                break;
            case ScriptableCard.EffectsType.AddCreatureInDeckOp:
                AddCreatureInDeckOp(card,_effect.value,_effect.linkedCard);
                break;
            case ScriptableCard.EffectsType.DestroySelf:
                DestroySelf(card);
                break;
            case ScriptableCard.EffectsType.RevealCardInHand:
                RevealInHand(card);
                break;
            case ScriptableCard.EffectsType.AddCreatureInHand:
                AddCreatureInHand(card,_effect.value,_effect.linkedCard);
                break;
            case ScriptableCard.EffectsType.AddCreatureInHandOp:
                AddCreatureInHandOp(card,_effect.value,_effect.linkedCard);
                break;
            default:
                Debug.LogError("no effect founded");
                break;
        }
        if (GameManager.singleton.debugMode)
            Debug.Log((card.playerOwner?"PG ":"OP ")+card.ScriptCard.name+"->"+_effect.type.ToString()+" "+_effect.effectType.ToString() + ": value="+_effect.value +" linkedCard:"+_effect.linkedCard);
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

    void Discard(CardHandler card,int value)
    {
        for (int i = 0; i < value; i++)
        {
            if (card.playerOwner)
                PlayerHandler.singletonPlayer.DiscardCardinHand();
            else
                PlayerHandler.singletonOpponent.DiscardCardinHand();
        }
    }

    void DiscardOp(CardHandler card,int value)
    {
        for (int i = 0; i < value; i++)
        {
            if (card.playerOwner)
                PlayerHandler.singletonOpponent.DiscardCardinHand();
            else
                PlayerHandler.singletonPlayer.DiscardCardinHand();
        }
    }

    void Summon(CardHandler card,int value,ScriptableCard linkedCard)
    {
        for (int i = 0; i < value; i++)
        {
            CardHandler linkedCardHandler = Instantiate(PlayerHandler.singletonPlayer.prefabCard).GetComponent<CardHandler>();
            linkedCardHandler.SetCard(linkedCard,ScriptableCard.Type.Battlefield ,card.playerOwner,false,false);
            if (card.playerOwner)
                PlayerHandler.singletonPlayer.SummonCreature(linkedCardHandler);
            else
                PlayerHandler.singletonOpponent.SummonCreature(linkedCardHandler);
        }
    }

    void SummonOp(CardHandler card,int value,ScriptableCard linkedCard)
    {
        for (int i = 0; i < value; i++)
        {
            CardHandler linkedCardHandler = Instantiate(PlayerHandler.singletonPlayer.prefabCard).GetComponent<CardHandler>();
            linkedCardHandler.SetCard(linkedCard,ScriptableCard.Type.Battlefield,!card.playerOwner,false,false);
            if (card.playerOwner)
                PlayerHandler.singletonOpponent.SummonCreature(linkedCardHandler);
            else
                PlayerHandler.singletonPlayer.SummonCreature(linkedCardHandler);
        }
    }

    void AddCreatureInDeck(CardHandler card,int value,ScriptableCard linkedCard)
    {
        for (int i = 0; i < value; i++)
        {
            if (card.playerOwner)
                PlayerHandler.singletonPlayer.AddCardInDeck(linkedCard);
            else
                PlayerHandler.singletonOpponent.AddCardInDeck(linkedCard);
        }
    }

    void AddCreatureInDeckOp(CardHandler card,int value,ScriptableCard linkedCard)
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

    void AddCreatureInHand(CardHandler card,int value,ScriptableCard linkedCard)
    {
        GameObject go = Instantiate(PlayerHandler.singletonPlayer.prefabCard);
        go.name = "card: "+card.ScriptCard.nome;
        CardHandler cardHandler = go.GetComponent<CardHandler>();
        for (int i = 0; i < value; i++)
        {
            if (card.playerOwner)
            {
                cardHandler.SetCard(linkedCard,ScriptableCard.Type.Hand,!PlayerHandler.singletonPlayer.isEnemy,PlayerHandler.singletonPlayer.isBot,!PlayerHandler.singletonPlayer.isBot);
                PlayerHandler.singletonPlayer.AddCardInHand(cardHandler);
            }
            else
            {
                cardHandler.SetCard(linkedCard,ScriptableCard.Type.Hand,!PlayerHandler.singletonOpponent.isEnemy,PlayerHandler.singletonOpponent.isBot,!PlayerHandler.singletonOpponent.isBot);
                PlayerHandler.singletonOpponent.AddCardInHand(cardHandler);
            }
        }
    }

    void AddCreatureInHandOp(CardHandler card,int value,ScriptableCard linkedCard)
    {
        GameObject go = Instantiate(PlayerHandler.singletonPlayer.prefabCard);
        go.name = "card: "+card.ScriptCard.nome;
        CardHandler cardHandler = go.GetComponent<CardHandler>();
        for (int i = 0; i < value; i++)
        {
            if (card.playerOwner)
            {
                cardHandler.SetCard(linkedCard,ScriptableCard.Type.Hand,!PlayerHandler.singletonOpponent.isEnemy,PlayerHandler.singletonOpponent.isBot,!PlayerHandler.singletonOpponent.isBot);
                PlayerHandler.singletonOpponent.AddCardInHand(cardHandler);
            }
            else
            {
                cardHandler.SetCard(linkedCard,ScriptableCard.Type.Hand,!PlayerHandler.singletonPlayer.isEnemy, PlayerHandler.singletonPlayer.isBot, !PlayerHandler.singletonPlayer.isBot);
                PlayerHandler.singletonPlayer.AddCardInHand(cardHandler);
            }
        }
    }
}
