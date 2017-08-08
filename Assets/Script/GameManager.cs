using System.Collections;
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
        currentPhase = Phase.Main;
        PlayerHandler.singletonPlayer.canSummon = true;
        InitGame();
    }

    void NextPhase()
    {
        if (currentPhase == Phase.OpEndPhase)
            currentPhase = 0;
        else
            currentPhase = (Phase)((int)currentPhase + 1);

        if (events[(int)currentPhase] != null)
            events[(int)currentPhase](currentPhase);

        if (currentPhase != Phase.Main && currentPhase != Phase.OpMain)
            RequestNextPhase();
        else if (currentPhase == Phase.Main)
            PlayerHandler.singletonPlayer.canSummon = true;
        else if (currentPhase == Phase.OpMain)
            PlayerHandler.singletonOpponent.canSummon = true;
    }

    public void RequestNextPhase()
    {
        StartCoroutine(WaitForNextPhase());
    }

    IEnumerator WaitForNextPhase()
    {
        yield return new WaitForSeconds(0.8f);
        NextPhase();
    }

    //prephase
    void InitGame()
    {
        PlayerHandler.singletonPlayer.deck = UtilityFunctions.ShuffleDeck(PlayerHandler.singletonPlayer.deck,PlayerHandler.singletonPlayer.deck.Length);
        PlayerHandler.singletonOpponent.deck = UtilityFunctions.ShuffleDeck(PlayerHandler.singletonOpponent.deck,PlayerHandler.singletonOpponent.deck.Length);
        for (int i = 0; i <= 5; i++)
        {
            events[8](Phase.Draw);//Draw
            events[9](Phase.OpDraw);//OpDraw
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
        events[9](Phase.Draw);//OpDraw
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
        ScriptableCard topCard = UtilityFunctions.GetTopCardOfDeck(PlayerHandler.singletonPlayer.deck, PlayerHandler.singletonPlayer.cardsLeft);
        if (topCard)
        {
            PlayerHandler.singletonPlayer.AddCardInHand(topCard);
            PlayerHandler.singletonPlayer.cardsLeft--;
        }
        else
        {
            Debug.LogError("empty deck");
        }
    }

    void OpDraw(Phase currPhase,CardHandler card=null)
    {
        ScriptableCard topCard = UtilityFunctions.GetTopCardOfDeck(PlayerHandler.singletonOpponent.deck, PlayerHandler.singletonOpponent.cardsLeft);
        if (topCard)
        {
            PlayerHandler.singletonOpponent.AddCardInHand(topCard);
            PlayerHandler.singletonOpponent.cardsLeft--;
        }
        else
        {
            Debug.LogError("Opponent empty deck");
        }
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
