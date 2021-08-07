using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFollowController : MonoBehaviour
{
    public GameObject MyGameObject;

    void Update()
    {
        this.transform.position = MyGameObject.transform.position;
    }
}
