using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_a_Store : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Timer, gold, stats;
    public GameObject[] Lever;
    public int Selection;

    public AudioSource SFX, Yes_SFX, No_SFX;

    // Update is called once per frame
    void Update()
    {
        Timer.text = GameManager.SECONDS_LEFT.ToString();
        gold.text = GameManager.GAME.Coins + "$";
        stats.text = "Health: " + GameManager.GAME.Health +
            "\nArmor: " + GameManager.GAME.Armor +
            "\nArrows: " + GameManager.GAME.Arrows +
            "\nBombs: " + GameManager.GAME.Bombs +
            "\nPoints: \n" + GameManager.GAME.Points;
        foreach (GameObject _go in Lever) _go.SetActive(false);
        Lever[Selection].SetActive(true);

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            Selection = Selection - 1;
            if (Selection < 0) Selection = Lever.Length - 1;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Selection = Selection + 1;
            if (Selection > Lever.Length - 1) Selection = 0;
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)) Select(Selection);
    }

    public void Select(int n)
    {
        if(n == 0)
        {
            if(GameManager.GAME.Coins > 14)
            {
                //Play Success Sound
                SFX.PlayOneShot(Yes_SFX.clip);
                GameManager.GAME.Coins -= 15;
                GameManager.GAME.Health += 1;
            }
            else
            {
                //Play failure sound
                SFX.PlayOneShot(No_SFX.clip);
            }
        }
        if(n == 1)
        {
            if(GameManager.GAME.Coins > 16)
            {
                //Play Success Sound
                SFX.PlayOneShot(Yes_SFX.clip);
                GameManager.GAME.Coins -= 17;
                GameManager.GAME.Armor += 1;
            }
            else
            {
                //Play failure sound
                SFX.PlayOneShot(No_SFX.clip);
            }
        }
        if(n == 2)
        {
            if(GameManager.GAME.Coins > 0)
            {
                //Play Success Sound
                SFX.PlayOneShot(Yes_SFX.clip);
                GameManager.GAME.Coins -= 1;
                GameManager.GAME.Points += 100;
            }
            else
            {
                //Play failure sound
                SFX.PlayOneShot(No_SFX.clip);
            }
        }
        if(n == 3)
        {
            if(GameManager.GAME.Coins > 19)
            {
                //Play Success Sound
                SFX.PlayOneShot(Yes_SFX.clip);
                GameManager.GAME.Coins -= 20;
                GameManager.GAME.Arrows += 1;
            }
            else
            {
                //Play failure sound
                SFX.PlayOneShot(No_SFX.clip);
            }
        }
        if(n == 4)
        {
            if(GameManager.GAME.Coins > 34)
            {
                //Play Success Sound
                SFX.PlayOneShot(Yes_SFX.clip);
                GameManager.GAME.Coins -= 35;
                GameManager.GAME.Bombs += 1;
            }
            else
            {
                //Play failure sound
                SFX.PlayOneShot(No_SFX.clip);
            }
        }
        if(n == 5)
        {
            if(GameManager.GAME.Coins > 99999)
            {
                //Win Game
                SFX.PlayOneShot(Yes_SFX.clip);
                UnityEngine.SceneManagement.SceneManager.LoadScene(3);
            }
            else
            {
                //Play failure sound
                SFX.PlayOneShot(No_SFX.clip);
            }
        }
        if(n == 6) //Exit Store
        {
            SFX.PlayOneShot(Yes_SFX.clip);
            GameManager.GAME_PHASE = "Kill Enemies";
            GameManager.GAME.CountingDown = false;
            gameObject.SetActive(false);
        }
    }
}
