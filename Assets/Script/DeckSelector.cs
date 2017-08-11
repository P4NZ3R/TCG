using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSelector : MonoBehaviour {
    public static DeckSelector singleton;
    public GameObject prefabDeck;
    public ScriptableDeck selectedDeck;
    public ScriptableDeck[] decks;
	// Use this for initialization
	void Awake () {
        singleton = this;
    }

    void Start ()
    {
        decks = Resources.LoadAll<ScriptableDeck>("Decks");
        for (int i = 0; i < decks.Length; i++)
        {
            ButtonDeckSelector tmp = Instantiate(prefabDeck).GetComponent<ButtonDeckSelector>();
            tmp.idDeck = i;
            tmp.nomeDeck.text = decks[i].name;
            tmp.transform.SetParent(transform);
        }
    }
}
