using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour {
    [SerializeField]
    Text cardName;
    [SerializeField]
    Image img;

    ScriptableCard card;

    public void SetCard (ScriptableCard card,bool interactable=true) {
        cardName.text = card.nome;
        img.sprite = card.image;
        this.card = card;
        if (!interactable)
            GetComponent<Button>().interactable = false;
	}

    public void OnClick()
    {
        PlayerHandler.singleton.RemoveCardInHand(card);
        PlayerHandler.singleton.SummonCreature(card);
    }
}
