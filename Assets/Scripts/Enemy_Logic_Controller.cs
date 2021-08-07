using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Logic_Controller : MonoBehaviour
{
    public float InvincibleTime;
    public Animator MySprite;
    public float Health;
    public float Speed;
    public float Damage;
    public GameObject Grave_Prefab, Pop_Prefab;

    public int min_hearts, max_hearts, min_shields, max_shields, min_coins, max_coins, min_bags, max_bags, min_points, max_points, min_Arrows, max_Arrows, min_Bombs, max_Bombs;

    float InvincibleCountdown = 0;
    float RateOfCountdownDecay = .1f;



    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name == "Sword" && InvincibleCountdown == 0)
        {
            InvincibleCountdown = InvincibleTime;
            Vector3 dir = collision.collider.transform.position - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody2D>().AddForce(dir * 150, ForceMode2D.Impulse);
            Health = Health - GameManager.GAME.SwordDamage;
        }
        if (collision.collider.gameObject.tag == "Arrow" && InvincibleCountdown == 0)
        {
            Debug.Log("HIT");
            InvincibleCountdown = InvincibleTime;
            Vector3 dir = collision.collider.transform.position - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody2D>().AddForce(dir * 50, ForceMode2D.Impulse);
            Health = Health - GameManager.GAME.ArrowDamage;
            collision.gameObject.GetComponent<Arrow_Controller>().StopArrow();
        }
    }
}
