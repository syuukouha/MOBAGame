using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MOBACommon.Dto;
using UnityEngine.AI;
/// <summary>
/// 所有战斗模型的控制基类
/// </summary>
public class BaseController : MonoBehaviour
{
    /// <summary>
    /// 此物体的数据模型
    /// </summary>
    #region 数据
    public SoldierModel Model { get; set; }
    [SerializeField]
    protected AnimationConroller animationController;
    protected HpController hpController;
    protected NavMeshAgent agent;
    public void Init(SoldierModel model,bool isFriendly)
    {
        this.Model = model;
        hpController.SetColor(isFriendly);
        //根据isFriendly 设置Tag
        gameObject.tag = isFriendly ? "Friend" : "Enemy";
    }

    #endregion
    #region 动画
    #endregion

    #region 血量
    /// <summary>
    /// 血量改变
    /// </summary>
    public void OnHPChange()
    {
        hpController.SetHP((float)Model.CurrentHP / Model.MaxHP);
    }
    #endregion

    #region 移动控制
    protected bool IsMoving
    {
        get { return agent.pathPending || agent.remainingDistance > agent.stoppingDistance || agent.velocity != Vector3.zero || agent.pathStatus != NavMeshPathStatus.PathComplete; }
    }
    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="point"></param>
    public void Move(Vector3 point)
    {
        point.y = transform.position.y;
        //寻路
        agent.ResetPath();
        agent.SetDestination(point);
        //播放动画
        animationController.Walk();
        animationController.animState = AnimationConroller.AnimState.WALK;
    }
    #endregion

    #region 攻击控制
    //选择人物，给服务器发送一个要攻击的ID，在服务器计算伤害，然后同步攻击
    /// <summary>
    /// 攻击请求
    /// </summary>
    public virtual void RequestAttack() { }
    /// <summary>
    /// 攻击响应
    /// </summary>
    public virtual void ResponseAttack(Transform[] target) { }
    /// <summary>
    /// 死亡
    /// </summary>
    public virtual void ResponseDeath() { }
    #endregion

    void Update()
    {
        //检测寻路是否终止
        if(animationController.animState == AnimationConroller.AnimState.WALK && !IsMoving)
        {
            animationController.Free();
        }
    }
}
