using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_a_Wall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Arrow")
        {
            collision.gameObject.GetComponent<Arrow_Controller>().StopArrow();
        }
    }
}
