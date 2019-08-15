﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller2 : MonoBehaviour
{

    Rigidbody2D Rigid;
    public float Movement_Speed = 10f;
    private float Movement = 0;
    private int Direction = 1;
    private Vector3 Player_LocalScale;

    public Sprite[] Spr_Player = new Sprite[2];

    private float leftBorder;       //游戏左边界x
    private float rightBorder;      //游戏右边界x

    public Camera MainCamera2;

    // Use this for initialization
    void Start()
    {
        Rigid = GetComponent<Rigidbody2D>();
        Player_LocalScale = transform.localScale;
        Debug.Log(transform.localPosition);
        leftBorder = MainCamera2.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        rightBorder = MainCamera2.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        //Debug.Log(leftBorder);
        //Debug.Log(rightBorder);
    }

    // Update is called once per frame
    //   void Update () 
    //   {
    //       // Set Movement value
    //       Movement = Input.acceleration.x * Movement_Speed; //Input.GetAxis("Horizontal") * Movement_Speed; //Input.acceleration.x * Movement_Speed;

    //       // Player look right or left
    //       if (Movement > 0)
    //           transform.localScale = new Vector3(Player_LocalScale.x, Player_LocalScale.y, Player_LocalScale.z);
    //       else if (Movement < 0)
    //           transform.localScale = new Vector3(-Player_LocalScale.x, Player_LocalScale.y, Player_LocalScale.z);
    //}

    void Update()
    {
       
        Vector3 acc = Vector3.zero;
        Vector3 diff;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            acc.x = -0.1f;
            transform.localScale = new Vector3(-Player_LocalScale.x, Player_LocalScale.y, Player_LocalScale.z);
        }
        if ( Input.GetKey(KeyCode.RightArrow))
        {
            acc.x = 0.1f;
            transform.localScale = new Vector3(Player_LocalScale.x, Player_LocalScale.y, Player_LocalScale.z);
        }

        diff = Vector3.MoveTowards(transform.localPosition, transform.localPosition + acc, 5f * Time.deltaTime);

 

        if (diff.x < leftBorder)
        {
            diff.x = rightBorder;
        }
        else if (diff.x > rightBorder)
        {
            diff.x = leftBorder;
        }
        transform.localPosition = diff;
    }

    void FixedUpdate()
    {
        
        // Calculate player velocity
        Vector2 Velocity = Rigid.velocity;
        Velocity.x = Movement;
        Rigid.velocity = Velocity;

        // Player change sprite
        if (Velocity.y <= 0)
        {
            GetComponent<SpriteRenderer>().sprite = Spr_Player[0];

            // Active player collider
            GetComponent<BoxCollider2D>().enabled = true;

            // Fall propeller after fly up
            Propeller_Fall();
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = Spr_Player[1];

            // Deactive player collider
            GetComponent<BoxCollider2D>().enabled = false;
        }

     

        // Player wrap
        Vector3 Top_Left = MainCamera2.ViewportToWorldPoint(new Vector3(0, 0, 0));
        float Offset = 0.5f;

     

        if (transform.position.x > rightBorder + Offset)
            transform.position = new Vector3(leftBorder - Offset, transform.position.y, transform.position.z);
        else if (transform.position.x < leftBorder - Offset)
            transform.position = new Vector3(rightBorder + Offset, transform.position.y, transform.position.z);

    

    }

    void Propeller_Fall()
    {
        if (transform.childCount > 0)
        {
            transform.GetChild(0).GetComponent<Animator>().SetBool("Active", false);
            transform.GetChild(0).GetComponent<Propeller2>().Set_Fall(gameObject);
            transform.GetChild(0).parent = null;
        }
    }
}
