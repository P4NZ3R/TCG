using UnityEngine;
using System.Collections;

public class BattleHandler : MonoBehaviour
{
    bool playerTurn = true;
    public struct Player
    {
        public int numDice;
        public GameObject[] dice;
        public int[] neutral;
        public int[] earth;
        public int[] fire;
        public int[] water;
        public int[] wind;
    }

    Player player;
    Player enemy;
    // Use this for initialization
    void Start()
    {
        player = CreatePlayer();
        enemy = CreatePlayer();

        player = AddDice(player);
        player = AddDice(player);
        AddDice(enemy);
        AddDice(enemy);

        RollAllDices(player);
        Debug.Log("numdice:"+player.numDice);
    }

    // Update is called once per frame
    void Update()
    {

    }

    Player CreatePlayer()
    {
        Player guest = new Player();
        guest.dice = GameObject.FindGameObjectsWithTag("Dice");
        guest.neutral = new int[5];
        guest.earth = new int[5];
        guest.fire = new int[5];
        guest.water = new int[5];
        guest.wind = new int[5];
        return guest;
    }

    public Color[] RollAllDices(Player guest)
    {
        Color[] value = new Color[guest.numDice];
        for (int i = 0; i < guest.numDice; i++)
        {
            value[i]=RollDice(guest,i);
            guest.dice[i].GetComponent<UnityEngine.UI.Image>().color = value[i];
        }
        return value;
    }

    public Color RollDice(Player guest,int id)
    {
        int value = Random.Range(1, 7);
        value -= guest.neutral[id];
        if (value <= 0) return Color.gray;
        value -= guest.earth[id];
        if (value <= 0) return Color.black;
        value -= guest.fire[id];
        if (value <= 0) return Color.red;
        value -= guest.water[id];
        if (value <= 0) return Color.blue;
        value -= guest.wind[id];
        if (value <= 0) return Color.cyan;
        return Color.white;
    }

    public Player AddDice(Player guest)
    {
        if (guest.numDice >= 5)
        {
            return guest;
        }
        guest.neutral[guest.numDice] = 2;
        guest.earth[guest.numDice] = 1;
        guest.fire[guest.numDice] = 1;
        guest.water[guest.numDice] = 1;
        guest.wind[guest.numDice] = 1;
        guest.numDice++;
        return guest;
    }
}
