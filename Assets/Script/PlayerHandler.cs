using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHandler : MonoBehaviour {
    public static PlayerHandler singletonPlayer;
    public static PlayerHandler singletonOpponent;
    //
    public bool isEnemy;
    public bool isBot;
    public bool speedUp;
    public GameObject prefabCard;
    [SerializeField]
    GameObject handLayout;
    [SerializeField]
    GameObject creaturesLayout;
    [SerializeField]
    Text healthText;
    [SerializeField]
    Text deckCounter;
    //
    int healthLeft=30;
    public int HealthLeft{
        get
        { 
            return healthLeft;
        }
        set
        {
            healthLeft = value; 
            healthText.text = healthLeft.ToString();
            healthText.color = healthLeft >= 20 ? Color.black : healthLeft >= 10 ? Color.yellow : Color.red;
            if (healthLeft <= 0)
                LoseGame();
        }
    }
    [HideInInspector]
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

        if (!isEnemy)
        {
            deck = new ScriptableCard[DeckSelector.singleton.selectedDeck.deck.Length];
            for (int i = 0; i < DeckSelector.singleton.selectedDeck.deck.Length; i++)
            {
                deck[i] = DeckSelector.singleton.selectedDeck.deck[i];
            }
        }
        else
        {
            ScriptableDeck[] _decks = Resources.LoadAll<ScriptableDeck>("Decks Op");
            int idDeck = Random.Range(0, _decks.Length);
            deck = new ScriptableCard[_decks[idDeck].deck.Length];
            for (int i = 0; i < deck.Length; i++)
            {
                deck[i] = _decks[idDeck].deck[i];
            }
        }
        
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
        if (Input.GetKeyDown(KeyCode.Escape))
            BackToMenu();
	}

    public void BotActive()
    {
        isBot = !isBot;
    }

    public void SpeedUp()
    {
        speedUp = !speedUp;
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
        deckLeft.Insert(Random.Range(0,deckLeft.Count-1),cardHandler);

        AddEffectOnDelegates(cardHandler, ScriptableEffect.Type.Deck);

        deckCounter.text = deckLeft.Count.ToString();
        deckCounter.color = deckLeft.Count > 0 ? Color.black : Color.red;
    }

    public void RemoveCardFromDeck(CardHandler card)
    {
        deckLeft.Remove(card);

        RemoveEffectOnDelegates(card, ScriptableEffect.Type.Deck);

        deckCounter.text = deckLeft.Count.ToString();
        deckCounter.color = deckLeft.Count > 0 ? Color.black : Color.red;
    }

    public void DiscardCardinHand()
    {
        if (hand.Count <= 0)
            return;

        CardHandler card = hand[0];
        ScriptableEffect _effect = UtilityFunctions.SearchEffectPhase(card.ScriptCard, GameManager.Phase.SelfDiscard);
        if (_effect != null)
        {
            _effect.Activate(card);
        }
            
        RemoveCardFromHand(card);
        Destroy(card.gameObject);

        if(isEnemy)
            GameManager.singleton.events[15](GameManager.Phase.OpDiscard,card);
        else
            GameManager.singleton.events[14](GameManager.Phase.Discard,card);
    }


    public void AddCardInHand(CardHandler card)
    {
        card.transform.SetParent(handLayout.transform);
        card.transform.SetAsFirstSibling();
        hand.Add(card);

        AddEffectOnDelegates(card, ScriptableEffect.Type.Hand);
    }

    public void RemoveCardFromHand(CardHandler card)
    {
        card.Interactable(false);
        card.SetCover(false);
        hand.Remove(card);

        RemoveEffectOnDelegates(card, ScriptableEffect.Type.Hand);

    }

    void AddEffectOnDelegates(CardHandler card,ScriptableEffect.Type _type)
    {
        foreach (ScriptableCard.Effect _effect in card.ScriptCard.effects)
        {
            int phase = (int)_effect.phase;
            //controlli sul proprietario della carta per applicare gli effetti
            if (isEnemy)
            {
                phase=ConvertPhaseOpponentPlayer(phase);
            }
            //
            if (phase <= 15 && _type==_effect.effect.type)
            {
                GameManager.singleton.events[phase] -= card.ActivateEffect;//nel caso ci siano effetti multipli nella stessa fase evita che stackino assieme
                GameManager.singleton.events[phase] += card.ActivateEffect;
            }
            else if (_effect.phase == GameManager.Phase.SelfSummon && _type==ScriptableEffect.Type.Battlefield)
                _effect.effect.Activate(card);
            else if (_effect.effect.effect == ScriptableEffect.Effects.RevealCardInHand && _type==ScriptableEffect.Type.Hand)
                _effect.effect.Activate(card);
        }
    }

    void RemoveEffectOnDelegates(CardHandler card,ScriptableEffect.Type _type)
    {
        foreach (ScriptableCard.Effect _effect in card.ScriptCard.effects)
        {
            int phase = (int)_effect.phase;
            //controlli sul proprietario della carta per applicare gli effetti
            if (isEnemy)
            {
                phase=ConvertPhaseOpponentPlayer(phase);
            }
            //
            if (phase <= 15 && _type==_effect.effect.type)
            {
                GameManager.singleton.events[phase] -= card.ActivateEffect;
            }
            else if (_effect.phase == GameManager.Phase.SelfDeath && _type==ScriptableEffect.Type.Battlefield)
                _effect.effect.Activate(card);
        }
    }

    void LoseGame()
    {
        Debug.LogWarning(gameObject.name+" lose the game");
        GameManager.singleton.gameEnded = true;
        Invoke("BackToMenu", 5f);
    }

    void BackToMenu()
    {
        SceneManager.LoadScene("deckSelect");
    }

    public void SummonCreature(CardHandler card)
    {
        creatures.Add(card);
        card.transform.SetParent(creaturesLayout.transform);
        card.transform.SetAsFirstSibling();

        AddEffectOnDelegates(card, ScriptableEffect.Type.Battlefield);

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
        RemoveEffectOnDelegates(card, ScriptableEffect.Type.Battlefield);

//        
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
            else if (phase == 15)
                phase = 14;
        }
        return phase;
    }
}
