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
            // Check if AI card died and replace if necessary
            if (GameManager.gm.Ai_card.data.health <= 0)
            {
                GameManager.gm.ReplaceCard(GameManager.gm.Ai_card, "AI");
            }
            GameManager.gm.Game_order("AI");
        } else if (buttonName == "Attack_2")
        {
            GameManager.gm.Player_card.data.health -= GameManager.gm.Ai_card.data.damage;
            GameManager.gm.Player_card.UpdateCard();
            // Check if Player card died and replace if necessary
            if (GameManager.gm.Player_card.data.health <= 0)
            {
                GameManager.gm.ReplaceCard(GameManager.gm.Player_card, "Player");
            }
            GameManager.gm.Game_order("Player");
        }
    }
}
