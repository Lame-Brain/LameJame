using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Controller : MonoBehaviour
{
    public bool armed;
    public GameObject Boom_prefab;

    public void Arm_Bomb()
    {
        armed = true;
        StartCoroutine(TickingTimeBomb());
    }

    public void Disarm_Bomb()
    {
        armed = false;
        gameObject.transform.position = GameManager.POOL.transform.position;
    }

    IEnumerator TickingTimeBomb()
    {
        yield return new WaitForSeconds(1f);
        if(armed) Instantiate(Boom_prefab, transform.position, Quaternion.identity);
        armed = false;
        gameObject.transform.position = GameManager.POOL.transform.position;
        //Play Boom Sound
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Sword" || collision.collider.gameObject.tag == "Arrow" || collision.collider.gameObject.tag == "Explosion") 
        {
            if (armed)
            {
                Instantiate(Boom_prefab, transform.position, Quaternion.identity);
                armed = false;
                gameObject.transform.position = GameManager.POOL.transform.position;
                //Play Boom Sound
            }
        }
    }
}
