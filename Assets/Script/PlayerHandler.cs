using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {
    public static PlayerHandler singletonPlayer;
    public static PlayerHandler singletonOpponent;
    //
    public bool isEnemy;
    [SerializeField]
    GameObject prefabCard;
    [SerializeField]
    GameObject handLayout;
    [SerializeField]
    GameObject creaturesLayout;
    //
    public ScriptableCard[] deck;
    List<CardHandler> hands = new List<CardHandler>();
    List<CardHandler> creatures = new List<CardHandler>();
    public int cardsLeft;
    public bool canSummon=false;

    void Awake()
    {
        if (isEnemy)
            singletonOpponent = this;
        else
            singletonPlayer = this;
        cardsLeft = deck.Length;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (canSummon && isEnemy && GameManager.singleton.currentPhase == GameManager.Phase.OpMain && hands.Count > 0)
        {
            CardHandler _card = hands[0];
            RemoveCardInHand(_card);
            SummonCreature(_card);
            GameManager.singleton.RequestNextPhase();
            canSummon = false;
        }
	}

    public void AddCardInHand(ScriptableCard card)
    {
        GameObject go = Instantiate(prefabCard);
        go.transform.SetParent(handLayout.transform);
        CardHandler cardHandler = go.GetComponent<CardHandler>();
        cardHandler.SetCard(card,!isEnemy,!isEnemy);
        
        hands.Add(cardHandler);
    }

    public void RemoveCardInHand(CardHandler card)
    {
        card.Interactable(false);
        card.SetCover(false);
        hands.Remove(card);
    }

    public void SummonCreature(CardHandler card)
    {
        ScriptableCard ScriptCard = card.ScriptCard;
        creatures.Add(card);
        card.transform.SetParent(creaturesLayout.transform);
        foreach (ScriptableCard.Effect _effect in ScriptCard.effects)
        {
            int phase = (int)_effect.phase;
            //controlli sul proprietario della carta per applicare gli effetti
            if (isEnemy)
            {
                phase=ConvertPhaseOpponentPlayer(phase);
            }
            //
            if (phase <= 13)
            {
                GameManager.singleton.events[phase] += card.ActivateEffect;
            }
            else
            {
                if (_effect.phase == GameManager.Phase.SelfSummon)
                {
                    _effect.effect.Activate(card);
                }
            }
        }
        if(isEnemy)
            GameManager.singleton.events[11](GameManager.Phase.OpSummonPerm);//OpSummonPerm
        else
            GameManager.singleton.events[10](GameManager.Phase.SummonPerm);//SummonPerm

    }

    public void DestroyCreature(CardHandler card)
    {
        creatures.Remove(card);

        foreach (ScriptableCard.Effect _effect in card.ScriptCard.effects)
        {
            int phase = (int)_effect.phase;
            if (phase <= 13)
            {
                GameManager.singleton.events[phase] -= card.ActivateEffect;
            }
            else
            {
                if (_effect.phase == GameManager.Phase.SelfDeath)
                {
                    _effect.effect.Activate(card);
                }
            }
        }
        Destroy(card.gameObject);
        if(isEnemy)
            GameManager.singleton.events[13](GameManager.Phase.OpDestroyPerm);//OpDestroyPerm
        else
            GameManager.singleton.events[12](GameManager.Phase.DestroyPerm);//DestroyPerm

    }

    public int ConvertPhaseOpponentPlayer(int phase)
    {
        if (isEnemy)
        {
            if (phase <= 3)
                phase += 4;
            else if (phase <= 7)
                phase -= 4;
            else if (phase == 8)
                phase = 9;
            else if (phase == 9)
                phase = 8;
            else if (phase == 10)
                phase = 11;
            else if (phase == 11)
                phase = 10;
            else if (phase == 12)
                phase = 13;
            else if (phase == 13)
                phase = 12;
        }
        return phase;
    }
}
