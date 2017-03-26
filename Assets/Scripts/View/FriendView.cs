using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendView : MonoBehaviour
{
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text stateText;

    public int ID;
    /// <summary>
    /// 初始化显示
    /// </summary>
    /// <param name="name"></param>
    /// <param name="state"></param>
    public void InitView(int id,string name, bool isOnline)
    {
        this.ID = id;
        nameText.text = name;
        stateText.text = isOnline ? "状态:在线" : "状态:离线";
    }
    public void UpdateView(bool isOnline)
    {
        stateText.text = isOnline ? "状态:在线" : "状态:离线";
    }
}
