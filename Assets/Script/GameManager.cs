using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager singleton;
    //delegates
    public delegate void GenericEvent();
    //events
    public event GenericEvent OnUpkeep;
    public event GenericEvent OnMain;
    public event GenericEvent OnBattle;
    public event GenericEvent OnEndPhase;
    public event GenericEvent OnOpUpkeep;
    public event GenericEvent OnOpMain;
    public event GenericEvent OnOpBattle;
    public event GenericEvent OnOpEndPhase;
                             
    public event GenericEvent OnDraw;
    public event GenericEvent OnOpDraw;
    public event GenericEvent OnSummonPerm;
    public event GenericEvent OnOpSummonPerm;
    //enum
    public enum Phase{Upkeep,Main,Battle,EndPhase,OpUpkeep,OpMain,OpBattle,OpEndPhase,Summon}
    //setter
    Phase CurrentPhase
    {
        get{ return currentPhase;}
        set
        { 
            currentPhase = value; 
            if(events[(int)value]!=null)
                events[(int)value]();
        }
    }
    //variables
    [SerializeField]
    PlayerHandler player;
    Phase currentPhase;
    public GenericEvent[] events = new GenericEvent[8];

    //functions
    void Awake()
    {
        singleton = this;
        events[0] += Upkeep;
        events[1] += Main;
        events[2] += Battle;
        events[3] += EndPhase;
        events[4] += OpUpkeep;
        events[5] += OpMain;
        events[6] += OpBattle;
        events[7] += OpEndPhase;

        OnDraw += Draw;
        OnOpDraw += OpDraw;
    }

    void Start()
    {
        CurrentPhase = 0;

        InitGame();
    }

    public void NextPhase()
    {
        if (CurrentPhase == Phase.OpEndPhase)
            CurrentPhase = 0;
        else
            CurrentPhase = (Phase)((int)CurrentPhase + 1);
    }

    //prephase
    void InitGame()
    {
        player.deck = UtilityFunctions.ShuffleDeck(player.deck,player.deck.Length);
        for (int i = 0; i < 5; i++)
        {
            OnDraw();
        }
    }

    //phases
    void Upkeep()
    {
        Debug.Log("UpKeep!");
        OnDraw();
        OnDraw();
    }
    void Main()
    {
        Debug.Log("Main!");
    }
    void Battle()
    {
        Debug.Log("Battle!");
    }
    void EndPhase()
    {
        Debug.Log("EndPhase!");
    }

    void OpUpkeep()
    {
        Debug.Log("OpUpkeep!");
    }
    void OpMain()
    {
        Debug.Log("OpMain!");
    }
    void OpBattle()
    {
        Debug.Log("Opbattle!");
    }
    void OpEndPhase()
    {
        Debug.Log("OpEndPhase!");
    }

    //functions
    void Draw()
    {
        ScriptableCard topCard = UtilityFunctions.GetTopCardOfDeck(player.deck, player.cardsLeft);
        if (topCard)
        {
            player.AddCardInHand(topCard);
            player.cardsLeft--;
        }
    }

    void OpDraw()
    {
        
    }

    public void SummonPerm()
    {
        OnSummonPerm();
    }

    public void OpSummonPerm()
    {
        OnOpSummonPerm();
    }

}
