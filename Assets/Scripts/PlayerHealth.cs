﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Image[] Health;
    [SerializeField] public static int LivesRemaining;

    void Start()
    {
        LivesRemaining = Health.Length - 1;
    }

    public void GainLife()
    {
        if(LivesRemaining != (Health.Length - 1))
        {
            LivesRemaining++;
            Health[LivesRemaining].gameObject.SetActive(true);
        }
    }

    public void LoseLife()
    {
        if(LivesRemaining > -1)
        {
            Health[LivesRemaining].gameObject.SetActive(false);
            LivesRemaining--;
        }

        if(LivesRemaining == -1)
        {
            ScoreManager.SaveScores();
            StartCoroutine(DeathAnimation());
        }
    }

    IEnumerator DeathAnimation()
    {
        GetComponent<PlayerMove>().PlayerAnimator.SetBool("IsDead", true);
        yield return new WaitForSeconds(GetComponent<PlayerMove>().PlayerAnimator.GetCurrentAnimatorStateInfo(0).length);
        GetComponent<PlayerMove>().PlayerAnimator.SetBool("IsDead", false);

        this.transform.position = this.GetComponent<PlayerMove>().Checkpoint.position;

        this.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        this.transform.rotation = Quaternion.identity;

        GetComponent<PlayerMove>().CanJump = true;
        GetComponent<PlayerMove>().JumpCount = 1;

        for (int i = 0; i < Health.Length; i++)
        {
            GainLife();
        }

        yield return null;
    }
}
