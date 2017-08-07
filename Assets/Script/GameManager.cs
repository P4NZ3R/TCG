using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager singleton;
    //delegates
//    public delegate void GenericEvent();
    public delegate void TestEvent(CardHandler card=null);
    //events
    public event TestEvent OnUpkeep;
    public event TestEvent OnMain;
    public event TestEvent OnBattle;
    public event TestEvent OnEndPhase;
    public event TestEvent OnOpUpkeep;
    public event TestEvent OnOpMain;
    public event TestEvent OnOpBattle;
    public event TestEvent OnOpEndPhase;
                             
    public event TestEvent OnDraw;
    public event TestEvent OnOpDraw;
    public event TestEvent OnSummonPerm;
    public event TestEvent OnOpSummonPerm;
    public event TestEvent OnDestroyPerm;
    public event TestEvent OnOpDestroyPerm;
    //enum
    public enum Phase{Upkeep,Main,Battle,EndPhase,OpUpkeep,OpMain,OpBattle,OpEndPhase,Summon}
    //setter

    //variables
    [SerializeField]
    PlayerHandler player;
    Phase currentPhase;
    public TestEvent[] events = new TestEvent[8];

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

        OnDraw += Draw;
        OnOpDraw += OpDraw;
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
            events[(int)currentPhase]();
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

    void Upkeep(CardHandler card=null)
    {
        Debug.Log("UpKeep!");
        OnDraw();
    }
    void Main1(CardHandler card=null)
    {
        Debug.Log("Main!");
    }
    void Battle(CardHandler card=null)
    {
        Debug.Log("Battle!");
    }
    void EndPhase(CardHandler card=null)
    {
        Debug.Log("EndPhase!");
    }

    void OpUpkeep(CardHandler card=null)
    {
        Debug.Log("OpUpkeep!");
    }
    void OpMain1(CardHandler card=null)
    {
        Debug.Log("OpMain!");
    }
    void OpBattle(CardHandler card=null)
    {
        Debug.Log("Opbattle!");
    }
    void OpEndPhase(CardHandler card=null)
    {
        Debug.Log("OpEndPhase!");
    }

    //functions
    public void Draw(CardHandler card=null)
    {
        ScriptableCard topCard = UtilityFunctions.GetTopCardOfDeck(player.deck, player.cardsLeft);
        if (topCard)
        {
            player.AddCardInHand(topCard);
            player.cardsLeft--;
        }
    }

    public void OpDraw(CardHandler card=null)
    {
        
    }

    public void SummonPerm(CardHandler card=null)
    {
        if(OnSummonPerm!=null)
            OnSummonPerm();
    }

    public void OpSummonPerm(CardHandler card=null)
    {
        if(OnOpSummonPerm!=null)
            OnOpSummonPerm();
    }

    public void DestroyPerm(CardHandler card=null)
    {
        if(OnDestroyPerm!=null)
            OnDestroyPerm();
    }

    public void OpDestroyPerm(CardHandler card=null)
    {
        if(OnOpDestroyPerm!=null)
            OnOpDestroyPerm();
    }

}
