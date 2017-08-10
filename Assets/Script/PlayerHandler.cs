using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour {
    public static PlayerHandler singletonPlayer;
    public static PlayerHandler singletonOpponent;
    //
    public bool isEnemy;
    public bool isBot;
    public GameObject prefabCard;
    [SerializeField]
    GameObject handLayout;
    [SerializeField]
    GameObject creaturesLayout;
    [SerializeField]
    Text healthText;
    //
    int healthLeft;
    public int HealthLeft{
        get
        { 
            return healthLeft;
        }
        set
        {
            healthLeft = value; 
            healthText.text = healthLeft.ToString();
            if (healthLeft <= 0)
                LoseGame();
        }
    }
    public ScriptableCard[] deck;
    public List<CardHandler> deckLeft = new List<CardHandler>();
    public List<CardHandler> hand = new List<CardHandler>();
    public List<CardHandler> creatures = new List<CardHandler>();
    public bool canSummon=false;

    void Awake()
    {
        if (isEnemy)
            singletonOpponent = this;
        else
            singletonPlayer = this;
    }

	// Use this for initialization
	void Start () {
        HealthLeft = 30;
        if (isBot && !isEnemy && canSummon)
            BotPlayCard();
	}
	
	// Update is called once per frame
	void Update () {
        if (canSummon && isBot && hand.Count > 0)
        {
            BotPlayCard();
        }
	}

    void BotPlayCard()
    {
        CardHandler _card = hand[0];
        RemoveCardFromHand(_card);
        SummonCreature(_card);
        GameManager.singleton.RequestNextPhase();
        canSummon = false;
    }

    public void AddCardInDeck(ScriptableCard card)
    {
        GameObject go = Instantiate(prefabCard);
        go.name = "card: "+card.nome;
        go.transform.SetParent(transform);
        CardHandler cardHandler = go.GetComponent<CardHandler>();
        cardHandler.SetCard(card,!isEnemy,isBot,!isBot);

        deckLeft.Add(cardHandler);
    }

    public void DiscardCardinHand()
    {
        if (hand.Count <= 0)
            return;
        Debug.LogError("discarded "+hand[0].name);

        CardHandler card = hand[0];
        ScriptableEffect _effect = UtilityFunctions.SearchEffectPhase(card.ScriptCard, GameManager.Phase.SelfDiscard);
        if (_effect != null)
        {
            Debug.LogError("asdas");
            _effect.Activate(card);
        }
            
        RemoveCardFromHand(card);
        Destroy(card.gameObject);

        if(isEnemy)
            GameManager.singleton.events[15](GameManager.Phase.OpDiscard,card);
        else
            GameManager.singleton.events[14](GameManager.Phase.Discard,card);
    }

    public void RemoveCardFromDeck(CardHandler card)
    {
        deckLeft.Remove(card);
    }

    public void AddCardInHand(CardHandler card)
    {
        card.transform.SetParent(handLayout.transform);
        card.transform.SetAsFirstSibling();

        hand.Add(card);
    }

    void LoseGame()
    {
        Debug.LogError(gameObject.name+" lose the game");
        GameManager.singleton.gameEnded = true;
    }

    public void RemoveCardFromHand(CardHandler card)
    {
        card.Interactable(false);
        card.SetCover(false);
        hand.Remove(card);
    }

    public void SummonCreature(CardHandler card)
    {
        ScriptableCard ScriptCard = card.ScriptCard;
        creatures.Add(card);
        card.transform.SetParent(creaturesLayout.transform);
        card.transform.SetAsFirstSibling();
        foreach (ScriptableCard.Effect _effect in ScriptCard.effects)
        {
            int phase = (int)_effect.phase;
            //controlli sul proprietario della carta per applicare gli effetti
            if (isEnemy)
            {
                phase=ConvertPhaseOpponentPlayer(phase);
            }
            //
            if (phase <= 15)
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
        if (!card)
        {
            Debug.LogError("no creature to destroy");
            return;
        }
            
        creatures.Remove(card);

        foreach (ScriptableCard.Effect _effect in card.ScriptCard.effects)
        {
            int phase = (int)_effect.phase;
            if (isEnemy)
            {
                phase=ConvertPhaseOpponentPlayer(phase);
            }
            if (phase <= 15)
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
            else if (phase == 14)
                phase = 15;
            else if (phase == 15)
                phase = 14;
        }
        return phase;
    }
}
