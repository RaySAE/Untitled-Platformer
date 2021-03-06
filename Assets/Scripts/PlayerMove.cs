﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// THis script defines how the player moves. The designer can set the jump force, the run speed, the gravity multiplier, the fall modifier
/// the knockback and the snooze timer. The JumpForce determines how much force the player jumps with. The RunSpeed determines how fast the 
/// player can move on the X axis. The GravityMultiplier is used to determine the rate at which gravity increases when the player is falling.
/// The JumpModifier determines how fast the jump slows down as the player reaches maximum jump height. The fallModifier is used to help
/// normalise the rate at which gravity acts on a falling player. The knockback is used to determine how far the player is knocked back
/// when colliding with an enemy or trap. The direction of the knockback is always the opposite of the direction the player is facing.
/// The snooze timer is used to define how fast the player falls asleep when idle.
/// </summary>

public class PlayerMove : MonoBehaviour
{
    private GameManager Manager;
    [SerializeField] private float JumpForce = 250.0f;
    [SerializeField] private float RunSpeed = 2.0f;
    [SerializeField] private float GravityMultiplier = 2.0f;
    [SerializeField] private float JumpModifier = 1.5f;
    [SerializeField] private float FallModifier = 1.0f;
    [SerializeField] private float Knockback = 3.0f;
    [SerializeField] private float SnoozeTimer = 1.0f;
    [SerializeField] private AudioClip JumpClip;
    [SerializeField] private AudioClip LandClip;

    private int FallSpeed;
    private Rigidbody2D RigidBody;
    private CapsuleCollider2D CapsuleCollider;
    private Transform ThisTransform;

    public Animator PlayerAnimator;
    public int JumpCount = 1;
    public bool CanJump = true;
    public Transform Checkpoint;
    public AnimatorClipInfo[] ClipInfo;


    private void Awake()
    {
        Manager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        ThisTransform = this.transform;
    }

    private void OnCollisionEnter2D(Collision2D CollisionInfo)
    {
        if (CollisionInfo.gameObject.tag == "Ground")
        {
            JumpCount = 1;
            CanJump = true;

            if(FallSpeed < -20)
            {
                GetComponent<PlayerHealth>().LoseHeart();
                GetComponent<PlayerHealth>().LoseHeart();
                GetComponent<PlayerHealth>().LoseHeart();
                GetComponent<PlayerHealth>().LoseHeart();
            }

            if (FallSpeed < -15)
            {
                GetComponent<PlayerHealth>().LoseHeart();
                GetComponent<PlayerHealth>().LoseHeart();
            }

            if (FallSpeed < -10)
            {
                GetComponent<PlayerHealth>().LoseHeart();
            }

            ThisTransform.GetComponent<AudioSource>().PlayOneShot(LandClip);

            if(PlayerAnimator.GetBool("IsWalking") == false)
            {
                PlayerAnimator.SetBool("IsIdle", true);
            }

            RigidBody.velocity = Vector2.zero;
            RigidBody.angularVelocity = 0.0f;

            FallSpeed = 0;

            StartCoroutine(GoBackToSleep());
        }

        if(CollisionInfo.gameObject.tag == "Enemy")
        {
            if(CollisionInfo.transform.position.x > ThisTransform.position.x)
            {
                ThisTransform.GetComponent<Rigidbody2D>().AddForce((Vector2.left + Vector2.up) * Knockback, ForceMode2D.Impulse);
            }

            else
            {
                ThisTransform.GetComponent<Rigidbody2D>().AddForce((Vector2.right + Vector2.up) * Knockback, ForceMode2D.Impulse);
            }
        }
    }

    public void StopEverything()
    {
        StopAllCoroutines();
    }

    public IEnumerator Snooze()
    {
        yield return new WaitForSeconds(SnoozeTimer);

        PlayerAnimator.SetBool("IsIdle", false);
        PlayerAnimator.SetBool("IsSleeping", true);

        yield return null;
    }

    public IEnumerator WakeUp()
    {
        ClipInfo = PlayerAnimator.GetCurrentAnimatorClipInfo(0);

        if(ClipInfo[0].clip.name == "Snooze")
        {
            PlayerAnimator.SetBool("IsSleeping", false);
            PlayerAnimator.SetBool("IsIdle", true);
        }

        else
        {
            yield return null;
        }

        yield return null;
    }

    public IEnumerator GoBackToSleep()
    {
        //yield return new WaitForSeconds(ClipInfo.Length);
        yield return new WaitForSeconds(SnoozeTimer);

        StartCoroutine(Snooze());

        yield return null;
    }

    public void PlayerAction()
    {
        switch(ClipInfo[0].clip.name)
        {
            case "Player Attack":
                ThisTransform.GetComponent<AudioSource>().PlayOneShot(ThisTransform.GetChild(1).GetComponent<PlayerMeleeAttack>().AttackClip);
                break;

            case "Player Attack Up":
                ThisTransform.GetComponent<AudioSource>().PlayOneShot(ThisTransform.GetChild(1).GetComponent<PlayerMeleeAttack>().AttackClip);
                break;
            case "Player Jump":
                ThisTransform.GetComponent<AudioSource>().PlayOneShot(JumpClip);
                break;
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if(Time.timeScale > 0)
        {
            ClipInfo = PlayerAnimator.GetCurrentAnimatorClipInfo(0);

            #region Walk Left
            if (Input.GetKey(Manager.Keys[0]))
            {
                if (ClipInfo[0].clip.name != "Snooze" && ClipInfo[0].clip.name != "Wake Up")
                {
                    this.transform.rotation = Quaternion.Euler(0, 180, 0);

                    ThisTransform.Translate(Vector2.right * Time.deltaTime * RunSpeed, Space.Self);
                }
            }
            #endregion

            #region Walk Right
            if (Input.GetKey(Manager.Keys[1]))
            {
                //ThisTransform.GetComponent<SpriteRenderer>().flipX = true;
                if (ClipInfo[0].clip.name != "Snooze" && ClipInfo[0].clip.name != "Wake Up")
                {
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);

                    ThisTransform.Translate(Vector2.right * Time.deltaTime * RunSpeed, Space.Self);
                }
            }
            #endregion

            #region Start Walking
            if (Input.GetKeyDown(Manager.Keys[0]) || Input.GetKeyDown(Manager.Keys[1]))
            {
                PlayerAnimator.SetBool("IsIdle", false);
                PlayerAnimator.SetBool("IsWalking", true);

                StopEverything();
                StartCoroutine(WakeUp());
            }
            #endregion

            #region Stop Walking
            if (Input.GetKeyUp(Manager.Keys[0]) || Input.GetKeyUp(Manager.Keys[1]))
            {
                PlayerAnimator.SetBool("IsWalking", false);
                PlayerAnimator.SetBool("IsIdle", true);

                StopEverything();
                StartCoroutine(GoBackToSleep());
            }
            #endregion

            #region Jump
            if (Input.GetKeyDown(Manager.Keys[5]))
            {
                if (ClipInfo[0].clip.name != "Snooze" && ClipInfo[0].clip.name != "Wake Up")
                {
                    if (CanJump == true)
                    {
                        RigidBody.velocity = Vector2.up * JumpForce * Time.fixedDeltaTime;
                        JumpCount--;

                        if (JumpCount < 0)
                        {
                            CanJump = false;
                        }

                        PlayerAnimator.SetBool("IsJumping", true);
                        PlayerAnimator.SetBool("IsIdle", false);
                    }
                }

                else
                {
                    StopEverything();
                    StartCoroutine(WakeUp());
                }
            }

            if(Input.GetKeyUp(Manager.Keys[5]))
            {
                StopEverything();
                StartCoroutine(GoBackToSleep());
            }
            #endregion

            #region Jump Realism
            if (RigidBody.velocity.y < 0)
            {
                RigidBody.velocity += Vector2.up * Physics2D.gravity.y * (GravityMultiplier - FallModifier) * Time.fixedDeltaTime;
                PlayerAnimator.SetBool("IsJumping", false);
                FallSpeed = (int)RigidBody.velocity.y;
            }

            else if (RigidBody.velocity.y > 0 && !Input.GetKey(Manager.Keys[5]))
            {
                RigidBody.velocity += Vector2.up * Physics2D.gravity.y * (GravityMultiplier - JumpModifier) * Time.fixedDeltaTime;
            }
            #endregion

            #region Resize Collider
            switch(ClipInfo[0].clip.name)
            {
                case "Player Walk":
                case "Player Jump":
                case "Player Attack Up":
                case "Player Attack":
                    CapsuleCollider.direction = CapsuleDirection2D.Vertical;
                    CapsuleCollider.offset = new Vector2(0.0f, -.07f);
                    CapsuleCollider.size = new Vector2(0.5f, 1.0f);
                    break;

                case "Player Idle":
                    CapsuleCollider.direction = CapsuleDirection2D.Vertical;
                    CapsuleCollider.offset = new Vector2(0, -.32f);
                    CapsuleCollider.size = new Vector2(0.5f, 0.5f);
                    break;

                case "Snooze":
                    CapsuleCollider.direction = CapsuleDirection2D.Horizontal;
                    CapsuleCollider.offset = new Vector2(0, -.4f);
                    CapsuleCollider.size = new Vector2(0.7f, 0.25f);
                    break;
            }
            #endregion
        }
    }
}
