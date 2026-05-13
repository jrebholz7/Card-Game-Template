using UnityEngine;
using UnityEngine.UI;
public class ButtonController : MonoBehaviour
{
    public UnityEngine.UI.Button myButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myButton.onClick.AddListener(OnButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnButtonClicked()
    {
        string buttonName = gameObject.name;
        Debug.Log(buttonName + " was clicked!");
        if (buttonName == "Attack_1")
        {
            GameManager.gm.Ai_card.data.health -= GameManager.gm.Player_card.data.damage;
            GameManager.gm.Ai_card.UpdateCard();
            GameManager.gm.UpdateAttackText("Player", "basic attack");
            // Check if AI card died and replace if necessary
            if (GameManager.gm.Ai_card.data.health <= 0)
            {
                GameManager.gm.ReplaceCard(GameManager.gm.Ai_card, "Ai");
            }
            GameManager.gm.Game_order("Ai");
        } else if (buttonName == "Attack_1_2")
        {
            if (GameManager.gm.IsAttack_1_2Available())
            {
                int doubleDamage = GameManager.gm.Player_card.data.damage * 2;
                GameManager.gm.Ai_card.data.health -= doubleDamage;
                GameManager.gm.Ai_card.UpdateCard();
                GameManager.gm.UseAttack_1_2();
                GameManager.gm.UpdateAttackText("Player", "double damage attack");
                Debug.Log("Player uses double damage attack! Cooldown: 5 turns");
                // Check if AI card died and replace if necessary
                if (GameManager.gm.Ai_card.data.health <= 0)
                {
                    GameManager.gm.ReplaceCard(GameManager.gm.Ai_card, "Ai");
                }
                GameManager.gm.Game_order("Ai");
            } else
            {
                Debug.Log("Attack_1_2 is on cooldown! Remaining cooldown: " + GameManager.gm.GetAttack_1_2Cooldown());
            }
        }
    }
}
