using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GAME;
    public static GameObject POOL;
    public static int SECONDS_LEFT;
    public static string GAME_PHASE;

    public float Health, Armor, Arrows, Bombs, Coins, Points;
    public float SwordDamage, ArrowDamage, BombDamage, RockDamge;
    public bool CountingDown;
    public GameObject arrow_prefab, bomb_prefab, rock_prefab;
    public GameObject[] floor, wall;
    public GameObject[] monsterSpritePrefab, monsterObjectPrefab, powerup;
    public List<GameObject> ArrowPool = new List<GameObject>();
    public List<GameObject> BombPool = new List<GameObject>();
    public List<GameObject> RockPool = new List<GameObject>();
    public GameObject MessagePanel, StorePanel;
    public TMPro.TextMeshProUGUI MessageText;

    public AudioSource SFX, Num10_SFX, Num9_SFX, Num8_SFX, Num7_SFX, Num6_SFX, Num5_SFX, Num4_SFX, Num3_SFX, Num2_SFX, Num1_SFX, KillEnemies_SFX, GatherLoot_SFX, BuyStuff_SFX;

    private GameObject _go_arw, _go_bmb, _go_rck;
    private int _num_per_wave = 20;
    private int _wave_variance = 5;

    public bool _takeInput = false;

    private void Awake()
    {
        GAME = this;
        POOL = GameObject.FindGameObjectWithTag("ItemPool");
        for(int _i = 0; _i < 50; _i++)
        {
            _go_arw = Instantiate(arrow_prefab, POOL.transform.position, Quaternion.identity);
            _go_bmb = Instantiate(bomb_prefab, POOL.transform.position, Quaternion.identity);
            _go_rck = Instantiate(rock_prefab, POOL.transform.position, Quaternion.identity);
            ArrowPool.Add(_go_arw);
            BombPool.Add(_go_bmb);
            RockPool.Add(_go_rck);            
        }
    }

    private void Start()
    {
        for(int _y = -10; _y < 11; _y++)
        {
            for(int _x = -10; _x < 11; _x++)
            {
                Instantiate(floor[Random.Range(0, floor.Length)], new Vector3(_x, _y, 0), Quaternion.identity);
            }
        }
        for(int _i = -11; _i < 12; _i++)
        {
            Instantiate(wall[Random.Range(0, wall.Length)], new Vector3(_i, -11, 0), Quaternion.identity);
            Instantiate(wall[Random.Range(0, wall.Length)], new Vector3(_i, 11, 0), Quaternion.identity);
            Instantiate(wall[Random.Range(0, wall.Length)], new Vector3(-11, _i, 0), Quaternion.identity);
            Instantiate(wall[Random.Range(0, wall.Length)], new Vector3(11, _i, 0), Quaternion.identity);
        }
        GAME_PHASE = "Kill Enemies";
    }

    private void Update()
    {
        if(GAME_PHASE == "Kill Enemies" && !CountingDown)
        {
            if(!SFX.isPlaying) SFX.PlayOneShot(KillEnemies_SFX.clip);
            MessagePanel.SetActive(true);
            MessageText.text = "You Have 10 Seconds to Kill The Enemies.";
            if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Space))
            {
                SECONDS_LEFT = 10;
                CountingDown = true;
                GAME_PHASE = "Get Loot";
                MessagePanel.SetActive(false);
                SFX.PlayOneShot(Num10_SFX.clip);
                StartCoroutine(CountDownTheCheatTimer());
                //SPAWN ENEMIES
                int _spawnzone, _mon, _sel = 0; GameObject _obj, _sprt; float _x = 0, _y = 0; 
                int _wave = Random.Range(_num_per_wave - _wave_variance, _num_per_wave + _wave_variance);
                for (int _i = 0; _i < _wave; _i++)
                {
                    _spawnzone = Random.Range(1, 4);
                    if (_spawnzone == 1)
                    {
                        _x = Random.Range(-10, -8);
                        _y = Random.Range(2, -3);
                    }
                    if (_spawnzone == 2)
                    {
                        _x = Random.Range(-2, 3);
                        _y = Random.Range(10, 8);
                    }
                    if (_spawnzone == 3)
                    {
                        _x = Random.Range(8.5f, 10);
                        _y = Random.Range(2, -3);
                    }
                    if (_spawnzone == 4)
                    {
                        _x = Random.Range(-2, 3);
                        _y = Random.Range(-8.5f, -10);
                    }
                    _mon = Random.Range(0, 21);
                    if (_mon < 6) _sel = 0; //Spawn a Goblin
                    if (_mon > 5 && _mon < 11) _sel = 1; //Spawn a HobGoblin
                    if (_mon > 10 && _mon < 15) _sel = 2; //Spawn an Orc
                    if (_mon > 14 && _mon < 20) _sel = 3; //Spawn a Shade
                    if (_mon > 19) _sel = 4; // Spawn a Troll
                    _obj = Instantiate(monsterObjectPrefab[_sel], new Vector3(_x, _y, 0), Quaternion.identity);
                    _sprt = Instantiate(monsterSpritePrefab[_sel], new Vector3(_x, _y, 0), Quaternion.identity);
                    _obj.GetComponent<Enemy_Logic_Controller>().MySprite = _sprt.GetComponent<Animator>();
                    _sprt.GetComponent<SpriteFollowController>().MyGameObject = _obj;
                }
            }
        }
        if (GAME_PHASE == "Get Loot" && !CountingDown)
        {
            if (!SFX.isPlaying) SFX.PlayOneShot(GatherLoot_SFX.clip);
            //Clean up last wave
            foreach (GameObject _sprites in GameObject.FindGameObjectsWithTag("Sprite")) Destroy(_sprites);
            foreach (GameObject _enemy in GameObject.FindGameObjectsWithTag("Enemy")) Destroy(_enemy);
            foreach (GameObject _MEnemy in GameObject.FindGameObjectsWithTag("Medium Enemy")) Destroy(_MEnemy);
            foreach (GameObject _HEnemy in GameObject.FindGameObjectsWithTag("Heavy Enemy")) Destroy(_HEnemy);

            //Spawn treasures
            GameObject _go = null;
            foreach (GameObject _grave in GameObject.FindGameObjectsWithTag("Grave"))
            {
                float _xOffset = 0, _yOffset ;
                if (_grave.GetComponent<Grave_Content>().num_Hearts > 0)
                {
                    _xOffset = Random.Range(-0.5f, 0.5f); _yOffset = Random.Range(-0.5f, 0.5f);
                    _go = Instantiate(powerup[0], new Vector3(_grave.transform.position.x + _xOffset, _grave.transform.position.y + _yOffset, 0), Quaternion.identity);
                    _go.GetComponent<I_am_a_powerUp>().health = _grave.GetComponent<Grave_Content>().num_Hearts;
                }
                if (_grave.GetComponent<Grave_Content>().num_Shields > 0)
                {
                    _xOffset = Random.Range(-0.5f, 0.5f); _yOffset = Random.Range(-0.5f, 0.5f);
                    _go = Instantiate(powerup[1], new Vector3(_grave.transform.position.x + _xOffset, _grave.transform.position.y + _yOffset, 0), Quaternion.identity);
                    _go.GetComponent<I_am_a_powerUp>().armor = _grave.GetComponent<Grave_Content>().num_Shields;
                }
                if (_grave.GetComponent<Grave_Content>().num_Coins > 0)
                {
                    _xOffset = Random.Range(-0.5f, 0.5f); _yOffset = Random.Range(-0.5f, 0.5f);
                    _go = Instantiate(powerup[2], new Vector3(_grave.transform.position.x + _xOffset, _grave.transform.position.y + _yOffset, 0), Quaternion.identity);
                    _go.GetComponent<I_am_a_powerUp>().gold = _grave.GetComponent<Grave_Content>().num_Coins;
                }
                if (_grave.GetComponent<Grave_Content>().num_Bags > 0)
                {
                    for (int _i = 0; _i < _grave.GetComponent<Grave_Content>().num_Bags; _i++)
                    {
                        _xOffset = Random.Range(-0.5f, 0.5f); _yOffset = Random.Range(-0.5f, 0.5f);
                        _go = Instantiate(powerup[3], new Vector3(_grave.transform.position.x + _xOffset, _grave.transform.position.y + _yOffset, 0), Quaternion.identity);
                        _go.GetComponent<I_am_a_powerUp>().points = _grave.GetComponent<Grave_Content>().bag_Scale;
                    }
                }
                if (_grave.GetComponent<Grave_Content>().num_Arrows > 0)
                {
                    _xOffset = Random.Range(-0.5f, 0.5f); _yOffset = Random.Range(-0.5f, 0.5f);
                    _go = Instantiate(powerup[4], new Vector3(_grave.transform.position.x + _xOffset, _grave.transform.position.y + _yOffset, 0), Quaternion.identity);
                    _go.GetComponent<I_am_a_powerUp>().arrows = _grave.GetComponent<Grave_Content>().num_Arrows;
                }
                if (_grave.GetComponent<Grave_Content>().num_Bombs > 0)
                {
                    _xOffset = Random.Range(-0.5f, 0.5f); _yOffset = Random.Range(-0.5f, 0.5f);
                    _go = Instantiate(powerup[5], new Vector3(_grave.transform.position.x + _xOffset, _grave.transform.position.y + _yOffset, 0), Quaternion.identity);
                    _go.GetComponent<I_am_a_powerUp>().bombs = _grave.GetComponent<Grave_Content>().num_Bombs;
                }
                Destroy(_grave);
            }

            //Message stuff
            MessagePanel.SetActive(true);
            MessageText.text = "You Have 10 Seconds to Collect Loot.";
            if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Space))
            {
                SECONDS_LEFT = 10;
                if (!CountingDown)
                {
                    //random money
                    int _r = Random.Range(3, 25);
                    for (int _i = 0; _i < _r; _i++)
                    {
                        _go = Instantiate(powerup[2], new Vector3(Random.Range(-9f, 9f), Random.Range(-9f, 9f), 0), Quaternion.identity);
                        _go.GetComponent<I_am_a_powerUp>().gold = Random.Range(1, 10);
                    }
                }
                CountingDown = true;
                GAME_PHASE = "Buy Stuff";
                MessagePanel.SetActive(false);
                SFX.PlayOneShot(Num10_SFX.clip);
                StartCoroutine(CountDownTheTimer());
            }
        }
        if (GAME_PHASE == "Buy Stuff" && !CountingDown)
        {
            if (!SFX.isPlaying) SFX.PlayOneShot(BuyStuff_SFX.clip);
            //Clean up loot
            foreach (GameObject _go in GameObject.FindGameObjectsWithTag("PowerUp")) Destroy(_go);

            SECONDS_LEFT = 10;
            MessagePanel.SetActive(true);
            MessageText.text = "You Have 10 Seconds to Buy Stuff.";
            if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Space))
            {
                CountingDown = true;
                GAME_PHASE = "Kill Enemies";
                MessagePanel.SetActive(false);
                StorePanel.SetActive(true);
                StorePanel.GetComponent<I_am_a_Store>().Selection = 0;
                SFX.PlayOneShot(Num10_SFX.clip);
                StartCoroutine(CountDownTheCheatTimer());
            }
        }
    }

    public void Countdown()
    {
        if (SECONDS_LEFT > 0)
        {
            if (GAME_PHASE != "Kill Enemies") StartCoroutine(CountDownTheTimer());
            if (GAME_PHASE == "Kill Enemies") StartCoroutine(CountDownTheCheatTimer());
            if (SECONDS_LEFT == 10) SFX.PlayOneShot(Num10_SFX.clip);
            if (SECONDS_LEFT == 9) SFX.PlayOneShot(Num9_SFX.clip);
            if (SECONDS_LEFT == 8) SFX.PlayOneShot(Num8_SFX.clip);
            if (SECONDS_LEFT == 7) SFX.PlayOneShot(Num7_SFX.clip);
            if (SECONDS_LEFT == 6) SFX.PlayOneShot(Num6_SFX.clip);
            if (SECONDS_LEFT == 5) SFX.PlayOneShot(Num5_SFX.clip);
            if (SECONDS_LEFT == 4) SFX.PlayOneShot(Num4_SFX.clip);
            if (SECONDS_LEFT == 3) SFX.PlayOneShot(Num3_SFX.clip);
            if (SECONDS_LEFT == 2) SFX.PlayOneShot(Num2_SFX.clip);
            if (SECONDS_LEFT == 1) SFX.PlayOneShot(Num1_SFX.clip);
        }
        if (SECONDS_LEFT <= 0)
        {
            CountingDown = false;
            StorePanel.SetActive(false);
            MessagePanel.SetActive(false);
        }
    }

    IEnumerator CountDownTheTimer()
    {
        yield return new WaitForSeconds(1f);
        SECONDS_LEFT = SECONDS_LEFT - 1;
        Countdown();
    }

    IEnumerator CountDownTheCheatTimer()
    {
        yield return new WaitForSeconds(1.5f);
        SECONDS_LEFT = SECONDS_LEFT - 1;
        Countdown();
    }
}
