using System.Collections;
using System.Collections.Generic;
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

}
// GO TO EXCALIDRAW ON THE JOSEPH GOOGLE ACCOUNT FOR ALL OF THE PLAN FOR THIS

//How speed will work, EITHER
//1. One time, at the start of game, determines repetitive turn order, whoever moves first will continue moving first
///2. done after each turn taken by any card, fill up turn meter, most turn meter moves next
/// 
/// Speed formula = 1+(1*speed%) EX: 86 speed 1+(1*0.86) = 1+0.86 = 1.86 speed
//For 1. whoever has the highest speed # will move first, followed by second highest, etc
//For 2. will keep adding the speed each turn, whoevers speed is closer to 5 moves (cap can be changed later if i need higher speed values) 
//
//for damage, specific abilities will determine how much damage, doing something like 1x damage, 2x damage, 1.5x damage

//Damage will be subtracted from the health value, upon health reaching zero, effects might happen, might not idk ill decide later, and card dies and can no longer attack
//
//Part 1, select attack
//Part 2, select target
//Part 3, calculations to reduce health and update speeds and stat changes if I add stat boosting/dropping

//Zanes Idea:
//robot mech
//dmg 23
//spd 20
//hp 13
//attack 3: deal double dmg value to all enemies, reduces health to zero