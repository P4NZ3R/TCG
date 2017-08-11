using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonDeckSelector : MonoBehaviour {
    public int idDeck;
    public Text nomeDeck;

    public void SelectDeck()
    {
        DeckSelector.singleton.selectedDeck = DeckSelector.singleton.decks[idDeck];
        SceneManager.LoadScene("gameScene");
    }
}
