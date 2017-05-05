using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 血条控制
/// </summary>
public class HpController : MonoBehaviour {
    [SerializeField]
    private Slider hpBar;
    [SerializeField]
    private Image hpBarImage;
    /// <summary>
    /// 设置HPBar颜色
    /// </summary>
    /// <param name="isFriendly"></param>
    public void SetColor(bool isFriendly)
    {
        hpBarImage.color = isFriendly ? Color.green : Color.red;
    }
    /// <summary>
    /// 设置血量
    /// </summary>
    /// <param name="value">血量的百分比</param>
    public void SetHP(float value)
    {
        hpBar.value = value;
    }
    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
