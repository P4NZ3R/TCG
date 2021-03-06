﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardHandler : MonoBehaviour,IPointerEnterHandler {
    public bool playerOwner;
    ScriptableCard.Type position;
    [SerializeField]
    Text cardName;
    [SerializeField]
    Image img;
    [SerializeField]
    Text power;
    [SerializeField]
    GameObject cover;

    [HideInInspector]
    public ScriptableCard ScriptCard;
    [HideInInspector]
    public int currentPower;

    public void SetCard (ScriptableCard card,ScriptableCard.Type pos,bool playerOwner=true,bool isBot=false,bool interactable=true) {
        position = pos;
        this.playerOwner = playerOwner;
        cardName.text = card.nome;
        img.sprite = card.image;
        power.text = card.power.ToString();
        currentPower = (int)card.power;
        this.ScriptCard = card;
        if (!interactable)
            GetComponent<Button>().interactable = false;
        SetCover(isBot && !playerOwner);
	}

    public void Interactable(bool value)
    {
        GetComponent<Button>().interactable = value;
    }

    public void SetCover(bool value)
    {
        cover.SetActive(value);
    }

    public void SetPosition(ScriptableCard.Type pos)
    {
        position = pos;
    }

    public bool ChangePower(int value)
    {
        if (!gameObject)
        {
            Debug.LogError("ehm..");
            return false;
        }
        currentPower += value;
            power.text = currentPower.ToString();
        power.color = currentPower == ScriptCard.power ? Color.white : currentPower > ScriptCard.power ? Color.green : Color.red;
        if (currentPower <= 0)
            Death();
        return currentPower <= 0;
    }

    public void OnClick()
    {
        if (playerOwner && PlayerHandler.singletonPlayer.canSummon)
        {
            PlayerHandler.singletonPlayer.canSummon = false;
            GameManager.singleton.RequestNextPhase();
            PlayerHandler.singletonPlayer.RemoveCardFromHand(this);
            PlayerHandler.singletonPlayer.SummonCreature(this);
        }
        else if (!playerOwner && PlayerHandler.singletonOpponent.canSummon)
        {
            PlayerHandler.singletonOpponent.canSummon = false;
            GameManager.singleton.RequestNextPhase();
            PlayerHandler.singletonOpponent.RemoveCardFromHand(this);
            PlayerHandler.singletonOpponent.SummonCreature(this);
        }
    }

    public void ActivateEffect(GameManager.Phase currPhase,CardHandler card=null)
    {
        int phase = (int)currPhase;
        if (!playerOwner)
            phase = PlayerHandler.singletonOpponent.ConvertPhaseOpponentPlayer((int)currPhase);
        foreach (ScriptableCard.Effect _effect in ScriptCard.effects)
        {
            if (phase == (int)_effect.phase && position == _effect.type)
                EffectManager.singleton.Activate(this,_effect);
        }
    }

    public void Death()
    {
        if (playerOwner)
            PlayerHandler.singletonPlayer.DestroyCreature(this);
        else
            PlayerHandler.singletonOpponent.DestroyCreature(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!cover.activeSelf)
        ShowCard.singleton.ShowSelectedCard(this);
    }
}
