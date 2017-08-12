using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager singleton;
    //delegates
    public delegate void GenericEvent(Phase currPhase,CardHandler card=null);
    //events
    public GenericEvent[] events = new GenericEvent[16];
    //enum
    public enum Phase{Upkeep,Main,Battle,EndPhase,OpUpkeep,OpMain,OpBattle,OpEndPhase,Draw,OpDraw,SummonPerm,OpSummonPerm,DestroyPerm,OpDestroyPerm,Discard,OpDiscard,SelfSummon,SelfDeath,SelfDiscard,Null}
    //setter

    //variables
    [SerializeField]
    Text upkeepText;
    [SerializeField]
    Text mainText;
    [SerializeField]
    Text battleText;
    [SerializeField]
    Text endphaseText;

    [HideInInspector]
    public Phase currentPhase;
    [HideInInspector]
    public bool gameEnded;
    public int turn=1;
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
        events[14] += Discard;
        events[15] += OpDiscard;
    }

    void Start()
    {
        currentPhase = Phase.Upkeep;
        InitGame();
        events[0](Phase.Upkeep, null);
        RequestNextPhase();
    }

    void NextPhase()
    {
        if (gameEnded)
            return;
        if (currentPhase == Phase.OpEndPhase)
        {
            currentPhase = 0;
            turn++;
        }
        else
            currentPhase = (Phase)((int)currentPhase + 1);

        if (events[(int)currentPhase] != null)
            events[(int)currentPhase](currentPhase);

        if (currentPhase != Phase.Main && currentPhase != Phase.OpMain)
            RequestNextPhase();
        else if (currentPhase == Phase.Main)
        {
            if (PlayerHandler.singletonPlayer.hand.Count > 0)
                PlayerHandler.singletonPlayer.canSummon = true;
            else
                RequestNextPhase();
        }
        else if (currentPhase == Phase.OpMain)
        {
            if (PlayerHandler.singletonOpponent.hand.Count > 0)
                PlayerHandler.singletonOpponent.canSummon = true;
            else
                RequestNextPhase();
        }

        //grafica temporanea
        upkeepText.transform.parent.GetComponent<Image>().color = 
        mainText.transform.parent.GetComponent<Image>().color = 
        battleText.transform.parent.GetComponent<Image>().color = 
        endphaseText.transform.parent.GetComponent<Image>().color = Color.white;
                        
        if (currentPhase == Phase.Upkeep || currentPhase == Phase.OpUpkeep)
            upkeepText.transform.parent.GetComponent<Image>().color = Color.gray;
        else if (currentPhase == Phase.Main || currentPhase == Phase.OpMain)
            mainText.transform.parent.GetComponent<Image>().color = Color.gray;
        else if (currentPhase == Phase.Battle || currentPhase == Phase.OpBattle)
            battleText.transform.parent.GetComponent<Image>().color = Color.gray;
        else if (currentPhase == Phase.EndPhase || currentPhase == Phase.OpEndPhase)
            endphaseText.transform.parent.GetComponent<Image>().color = Color.gray;

        if (currentPhase == Phase.Upkeep)
            upkeepText.color = mainText.color = battleText.color = endphaseText.color = Color.blue;
        else if (currentPhase == Phase.OpUpkeep)
            upkeepText.color = mainText.color = battleText.color = endphaseText.color = Color.red;
        //
        if (currentPhase == Phase.Battle || currentPhase == Phase.OpBattle)
            ResolveBattle();
    }

    public void RequestNextPhase()
    {
        StartCoroutine(WaitForNextPhase());
    }

    IEnumerator WaitForNextPhase()
    {
        yield return new WaitForSeconds(PlayerHandler.singletonPlayer.speedUp ? 0.1f : 1f);
        if(currentPhase==Phase.Main || currentPhase==Phase.OpMain)
            yield return new WaitForSeconds(PlayerHandler.singletonPlayer.speedUp ? 0.2f : 2f);
        NextPhase();
    }

    //prephase
    void InitGame()
    {
        //player shuffle deck and add it to deckleft
        foreach (ScriptableCard card in PlayerHandler.singletonPlayer.deck)
        {
            PlayerHandler.singletonPlayer.AddCardInDeck(card);
        }
        //op shuffle deck and add it to deckleft
        foreach (ScriptableCard card in PlayerHandler.singletonOpponent.deck)
        {
            PlayerHandler.singletonOpponent.AddCardInDeck(card);
        }
        //both draw cards
        for (int i = 0; i < 4; i++)
        {
            events[8](Phase.Draw);//Draw
            events[9](Phase.OpDraw);//OpDraw
        }
    }

    //phases

    void Upkeep(Phase currPhase,CardHandler card=null)
    {
        Debug.Log("UpKeep!");
        if (turn > 15)
            PlayerHandler.singletonPlayer.HealthLeft -= turn - 15;
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
        if(turn>15)
            PlayerHandler.singletonOpponent.HealthLeft -= turn - 15;
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
        CardHandler topCard = PlayerHandler.singletonPlayer.deckLeft.Count>0 ? PlayerHandler.singletonPlayer.deckLeft[0] : null;
        if (topCard)
        {
            PlayerHandler.singletonPlayer.RemoveCardFromDeck(topCard);
            PlayerHandler.singletonPlayer.AddCardInHand(topCard);
        }
        else
        {
            Debug.Log("empty deck");
        }
    }

    public void OpDraw(Phase currPhase,CardHandler card=null)
    {
        CardHandler topCard = PlayerHandler.singletonOpponent.deckLeft.Count>0 ? PlayerHandler.singletonOpponent.deckLeft[0] : null;
        if (topCard)
        {
            PlayerHandler.singletonOpponent.RemoveCardFromDeck(topCard);
            PlayerHandler.singletonOpponent.AddCardInHand(topCard);
        }
        else
        {
            Debug.Log("Opponent empty deck");
        }
    }

    void SummonPerm(Phase currPhase,CardHandler card=null)
    {
        Debug.Log("summoned Perm");
    }

    void OpSummonPerm(Phase currPhase,CardHandler card=null)
    {
        Debug.Log("OpSummonPer");
    }

    void DestroyPerm(Phase currPhase,CardHandler card=null)
    {
        Debug.Log("Destroyed perm");
    }

    void OpDestroyPerm(Phase currPhase,CardHandler card=null)
    {
        Debug.Log("OpDestroyed perm");
    }

    void Discard(Phase currPhase,CardHandler card=null)
    {
        Debug.Log("Discard");
    }

    void OpDiscard(Phase currPhase,CardHandler card=null)
    {
        Debug.Log("OpDiscard");
    }

    void ResolveBattle(bool RampageBattle=false)
    {
        if (turn == 1 && currentPhase == Phase.Battle)
            return;

        CardHandler creature = PlayerHandler.singletonPlayer.creatures.Count > 0 ? PlayerHandler.singletonPlayer.creatures[0] : null;
        CardHandler opCreature = PlayerHandler.singletonOpponent.creatures.Count > 0 ? PlayerHandler.singletonOpponent.creatures[0] : null;
        
        if (PlayerHandler.singletonPlayer.creatures.Count > 0 && PlayerHandler.singletonOpponent.creatures.Count > 0)
        {
            int creaturePower = creature.currentPower;
            int opCreaturePower = opCreature.currentPower;
            if (currentPhase == Phase.Battle)
            {
                if (creaturePower >= opCreaturePower)
                {
                    opCreature.ChangePower(-creaturePower);
                    creature.ChangePower(-opCreaturePower);
                    //controllo se ha trample o Rampage
                    foreach (ScriptableCard.Effect _effect in creature.ScriptCard.effects)
                    {
                        if (_effect.effectType == ScriptableCard.EffectsType.Trample)
                        {
                            PlayerHandler.singletonOpponent.HealthLeft -= creaturePower - opCreaturePower;
                            break;
                        }
                        if (_effect.effectType == ScriptableCard.EffectsType.Rampage && !RampageBattle)
                        {
                            ResolveBattle(true);
                            break;
                        }
                    }
                }
            }
            else if (currentPhase == Phase.OpBattle)
            {
                if (opCreaturePower >= creaturePower)
                {
                    opCreature.ChangePower(-creaturePower);
                    creature.ChangePower(-opCreaturePower);
                    //controllo se ha trample o Rampage
                    foreach (ScriptableCard.Effect _effect in opCreature.ScriptCard.effects)
                    {
                        if (_effect.effectType == ScriptableCard.EffectsType.Trample)
                        {
                            PlayerHandler.singletonPlayer.HealthLeft -= opCreaturePower - creaturePower;
                            break;
                        }
                        if (_effect.effectType == ScriptableCard.EffectsType.Rampage && !RampageBattle)
                        {
                            ResolveBattle(true);
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            if (currentPhase == Phase.Battle && PlayerHandler.singletonPlayer.creatures.Count > 0)
            {
                PlayerHandler.singletonOpponent.HealthLeft -= creature.currentPower;
            }
            else if (currentPhase == Phase.OpBattle && PlayerHandler.singletonOpponent.creatures.Count > 0)
            {
                PlayerHandler.singletonPlayer.HealthLeft -= opCreature.currentPower;
            }
        }
    }
}
