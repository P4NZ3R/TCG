using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityFunctions {

    public static List<CardHandler> ShuffleDeck(CardHandler[] deck)
    {
        for (int i = deck.Length - 1; i > 0; i--) 
        {
            int r = Random.Range(0,i);
            CardHandler tmp = deck[i];
            deck[i] = deck[r];
            deck[r] = tmp;
        }
        List<CardHandler> deckToList = new List<CardHandler>();
        for (int j = 0; j < deck.Length; j++)
        {
            deckToList.Add(deck[j]);
        }
        return deckToList;
    }  

    public static ScriptableCard.Effect SearchEffectPhase(ScriptableCard card,GameManager.Phase phase)
    {
        foreach (ScriptableCard.Effect _effect in card.effects)
        {
            if (_effect.phase == phase)
            {
                return _effect;
            }
        }
        return null;
    }
}
