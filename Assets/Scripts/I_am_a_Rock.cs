using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_a_Rock : MonoBehaviour
{
    public bool flight;
     float _speed = 5;

    // Update is called once per frame
    void Update()
    {
        if (flight)
        {
            transform.Translate(Vector2.up * _speed * Time.deltaTime); //move along y axis 
        }
    }

    public void Throw_Rock()
    {
        flight = true;
    }

    public void Stop_Rock()
    {
        flight = false;
        gameObject.transform.position = GameManager.POOL.transform.position;
    }

    IEnumerator FlightTime()
    {
        yield return new WaitForSeconds(2.5f);
        Stop_Rock();
    }
}
