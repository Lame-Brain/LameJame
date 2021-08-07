using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GAME;
    public static GameObject POOL;

    public float SwordDamage, ArrowDamage, BombDamage;
    public GameObject arrow_prefab, bomb_prefab, rock_prefab;
    public GameObject[] floor, wall;
    public List<GameObject> ArrowPool = new List<GameObject>();
    public List<GameObject> BombPool = new List<GameObject>();
    public List<GameObject> RockPool = new List<GameObject>();

    private GameObject _go_arw, _go_bmb, _go_rck;

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
    }
}
