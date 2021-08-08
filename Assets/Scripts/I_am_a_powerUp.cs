using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_a_powerUp : MonoBehaviour
{
    public int health, armor, gold, points, arrows, bombs;
    public GameObject pop_prefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Play powerup sound

        GameManager.GAME.Health += health;
        GameManager.GAME.Armor += armor;
        GameManager.GAME.Coins += (gold * 50);
        GameManager.GAME.Points += points;
        GameManager.GAME.Arrows += arrows;
        GameManager.GAME.Bombs += bombs;
        Instantiate(pop_prefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
