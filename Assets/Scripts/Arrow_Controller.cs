using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    public bool flight;

    public void FireArrow()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * 20, ForceMode2D.Impulse);
        flight = true;
    }

    public void StopArrow()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * -20, ForceMode2D.Impulse);
        flight = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HIT " + collision.gameObject.name);
    }
}
