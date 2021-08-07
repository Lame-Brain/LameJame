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
    }

    public void Disarm_Bomb()
    {
        armed = false;
    }
}
