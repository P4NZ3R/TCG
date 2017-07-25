using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {
    //
    [SerializeField]
    GameManager manager;
    [SerializeField]
    GameObject prefabCard;
    [SerializeField]
    GameObject handLayout;
    //
    public ScriptableCard[] deck;
    List<ScriptableCard> hands = new List<ScriptableCard>();
    public int cardsLeft;

    void Awake()
    {
        cardsLeft = deck.Length;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            manager.NextPhase();
	}

    public void AddCardInHand(ScriptableCard card)
    {
        hands.Add(card);
        RefreshHandGui();
    }

    void RefreshHandGui()
    {
        for (int i = 0; i < handLayout.transform.childCount; i++)
        {
            Destroy(handLayout.transform.GetChild(i).gameObject);
        }

        foreach (ScriptableCard card in hands)
        {
            GameObject go = Instantiate(prefabCard);
            go.transform.SetParent(handLayout.transform);
            go.GetComponent<CardHandler>().SetCard(card);
        }
    }
}
