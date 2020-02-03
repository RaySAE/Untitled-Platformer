﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FlyingEnemyMove : MonoBehaviour
{
    /// <summary>
    /// This script details how a flyer will move between point A and point B. If the flyer does not move then leave all variables blank.
    /// </summary>
    /// 

    private Transform ThisTransform;
    private SpriteRenderer Sprite;
    private Vector3 NextPosition;
    private int PointNumber = 0;


    [Tooltip("A boolean to show if the enemy is moving or not.")]
    [SerializeField] private bool IsMoving;
    [Tooltip("A declaration of how fast the flyer is moving. Only positive numbers.")]
    [SerializeField] private float Speed;
    [Tooltip("An array of NavPoints at which the enemy will stop.")]
    [SerializeField] private Vector3[] NavPoints;


    void Start()
    {
        ThisTransform = transform;
        Sprite = GetComponent<SpriteRenderer>();

        if (IsMoving == true)
        {
            NextPosition = NavPoints[PointNumber];
        }
    }

    private void OnCollisionEnter2D(Collision2D CollisionInfo)
    {
        if(CollisionInfo.transform.tag == "Ground")
        {
            //Reverse = !Reverse;
        }

        if(CollisionInfo.transform.tag == "Player")
        {
            CollisionInfo.transform.GetComponent<PlayerHealth>().LoseLife();
        }
    }

    void Update()
    {
        if (IsMoving == true)
        {
            if (ThisTransform.position == NavPoints[PointNumber])
            {
                PointNumber++;

                if (PointNumber == NavPoints.Length)
                {
                    PointNumber = 0;
                }

                NextPosition = NavPoints[PointNumber];

                Sprite.flipX = !Sprite.flipX;
            }

            ThisTransform.position = Vector3.MoveTowards(ThisTransform.position, NextPosition, Speed * Time.deltaTime);
        }
    }
}