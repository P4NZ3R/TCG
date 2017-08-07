﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager singleton;
    //delegates
    public delegate void GenericEvent(Phase currPhase,CardHandler card=null);
    //events
    public GenericEvent[] events = new GenericEvent[14];
    //enum
    public enum Phase{Upkeep,Main,Battle,EndPhase,OpUpkeep,OpMain,OpBattle,OpEndPhase,Draw,OpDraw,SummonPerm,OpSummonPerm,DestroyPerm,OpDestroyPerm,SelfSummon,SelfDeath}
    //setter

    //variables
    [SerializeField]
    PlayerHandler player;
    [HideInInspector]
    public Phase currentPhase;

    //functions
    void Awake()
    {
        singleton = this;
        events[0] += Upkeep;
        events[1] += Main1;
        events[2] += Battle;
        events[3] += EndPhase;
        events[4] += OpUpkeep;
        events[5] += OpMain1;
        events[6] += OpBattle;
        events[7] += OpEndPhase;
        events[8] += Draw;
        events[9] += OpDraw;
        events[10] += SummonPerm;
        events[11] += OpSummonPerm;
        events[12] += DestroyPerm;
        events[13] += OpDestroyPerm;
    }

    void Start()
    {
        currentPhase = 0;

        InitGame();
    }

    public void NextPhase()
    {
        if (currentPhase == Phase.OpEndPhase)
            currentPhase = 0;
        else
            currentPhase = (Phase)((int)currentPhase + 1);

        if (events[(int)currentPhase] != null)
            events[(int)currentPhase](currentPhase);
    }

    //prephase
    void InitGame()
    {
        player.deck = UtilityFunctions.ShuffleDeck(player.deck,player.deck.Length);
        for (int i = 0; i < 5; i++)
        {
            events[8](Phase.Draw);//Draw
        }
    }

    //phases

    void Upkeep(Phase currPhase,CardHandler card=null)
    {
        Debug.Log("UpKeep!");
        events[8](Phase.Draw);//Draw
    }
    void Main1(Phase currPhase,CardHandler card=null)
    {
        Debug.Log("Main!");
    }
    void Battle(Phase currPhase,CardHandler card=null)
    {
        Debug.Log("Battle!");
    }
    void EndPhase(Phase currPhase,CardHandler card=null)
    {
        Debug.Log("EndPhase!");
    }

    void OpUpkeep(Phase currPhase,CardHandler card=null)
    {
        Debug.Log("OpUpkeep!");
    }
    void OpMain1(Phase currPhase,CardHandler card=null)
    {
        Debug.Log("OpMain!");
    }
    void OpBattle(Phase currPhase,CardHandler card=null)
    {
        Debug.Log("Opbattle!");
    }
    void OpEndPhase(Phase currPhase,CardHandler card=null)
    {
        Debug.Log("OpEndPhase!");
    }

    //functions
    public void Draw(Phase currPhase,CardHandler card=null)
    {
        ScriptableCard topCard = UtilityFunctions.GetTopCardOfDeck(player.deck, player.cardsLeft);
        if (topCard)
        {
            player.AddCardInHand(topCard);
            player.cardsLeft--;
        }
        else
        {
            Debug.LogError("empty deck");
        }
    }

    void OpDraw(Phase currPhase,CardHandler card=null)
    {
        
    }

    void SummonPerm(Phase currPhase,CardHandler card=null)
    {
        Debug.Log("summoned Perm");
    }

    void OpSummonPerm(Phase currPhase,CardHandler card=null)
    {
        
    }

    void DestroyPerm(Phase currPhase,CardHandler card=null)
    {
        Debug.Log("Destroyed perm");
    }

    void OpDestroyPerm(Phase currPhase,CardHandler card=null)
    {
        
    }

}
