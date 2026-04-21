using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public List<Card_data> deck = new List<Card_data>();
    public List<Card_data> player_deck = new List<Card_data>();
    public List<Card_data> ai_deck = new List<Card_data>();
    public List<Card_data> player_hand = new List<Card_data>();
    public List<Card_data> ai_hand = new List<Card_data>();
    public List<Card_data> discard_pile = new List<Card_data>();
    public Canvas canvas;
    public Card blank;
    public Card Player_card = GameObject.Find("Player_card").GetComponent<Card>();
    public Card Ai_card = GameObject.Find("Ai_card").GetComponent<Card>();
    public Vector3 player_hand_spawnpoint;
    public Vector3 offset;

    private void Awake()
    {
        if (gm != null && gm != this)
        {
            Destroy(gameObject);
        }
        else
        {
            gm = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        canvas = FindAnyObjectByType<Canvas>();
        Player_card = GameObject.Find("Player_card").GetComponent<Card>();
        Ai_card = GameObject.Find("Ai_card").GetComponent<Card>();
        Deal(Player_card);
        Deal(Ai_card);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Deal(Card replacement)
    {
        Shuffle(player_deck);
        Shuffle(ai_deck);
        for (int i = 0; i < 5; i++)
        {
            //player_hand.Add(player_deck[i]);
            //Card current_card = Instantiate(blank, player_hand_spawnpoint + offset, Quaternion.identity, canvas.transform);
            //offset.x += 300;
            
            if (replacement == Player_card)
            {
                replacement.data = player_deck[i];
                Player_card.data = replacement.data;
                player_deck.Remove(replacement.data);
                ///player_hand.Add(current_card.data);
            } else if (replacement == Ai_card)
            {
                replacement.data = ai_deck[i];
                Ai_card.data = replacement.data;
                ai_deck.Remove(replacement.data);
            }
            //current_card.transform.SetParent(canvas.transform);
            
        }

        for (int i = 0; i<ai_deck.Count; i++)
        {
            ai_hand.Add(ai_deck[i]);
        }
    }

    void Shuffle(List<Card_data> _deck)
    {
        System.Random rng = new System.Random();
        for (int i = 0; i < _deck.Count; i++)
        {
            int j = rng.Next(_deck.Count);
            Card_data temp = _deck[i];
            _deck[i] = _deck[j];
            _deck[j] = temp;
        }
    }
    void AI_Turn()
    {
        if (Ai_card.data.health <= 0)
        {
            Deal(Ai_card);
        }
        Player_card.data.health -= Ai_card.data.damage;
    }
    void Player_Turn()
    {
        if (Player_card.data.health <= 0)
        {
            Deal(Player_card);
        }
        Ai_card.data.health -= Player_card.data.damage;
    }
}
