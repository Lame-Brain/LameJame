using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public enum Direction { Up, UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight }
    public enum WeaponSelectionOptions { Sword, Bow, Bomb }
    public bool CanMove;
    public float Speed;
    public Animator MySprite;
    public Direction Facing;
    public float SwordDamage, ArrowDamage, BombDamage;
    public float InvincibleTime;

    public AudioSource SFX, SlashSFX, PlopSFX, ThwipSFX, ClickSFX, OuchSFX, TinkSFX;

    public float ReloadTime;
    [HideInInspector]     public WeaponSelectionOptions selectedWeapon;
    public GameObject SwordBox, ArrowBox, BombBox, QuitGameScreen;

    public float InvincibleCountdown = 0;
    float RateOfCountdownDecay = .1f;

    bool QuitMenuUp = false;

    // Start is called before the first frame update
    void Start()
    {
        selectedWeapon = WeaponSelectionOptions.Sword;
        //StartCoroutine(ReloadWeapon());
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

        if (InvincibleCountdown == 0 && GameManager.GAME.Health <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }

        if (CanMove && !GameManager.GAME_PAUSE)
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
                if (!done && selectedWeapon == WeaponSelectionOptions.Sword) { done = true; selectedWeapon = WeaponSelectionOptions.Bow; SFX.PlayOneShot(ClickSFX.clip); }
                if (!done && selectedWeapon == WeaponSelectionOptions.Bow) { done = true; selectedWeapon = WeaponSelectionOptions.Bomb; SFX.PlayOneShot(ClickSFX.clip); }
                if (!done && selectedWeapon == WeaponSelectionOptions.Bomb) { done = true; selectedWeapon = WeaponSelectionOptions.Sword; SFX.PlayOneShot(SlashSFX.clip); }
                //play sound
                StartCoroutine(ReloadWeapon());
            }
            if (Input.GetKeyUp(KeyCode.LeftControl) && selectedWeapon == WeaponSelectionOptions.Bow && ArrowBox.activeSelf && GameManager.GAME.Arrows > 0) //fire bow
            {
                int _i = 0;
                for (int _a = 0; _a < GameManager.GAME.ArrowPool.Count; _a++) if (!GameManager.GAME.ArrowPool[_a].GetComponent<Arrow_Controller>().flight) _i = _a;
                GameManager.GAME.ArrowPool[_i].transform.position = this.transform.position;
                GameManager.GAME.ArrowPool[_i].transform.rotation = this.transform.rotation;
                GameManager.GAME.ArrowPool[_i].GetComponent<Arrow_Controller>().FireArrow();
                StartCoroutine(ReloadWeapon());
                GameManager.GAME.Arrows -= 1;
                SFX.PlayOneShot(ThwipSFX.clip);
            }
            if (Input.GetKeyUp(KeyCode.LeftControl) && selectedWeapon == WeaponSelectionOptions.Bomb && BombBox.activeSelf && GameManager.GAME.Bombs > 0) //fire bomb
            {
                int _i = 0;
                for (int _a = 0; _a < GameManager.GAME.BombPool.Count; _a++) //Debug.Log(_a + ". bomb is armed? " + GameManager.GAME.BombPool[_a].GetComponent<Bomb_Controller>().armed);
                    if (!GameManager.GAME.BombPool[_a].GetComponent<Bomb_Controller>().armed) _i = _a;
                GameManager.GAME.BombPool[_i].transform.position = this.transform.position;
                GameManager.GAME.BombPool[_i].transform.rotation = this.transform.rotation;
                GameManager.GAME.BombPool[_i].transform.Translate(Vector2.down * .8f); 
                GameManager.GAME.BombPool[_i].GetComponent<Bomb_Controller>().Arm_Bomb();
                StartCoroutine(ReloadWeapon());
                GameManager.GAME.Bombs -= 1;
                SFX.PlayOneShot(PlopSFX.clip);
            }
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                //QuitMenuUp = true;
                //QuitGameScreen.SetActive(true);
                //GameManager.GAME_PAUSE = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }
        /*if (QuitMenuUp && Input.GetKeyUp(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        else if (QuitMenuUp && Input.anyKeyDown)
        {
            QuitMenuUp = false;
            QuitGameScreen.SetActive(false);
            GameManager.GAME_PAUSE = false;
        }*/
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
            if (InvincibleCountdown == 0) DamageHealth(GameManager.GAME.RockDamge);
            collision.collider.gameObject.GetComponent<I_am_a_Rock>().flight = false;
            collision.collider.gameObject.GetComponent<I_am_a_Rock>().Stop_Rock();
            InvincibleCountdown = InvincibleTime;
            Vector3 dir = collision.collider.transform.position - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody2D>().AddForce(dir * 100, ForceMode2D.Impulse);
        }
        if(collision.collider.gameObject.tag == "Enemy")
        {
            if (InvincibleCountdown == 0) DamageHealth(collision.collider.gameObject.GetComponent<Enemy_Logic_Controller>().Damage);
            InvincibleCountdown = InvincibleTime;
            Vector3 dir = collision.collider.transform.position - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody2D>().AddForce(dir * 200, ForceMode2D.Impulse);
        }
        if(collision.collider.gameObject.tag == "Medium Enemy")
        {
            if (InvincibleCountdown == 0) DamageHealth(collision.collider.gameObject.GetComponent<Enemy_Logic_Controller>().Damage);
            InvincibleCountdown = InvincibleTime;
            Vector3 dir = collision.collider.transform.position - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody2D>().AddForce(dir * 800, ForceMode2D.Impulse);
        }
        if(collision.collider.gameObject.tag == "Heavy Enemy")
        {
            if (InvincibleCountdown == 0) DamageHealth(collision.collider.gameObject.GetComponent<Enemy_Logic_Controller>().Damage);
            InvincibleCountdown = InvincibleTime;
            Vector3 dir = collision.collider.transform.position - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody2D>().AddForce(dir * 1000, ForceMode2D.Impulse);
        }
        if (collision.collider.gameObject.tag == "Explosion")
        {
            if (InvincibleCountdown == 0) DamageHealth(GameManager.GAME.BombDamage);
            InvincibleCountdown = InvincibleTime;
            Vector3 dir = collision.collider.transform.position - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody2D>().AddForce(dir * 3000, ForceMode2D.Impulse);
        }

    }

    public void DamageHealth(float damage)
    {
        float _armor = GameManager.GAME.Armor;
        if (GameManager.GAME.Armor > 0)
        {
            //Play Armor tink sound
            SFX.PlayOneShot(TinkSFX.clip);
            GameManager.GAME.Armor -= damage;
            if (GameManager.GAME.Armor < 0) GameManager.GAME.Armor = 0;
            damage = damage - _armor;
            if (damage < 0) damage = 0;
        }
        else
        {
            //Play Hurt sound
            SFX.PlayOneShot(OuchSFX.clip);
        }
        GameManager.GAME.Health = GameManager.GAME.Health - damage;
    }
}
