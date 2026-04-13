using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Timeline;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Orc_Script : MonoBehaviour
{
    public Sprite attack_1_sprite;
    public Sprite attack_2_sprite;
    public Sprite attack_3_sprite;
    private int attack_2_cooldown = 0;
    private int attack_3_cooldown = 0;
    private Canvas canvas;

    GameManager GM;
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
    }


    void attack_1()
    {
        
    }
    void attack_2()
    {
        if (attack_2_cooldown <= 0)
        {
            attack_2_cooldown = 3;
        }
    }
    void attack_3()
    {
        if (attack_3_cooldown <= 0)
        {
            attack_3_cooldown = 5;
        }
    }
}