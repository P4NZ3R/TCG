using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
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

    //enum
    public enum Phase{Upkeep,Main,Battle,EndPhase,OpUpkeep,OpMain,OpBattle,OpEndPhase}
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
    Phase currentPhase;
    GenericEvent[] events = new GenericEvent[8];

    //functions
    void Awake()
    {
        events[0] += Upkeep;
        events[1] += Main;
        events[2] += Battle;
        events[3] += EndPhase;
        events[4] += OpUpkeep;
        events[5] += OpMain;
        events[6] += OpBattle;
        events[7] += OpEndPhase;
    }

    void Start()
    {
        CurrentPhase = 0;
    }

    public void NextPhase()
    {
        if (CurrentPhase == Phase.OpEndPhase)
            CurrentPhase = 0;
        else
            CurrentPhase = (Phase)((int)CurrentPhase + 1);
    }

    //phases
    void Upkeep()
    {
        Debug.Log("UpKeep!");
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
}
