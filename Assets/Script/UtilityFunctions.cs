using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityFunctions {

    public static ScriptableCard[] ShuffleDeck(ScriptableCard[] deck,int cardsLeft)
    {
        for (int i = cardsLeft - 1; i > 0; i--) 
        {
            int r = Random.Range(0,i);
            ScriptableCard tmp = deck[i];
            deck[i] = deck[r];
            deck[r] = tmp;
        }
        return deck;
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
