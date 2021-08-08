using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public enum Direction { Up, UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight }
    public enum WeaponSelectionOptions { Sword, Bow, Bomb }
    public bool CanMove;
    public float Health, Armor;
    public float Speed;
    public Animator MySprite;
    public Direction Facing;
    public float SwordDamage, ArrowDamage, BombDamage;
    public float InvincibleTime;

    public float ReloadTime;
    [HideInInspector]     public WeaponSelectionOptions selectedWeapon;
    public GameObject SwordBox, ArrowBox, BombBox;

    public float InvincibleCountdown = 0;
    float RateOfCountdownDecay = .1f;


    // Start is called before the first frame update
    void Start()
    {
        selectedWeapon = WeaponSelectionOptions.Sword;
        StartCoroutine(ReloadWeapon());
    }

    // Update is called once per frame
    void Update()
    {
        if (InvincibleCountdown > 0)
        {
            InvincibleCountdown = InvincibleCountdown - RateOfCountdownDecay;
            MySprite.SetBool("Flash", true);

            if (InvincibleCountdown <= 0) { MySprite.SetBool("Flash", false); InvincibleCountdown = 0; }
        }

        if (InvincibleCountdown == 0 && Health <= 0)
        {
            //GAME OVER
        }

            if (CanMove)
        {
            if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                transform.Translate(Vector2.up * Speed * Time.deltaTime);
                Facing = Direction.Up;
                MySprite.SetBool("Walking", true);
            }
            if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                transform.eulerAngles = new Vector3(0, 0, 45);
                transform.Translate(Vector2.up * (Speed * Time.deltaTime));
                Facing = Direction.UpLeft;
                MySprite.SetBool("Walking", true);
            }
            if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                transform.eulerAngles = new Vector3(0, 0, 315);
                transform.Translate(Vector2.up * (Speed * Time.deltaTime));
                Facing = Direction.UpRight;
                MySprite.SetBool("Walking", true);
            }
            if (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                transform.eulerAngles = new Vector3(0, 0, 180);
                transform.Translate(Vector2.up * (Speed * Time.deltaTime));
                Facing = Direction.Down;
                MySprite.SetBool("Walking", true);
            }
            if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                transform.eulerAngles = new Vector3(0, 0, 135);
                transform.Translate(Vector2.up * (Speed * Time.deltaTime));
                Facing = Direction.DownLeft;
                MySprite.SetBool("Walking", true);
            }
            if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                transform.eulerAngles = new Vector3(0, 0, 225);
                transform.Translate(Vector2.up * (Speed * Time.deltaTime));
                Facing = Direction.DownRight;
                MySprite.SetBool("Walking", true);
            }
            if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
            {
                transform.eulerAngles = new Vector3(0, 0, 90);
                transform.Translate(Vector2.up * (Speed * Time.deltaTime));
                Facing = Direction.Left;
                MySprite.SetBool("Walking", true);
            }
            if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
            {
                transform.eulerAngles = new Vector3(0, 0, 270);
                transform.Translate(Vector2.up * (Speed * Time.deltaTime));
                Facing = Direction.Right;
                MySprite.SetBool("Walking", true);
            }
            if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                MySprite.SetBool("Walking", false);
            }
            if (Input.GetKeyUp(KeyCode.LeftAlt)) //Weapon Switch button
            {
                bool done = false;
                if (!done && selectedWeapon == WeaponSelectionOptions.Sword) { done = true; selectedWeapon = WeaponSelectionOptions.Bow; }
                if (!done && selectedWeapon == WeaponSelectionOptions.Bow) { done = true; selectedWeapon = WeaponSelectionOptions.Bomb; }
                if (!done && selectedWeapon == WeaponSelectionOptions.Bomb) { done = true; selectedWeapon = WeaponSelectionOptions.Sword; }
                //play sound
                StartCoroutine(ReloadWeapon());
            }
            if (Input.GetKeyUp(KeyCode.LeftControl) && selectedWeapon == WeaponSelectionOptions.Bow && ArrowBox.activeSelf) //fire bow
            {
                int _i = 0;
                for (int _a = 0; _a < GameManager.GAME.ArrowPool.Count; _a++) if (!GameManager.GAME.ArrowPool[_a].GetComponent<Arrow_Controller>().flight) _i = _a;
                GameManager.GAME.ArrowPool[_i].transform.position = this.transform.position;
                GameManager.GAME.ArrowPool[_i].transform.rotation = this.transform.rotation;
                GameManager.GAME.ArrowPool[_i].GetComponent<Arrow_Controller>().FireArrow();
                StartCoroutine(ReloadWeapon());
            }
            if (Input.GetKeyUp(KeyCode.LeftControl) && selectedWeapon == WeaponSelectionOptions.Bomb && BombBox.activeSelf) //fire bomb
            {
                int _i = 0;
                for (int _a = 0; _a < GameManager.GAME.BombPool.Count; _a++) //Debug.Log(_a + ". bomb is armed? " + GameManager.GAME.BombPool[_a].GetComponent<Bomb_Controller>().armed);
                    if (!GameManager.GAME.BombPool[_a].GetComponent<Bomb_Controller>().armed) _i = _a;
                GameManager.GAME.BombPool[_i].transform.position = this.transform.position;
                GameManager.GAME.BombPool[_i].transform.rotation = this.transform.rotation;
                GameManager.GAME.BombPool[_i].transform.Translate(Vector2.down * .8f); 
                GameManager.GAME.BombPool[_i].GetComponent<Bomb_Controller>().Arm_Bomb();
                StartCoroutine(ReloadWeapon());
            }
        }
    }

    IEnumerator ReloadWeapon()
    {
        this.SwordBox.SetActive(false);
        this.ArrowBox.SetActive(false);
        this.BombBox.SetActive(false);
        yield return new WaitForSeconds(ReloadTime);

        if (selectedWeapon == WeaponSelectionOptions.Sword) this.SwordBox.SetActive(true);
        if (selectedWeapon == WeaponSelectionOptions.Bow) this.ArrowBox.SetActive(true);
        if (selectedWeapon == WeaponSelectionOptions.Bomb) this.BombBox.SetActive(true);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Rock")
        {
            if (InvincibleCountdown == 0) Health = Health - GameManager.GAME.RockDamge;
            collision.collider.gameObject.GetComponent<I_am_a_Rock>().flight = false;
            collision.collider.gameObject.GetComponent<I_am_a_Rock>().Stop_Rock();
            InvincibleCountdown = InvincibleTime;
            Vector3 dir = collision.collider.transform.position - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody2D>().AddForce(dir * 100, ForceMode2D.Impulse);
        }
        if(collision.collider.gameObject.tag == "Enemy")
        {
            if (InvincibleCountdown == 0) Health = Health - 2;
            InvincibleCountdown = InvincibleTime;
            Vector3 dir = collision.collider.transform.position - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody2D>().AddForce(dir * 200, ForceMode2D.Impulse);
        }
        if(collision.collider.gameObject.tag == "Medium Enemy")
        {
            if (InvincibleCountdown == 0) Health = Health - 4;
            InvincibleCountdown = InvincibleTime;
            Vector3 dir = collision.collider.transform.position - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody2D>().AddForce(dir * 800, ForceMode2D.Impulse);
        }
        if(collision.collider.gameObject.tag == "Heavy Enemy")
        {
            if (InvincibleCountdown == 0) Health = Health - 8;
            InvincibleCountdown = InvincibleTime;
            Vector3 dir = collision.collider.transform.position - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody2D>().AddForce(dir * 1000, ForceMode2D.Impulse);
        }
    }
}
