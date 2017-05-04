using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 动画控制
/// </summary>
public class AnimationConroller : MonoBehaviour {
    public enum AnimState
    {
        FREE,
        WALK,
        ATTACK,
        DEATH
    }
    public AnimState animState = AnimState.FREE;

    private Animator animator;

    /// <summary>
    /// 播放闲置动画
    /// </summary>
    public void Free()
    {
        animator.SetBool("WALK", false);
        animState = AnimState.FREE;
    }
    /// <summary>
    /// 播放攻击动画
    /// </summary>
    public void Attack()
    {
        animator.SetBool("WALK", false);
        animator.SetTrigger("ATTACK");
        animState = AnimState.ATTACK;

    }
    /// <summary>
    /// 播放行走动画
    /// </summary>
    public void Walk()
    {
        animator.SetBool("WALK", true);
        animState = AnimState.WALK;


    }    /// <summary>
         /// 播放死亡动画
         /// </summary>
    public void Death()
    {
        animator.SetBool("WALK", false);
        animator.SetTrigger("DEATH");
        animState = AnimState.DEATH;

    }
    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
