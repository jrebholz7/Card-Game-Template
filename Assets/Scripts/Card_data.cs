using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Card_data", menuName = "Cards/Card_data", order = 1)]
public class Card_data : ScriptableObject
{
    public string card_name;
    public string description;
    public int health;
    public int speed;
    public int damage;
    public Sprite sprite;
    public MonoScript card_script;

    /// <summary>
    /// Creates a duplicate copy of this card data with independent values
    /// </summary>
    public Card_data Clone()
    {
        Card_data clonedCard = CreateInstance<Card_data>();
        clonedCard.card_name = this.card_name;
        clonedCard.description = this.description;
        clonedCard.health = this.health;
        clonedCard.speed = this.speed;
        clonedCard.damage = this.damage;
        clonedCard.sprite = this.sprite;
        clonedCard.card_script = this.card_script;
        return clonedCard;
    }
}
