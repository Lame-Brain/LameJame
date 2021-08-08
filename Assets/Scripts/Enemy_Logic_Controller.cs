using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Logic_Controller : MonoBehaviour
{
    public enum MonsterType { Soldier, Archer, Charger}
    public MonsterType Type;
    public bool canMove;
    public float InvincibleTime;
    public Animator MySprite;
    public float Health;
    public float Speed;
    public float Damage;
    public GameObject Grave_Prefab, Pop_Prefab;

    public int min_hearts, max_hearts, min_shields, max_shields, min_coins, max_coins, min_bags, max_bags, min_points, max_points, min_Arrows, max_Arrows, min_Bombs, max_Bombs;

    float InvincibleCountdown = 0;
    float RateOfCountdownDecay = .1f;
    bool inRange;
    bool targeting, charging, winding;
    Transform _Target, _storedTarget;


    // Start is called before the first frame update
    void Start()
    {
        _Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (InvincibleCountdown > 0)
        {
            InvincibleCountdown = InvincibleCountdown - RateOfCountdownDecay;
            MySprite.SetBool("Flashing", true);

            if (InvincibleCountdown <= 0) { MySprite.SetBool("Flashing", false); InvincibleCountdown = 0; }
        }

        if(InvincibleCountdown == 0 && Health <= 0)
        {
            GameObject _go;
            int _hrts = Random.Range(min_hearts, max_hearts); if (_hrts < 0) _hrts = 0;
            int _shlds = Random.Range(min_shields, max_shields); if (_shlds < 0) _shlds = 0;
            int _coins = Random.Range(min_coins, max_coins); if (_coins < 0) _coins = 0;
            int _bags = Random.Range(min_bags, max_bags); if (_bags < 0) _bags = 0;
            int _scale = Random.Range(min_points, max_points); if (_scale < 0) _scale = 0;
            int _arrws = Random.Range(min_Arrows, max_Arrows); if (_arrws < 0) _arrws = 0;
            int _bmbs = Random.Range(min_Bombs, max_Bombs); if (_bmbs < 0) _bmbs = 0;
            Instantiate(Pop_Prefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            _go = Instantiate(Grave_Prefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            _go.GetComponent<Grave_Content>().num_Hearts = _hrts;
            _go.GetComponent<Grave_Content>().num_Shields = _shlds;
            _go.GetComponent<Grave_Content>().num_Coins = _coins;
            _go.GetComponent<Grave_Content>().num_Bags = _bags;
            _go.GetComponent<Grave_Content>().bag_Scale = _scale;
            _go.GetComponent<Grave_Content>().num_Arrows = _arrws;
            _go.GetComponent<Grave_Content>().num_Bombs = _bmbs;
            Destroy(MySprite.gameObject);
            Destroy(this.gameObject);
        }

        if (canMove) //MOVEMENT
        {
            if (!inRange || Type == MonsterType.Soldier) //Soldiers or Enemies out of range
            {
                transform.up = _Target.position - transform.position; //face player with y axis
                transform.Translate(Vector2.up * Speed * Time.deltaTime); //move along y axis              
            }
            if(inRange && Type == MonsterType.Charger && !charging && !targeting) //Chargers stop at range and pause before charging
            {
                transform.up = _Target.position - transform.position; //face player with y axis
                targeting = true;
                StartCoroutine(PauseBeforeCharging());
            }
            if(inRange && Type == MonsterType.Charger && charging) //...Then they charge!
            {
                StartCoroutine(Charge());
            }
            if(inRange && Type == MonsterType.Archer && !winding) //Archer fires his shot, using targeting to space shots out.
            {
                winding = true;
                transform.up = _Target.position - transform.position; //face player with y axis
                transform.Translate(Vector2.up * Speed * Time.deltaTime);
                int _i = 0;
                for (int _a = 0; _a < GameManager.GAME.RockPool.Count; _a++) if (!GameManager.GAME.RockPool[_a].GetComponent<I_am_a_Rock>().flight) _i = _a;
                GameManager.GAME.RockPool[_i].transform.position = this.transform.position;
                GameManager.GAME.RockPool[_i].transform.rotation = this.transform.rotation;
                GameManager.GAME.RockPool[_i].GetComponent<I_am_a_Rock>().Throw_Rock();
                StartCoroutine(DelayBeforeThrowingAnotherRock());   
            }
            if(inRange && Type == MonsterType.Archer && winding)
            {
                transform.up = _Target.position - transform.position; //face player with y axis
                transform.Translate(Vector2.right * Speed * 2 * Random.Range(-1, 1) * Time.deltaTime); //move sideways
                transform.Translate(Vector2.down * Speed * 2 * Time.deltaTime); //move backwards
            }
        }
    }
    IEnumerator PauseBeforeCharging()
    {
        yield return new WaitForSeconds(1f);
        charging = true;
        targeting = false;
        _storedTarget = _Target;
    }
    IEnumerator Charge()
    {
        transform.Translate(Vector2.up * (Speed * 2.5f) * Time.deltaTime); //move along y axis at double speed
        yield return new WaitForSeconds(1f);
        charging = false;
    }
    IEnumerator DelayBeforeThrowingAnotherRock()
    {
        yield return new WaitForSeconds(2.5f);
        winding = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name == "Sword" && InvincibleCountdown == 0) //Hit by Sword
        {
            InvincibleCountdown = InvincibleTime;
            Vector3 dir = collision.collider.transform.position - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody2D>().AddForce(dir * 150, ForceMode2D.Impulse);
            Health = Health - GameManager.GAME.SwordDamage;
        }
        if (collision.collider.gameObject.tag == "Arrow" && InvincibleCountdown == 0) //hit by Arrow
        {
            InvincibleCountdown = InvincibleTime;
            Vector3 dir = collision.collider.transform.position - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody2D>().AddForce(dir * 50, ForceMode2D.Impulse);
            Health = Health - GameManager.GAME.ArrowDamage;
            collision.gameObject.GetComponent<Arrow_Controller>().StopArrow();
        }
        if (collision.collider.gameObject.tag == "Explosion" && InvincibleCountdown == 0) //hit by bomb
        {
            InvincibleCountdown = InvincibleTime;
            Vector3 dir = collision.collider.transform.position - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody2D>().AddForce(dir * 300, ForceMode2D.Impulse);
            Health = Health - GameManager.GAME.ArrowDamage;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().gameObject.name == "Range")
        {
            inRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.GetComponent<Collider2D>().gameObject.name == "Range")
        {
            inRange = false; targeting = false; charging = false;
        }
    }
}
