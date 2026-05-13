using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEditor.Rendering;

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
    public UnityEngine.UI.Button Attack_1_2;
    public TextMeshProUGUI Attack_used;
    public TextMeshProUGUI Ai_cards_left;
    public TextMeshProUGUI Player_cards_left;
    
    // Cooldown tracking for special attacks
    private int attack_1_2_cooldown = 0;  // Player's double damage attack
    private int attack_2_2_cooldown = 0;  // AI's double damage attack
    private bool game_ended = false;  // Track if the game has ended

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
        Attack_1_2 = GameObject.Find("Attack_1_2").GetComponent<UnityEngine.UI.Button>();
        Attack_used = GameObject.Find("Attack_used").GetComponent<TextMeshProUGUI>();
        Ai_cards_left = GameObject.Find("Ai_cards_left").GetComponent<TextMeshProUGUI>();
        Player_cards_left = GameObject.Find("Player_cards_left").GetComponent<TextMeshProUGUI>();
        StartCoroutine(ShuffleDecksSequentially());
    }

    IEnumerator ShuffleDecksSequentially()
    {
        // Populate player and AI decks with 6 cards each from the main deck
        for (int i = 0; i < 6 && i < deck.Count; i++)
        {
            player_deck.Add(deck[i].Clone());
            ai_deck.Add(deck[i].Clone());
        }
        
        Shuffle(player_deck);
        yield return new WaitForEndOfFrame();
        Shuffle(ai_deck);
        yield return new WaitForEndOfFrame();
        Deal(Player_card, "Player");
        Deal(Ai_card, "Ai");
        UpdateCardCounts();
        
        // Determine turn order based on card speed
        string firstPlayer = Player_card.data.speed >= Ai_card.data.speed ? "Player" : "Ai";
        Game_order(firstPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Deal(Card replacement, string deckType)
    {
        if (deckType == "Player")
        {
            if (player_deck.Count > 0)
            {
                replacement.data = player_deck[0].Clone();
                Player_card.data = replacement.data;
                Player_card.UpdateCard();
                player_hand.Add(replacement.data);
                player_deck.RemoveAt(0);
            }
        } else if (deckType == "Ai")
        {
            if (ai_deck.Count > 0)
            {
                replacement.data = ai_deck[0].Clone();
                Ai_card.data = replacement.data;
                Ai_card.UpdateCard();
                ai_hand.Add(replacement.data);
                ai_deck.RemoveAt(0);
            }
        }
    }

    public void ReplaceCard(Card replacement, string deckType)
    {
        if (deckType == "Player")
        {
            while (Player_card.data.health <= 0 && player_deck.Count > 0)
            {
                replacement.data = player_deck[0].Clone();
                Player_card.data = replacement.data;
                Player_card.UpdateCard();
                if (player_hand.Count > 0)
                {
                    player_hand.RemoveAt(0);
                }
                player_hand.Add(replacement.data);
                player_deck.RemoveAt(0);
                // Reset double damage cooldown when a new card is created
                attack_1_2_cooldown = 0;
                UpdateCardCounts();
            }
            // Only end the game if there are no more cards in deck AND the current card is dead
            if (player_deck.Count == 0 && Player_card.data.health <= 0)
            {
                GameEnd("Ai");
            }
        } else if (deckType == "Ai")
        {
            while (Ai_card.data.health <= 0 && ai_deck.Count > 0)
            {
                replacement.data = ai_deck[0].Clone();
                Ai_card.data = replacement.data;
                Ai_card.UpdateCard();
                if (ai_hand.Count > 0)
                {
                    ai_hand.RemoveAt(0);
                }
                ai_hand.Add(replacement.data);
                ai_deck.RemoveAt(0);
                // Reset double damage cooldown when a new card is created
                attack_2_2_cooldown = 0;
                UpdateCardCounts();
            }
            // Only end the game if there are no more cards in deck AND the current card is dead
            if (ai_deck.Count == 0 && Ai_card.data.health <= 0)
            {
                GameEnd("Player");
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
        ReduceCooldowns();
        Attack_1.gameObject.SetActive(false);
        Attack_1_2.gameObject.SetActive(false);
        
        // AI automatically attacks
        StartCoroutine(ExecuteAIAttack());
    }
    
    IEnumerator ExecuteAIAttack()
    {
        yield return new WaitForSeconds(1f);  // Add delay for better UX
        
        // If double damage is available and normal damage wouldn't kill opponent, use it
        if (IsAttack_2_2Available() && Ai_card.data.damage < Player_card.data.health)
        {
            int doubleDamage = Ai_card.data.damage * 2;
            Player_card.data.health -= doubleDamage;
            Player_card.UpdateCard();
            UseAttack_2_2();
            UpdateAttackText("Ai", "double damage attack");
            
            if (Player_card.data.health <= 0)
            {
                ReplaceCard(Player_card, "Player");
            }
        }
        else
        {
            // Use basic attack
            Player_card.data.health -= Ai_card.data.damage;
            Player_card.UpdateCard();
            UpdateAttackText("Ai", "basic attack");
            
            if (Player_card.data.health <= 0)
            {
                ReplaceCard(Player_card, "Player");
            }
        }
        
        Game_order("Player");
    }
    void Player_Turn()
    {
        ReduceCooldowns();
        Attack_1.gameObject.SetActive(true);
        Attack_1_2.gameObject.SetActive(attack_1_2_cooldown == 0);
    }
    
    void ReduceCooldowns()
    {
        if (attack_1_2_cooldown > 0)
            attack_1_2_cooldown--;
        if (attack_2_2_cooldown > 0)
            attack_2_2_cooldown--;
    }
    
    public bool IsAttack_1_2Available()
    {
        return attack_1_2_cooldown == 0;
    }
    
    public void UseAttack_1_2()
    {
        attack_1_2_cooldown = 5;
    }
    
    public bool IsAttack_2_2Available()
    {
        return attack_2_2_cooldown == 0;
    }
    
    public void UseAttack_2_2()
    {
        attack_2_2_cooldown = 5;
    }
    
    public int GetAttack_1_2Cooldown()
    {
        return attack_1_2_cooldown;
    }
    
    public int GetAttack_2_2Cooldown()
    {
        return attack_2_2_cooldown;
    }
    
    public void UpdateAttackText(string attacker, string attackType)
    {
        if (attacker == "Player")
        {
            Attack_used.text = "Player used " + attackType;
        }
        else if (attacker == "Ai")
        {
            Attack_used.text = "Ai used " + attackType;
        }
    }
    
    public void UpdateCardCounts()
    {
        Player_cards_left.text = "Player Cards Left: " + player_deck.Count;
        Ai_cards_left.text = "Ai Cards Left: " + ai_deck.Count;
    }
    
    public void UpdateWinnerText(string winner)
    {
        if (winner == "Player")
        {
            Attack_used.text = "Player wins!";
        }
        else if (winner == "Ai")
        {
            Attack_used.text = "Ai wins!";
        }
    }
    
    public void Game_order(string player)
    {
        if (game_ended)
        {
            return;  // Don't allow any turns after game has ended
        }
        
        if (player == "Player")
        {
            Player_Turn();
        } else if (player == "Ai")
        {
            AI_Turn();
        }
    }
    
    void GameEnd(string winner)
    {
        game_ended = true;  // Mark the game as ended
        UpdateWinnerText(winner);
        Attack_1.gameObject.SetActive(false);
        Attack_1_2.gameObject.SetActive(false);
        Player_card.gameObject.SetActive(false);
        Ai_card.gameObject.SetActive(false);
        
    }
    
}