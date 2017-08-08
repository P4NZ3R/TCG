using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCard : MonoBehaviour {
    public static ShowCard singleton;
    [SerializeField]
    Text cardName;
    [SerializeField]
    Image img;
    [SerializeField]
    Text power;
    [SerializeField]
    Text Description;

    void Awake()
    {
        singleton = this;
    }

    public void ShowSelectedCard (CardHandler card) 
    {
        cardName.text = card.ScriptCard.nome;
        img.sprite = card.ScriptCard.image;
        power.text = card.ScriptCard.power.ToString();
        Description.text = card.ScriptCard.description;
	}
}
