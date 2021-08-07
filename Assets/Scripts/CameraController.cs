using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool FollowHero;
    [HideInInspector] public Transform Hero;


    private void Start()
    {
        Hero = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (FollowHero)
        {
            this.transform.position = new Vector3(Hero.position.x, Hero.position.y, this.transform.position.z);
        }
    }
}
