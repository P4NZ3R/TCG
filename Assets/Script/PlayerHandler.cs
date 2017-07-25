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
    List<ScriptableCard> hands = new List<ScriptableCard>();
    List<ScriptableCard> creatures = new List<ScriptableCard>();
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
        hands.Add(card);
        RefreshGuiLayout(hands,handLayout.transform);
    }

    public void RemoveCardInHand(ScriptableCard card)
    {
        hands.Remove(card);
        RefreshGuiLayout(hands,handLayout.transform);
    }

    public void SummonCreature(ScriptableCard card)
    {
        creatures.Add(card);
        RefreshGuiLayout(creatures,creaturesLayout.transform,false);
//        foreach (ScriptableCard.Effect effect in card.effects)//TODO aggiungere gli effetti della carta negli eventi
//        {
//            if(effect.type <= 7)
//                GameManager.singleton.events[effect.type] += 
//        }
        GameManager.singleton.SummonPerm();
    }

    void RefreshGuiLayout(List<ScriptableCard>list, Transform layoutGui,bool interactable=true)
    {
        for (int i = 0; i < layoutGui.childCount; i++)
        {
            Destroy(layoutGui.GetChild(i).gameObject);
        }

        foreach (ScriptableCard card in list)
        {
            GameObject go = Instantiate(prefabCard);
            go.transform.SetParent(layoutGui.transform);
            go.GetComponent<CardHandler>().SetCard(card,interactable);
        }
    }
}
