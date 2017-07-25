using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour {
    [SerializeField]
    Text cardName;
    [SerializeField]
    Image img;

    public void SetCard (ScriptableCard card) {
        cardName.text = card.nome;
        img.sprite = card.image;
		
	}
}
