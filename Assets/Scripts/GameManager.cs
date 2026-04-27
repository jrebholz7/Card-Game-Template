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
    public UnityEngine.UI.Button Attack_1;
    public UnityEngine.UI.Button Attack_2;

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
        Attack_1 = GameObject.Find("Attack_1").GetComponent<UnityEngine.UI.Button>();
        Attack_2 = GameObject.Find("Attack_2").GetComponent<UnityEngine.UI.Button>();
        StartCoroutine(ShuffleDecksSequentially());
    }

    IEnumerator ShuffleDecksSequentially()
    {
        Shuffle(player_deck);
        yield return new WaitForEndOfFrame();
        Shuffle(ai_deck);
        yield return new WaitForEndOfFrame();
        Deal(Player_card, "Player");
        Deal(Ai_card, "AI");
        Game_order("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Deal(Card replacement, string deckType)
    {
        Debug.Log("Dealing function called");
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("for statement");
            if (deckType == "Player")
            {
                if (player_deck.Count > 0)
                {
                    Debug.Log("if statement works");
                    replacement.data = player_deck[0].Clone();
                    Player_card.data = replacement.data;
                    Player_card.UpdateCard();
                    player_hand.Add(replacement.data);
                    player_deck.RemoveAt(0);
                }
            } else if (deckType == "AI")
            {
                if (ai_deck.Count > 0)
                {
                    Debug.Log("if statement works but from ai");
                    replacement.data = ai_deck[0].Clone();
                    Ai_card.data = replacement.data;
                    Ai_card.UpdateCard();
                    ai_hand.Add(replacement.data);
                    ai_deck.RemoveAt(0);
                }
            }
        }
    }

    public void ReplaceCard(Card replacement, string deckType)
    {
        if (deckType == "Player")
        {
            if (player_deck.Count > 0)
            {
                replacement.data = player_deck[0].Clone();
                Player_card.data = replacement.data;
                Player_card.UpdateCard();
                player_hand.RemoveAt(0);
                player_hand.Add(replacement.data);
                player_deck.RemoveAt(0);
            }
        } else if (deckType == "AI")
        {
            if (ai_deck.Count > 0)
            {
                replacement.data = ai_deck[0].Clone();
                Ai_card.data = replacement.data;
                Ai_card.UpdateCard();
                ai_hand.RemoveAt(0);
                ai_hand.Add(replacement.data);
                ai_deck.RemoveAt(0);
            }
        }
    }

    void Shuffle(List<Card_data> _deck)
    {
        if (_deck.Count > 0)
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
    }
    void AI_Turn()
    {
        Attack_1.gameObject.SetActive(false);
        Attack_2.gameObject.SetActive(true);
        if (Ai_card.data.health <= 0)
        {
            ReplaceCard(Ai_card, "AI");
        }

    }
    void Player_Turn()
    {
        Attack_1.gameObject.SetActive(true);
        Attack_2.gameObject.SetActive(false);
        if (Player_card.data.health <= 0)
        {
            ReplaceCard(Player_card, "Player");
        }

    }
    public void Game_order(string player)
    {
        if (player == "Player")
        {
            Player_Turn();
        } else if (player == "AI")
        {
            AI_Turn();
        }
    }
    
}
