using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_a_GUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI HealthTxt, ArmorTxt, ArrowsTxt, BombsTxt, PointsTxt, CoinsTxt, SecondsLeft;

    int secleft;

    // Update is called once per frame
    void Update()
    {
        HealthTxt.text = "Health\n" + GameManager.GAME.Health;
        ArmorTxt.text = "Armor\n" + GameManager.GAME.Armor;
        ArrowsTxt.text = "Arrows\n" + GameManager.GAME.Arrows;
        BombsTxt.text = "Bombs\n" + GameManager.GAME.Bombs;
        PointsTxt.text = "Points: " + GameManager.GAME.Points;
        CoinsTxt.text = GameManager.GAME.Coins + "$";
        SecondsLeft.text = GameManager.SECONDS_LEFT.ToString();
    }
}
