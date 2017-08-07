using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {
    public static PlayerHandler singleton;
    //
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

    void Awake()
    {
        singleton = this;
        cardsLeft = deck.Length;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            GameManager.singleton.NextPhase();
	}

    public void AddCardInHand(ScriptableCard card)
    {
        GameObject go = Instantiate(prefabCard);
        go.transform.SetParent(handLayout.transform);
        CardHandler cardHandler = go.GetComponent<CardHandler>();
        cardHandler.SetCard(card,true);
        
        hands.Add(cardHandler);
    }

    public void RemoveCardInHand(CardHandler card)
    {
        card.Interactable(false);
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
        GameManager.singleton.events[12](GameManager.Phase.DestroyPerm);//DestroyPerm
    }
}
