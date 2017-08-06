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

    //TODO la carta in mano viene cancellata dopo questa funzione quindi le modifiche su cardhandler non rimangono
    //TODO on deve essere passata la delegate sullo scripable ma su cardHandler e cardhandler chiama quella sullo scripable, cosi e possibile passare i parametri
    public void SummonCreature(ScriptableCard ScriptCard,CardHandler card)
    {
        creatures.Add(ScriptCard);
        RefreshGuiLayout(creatures,creaturesLayout.transform,false);
        foreach (ScriptableCard.Effect _effect in ScriptCard.effects)
        {
            int phase = (int)_effect.phase;
            if (phase <= 7)
            {
                GameManager.singleton.events[phase] += _effect.effect.Activate;
            }
            else
            {
                if (_effect.phase == GameManager.Phase.Summon)
                {
                    Debug.LogError("summon effect");
                    _effect.effect.Activate(card);
                }
            }
        }
        GameManager.singleton.SummonPerm();
    }

    //TODO da rifare perche non si puo settare il power alla carta visto che viene sempre cancellata
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
