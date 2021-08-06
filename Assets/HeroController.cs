using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public enum Direction { Up, UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight }
    public bool CanMove;
    public float Speed;
    public Animator MySprite;
    public Direction Facing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }
}
