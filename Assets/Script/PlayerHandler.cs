using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {
    [SerializeField]
    GameManager manager;

    public ScriptableCard[] deck;
    public List<ScriptableCard> hands = new List<ScriptableCard>();
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
}
