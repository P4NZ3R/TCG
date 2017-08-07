using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour {
    [SerializeField]
    Text cardName;
    [SerializeField]
    Image img;
    [SerializeField]
    Text power;

    [HideInInspector]
    public ScriptableCard ScriptCard;
    int initPower;
    int currentPower;
    public void SetCard (ScriptableCard card,bool interactable=true) {
        cardName.text = card.nome;
        img.sprite = card.image;
        power.text = card.power.ToString();
        initPower = currentPower = (int)card.power;
        this.ScriptCard = card;
        if (!interactable)
            GetComponent<Button>().interactable = false;
	}

    public bool Damage(int dmg)
    {
        currentPower -= dmg;
        power.text = currentPower.ToString();
        if (currentPower <= 0)
            Death();
        return currentPower <= 0;
    }

    public void OnClick()
    {
        PlayerHandler.singleton.RemoveCardInHand(this);
        PlayerHandler.singleton.SummonCreature(this);
    }

    public void Death()
    {
        PlayerHandler.singleton.DestroyCreature(this);
    }
}
