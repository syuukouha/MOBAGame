using System.Collections;
using System.Collections.Generic;
using MOBACommon.Dto;
using UnityEngine;
using UnityEngine.UI;

public class AddFriendRequestView : MonoBehaviour
{
    [SerializeField]
    private Text infoText;

    public int ID;
    public void UpdateView(PlayerDto playerDto)
    {
        this.ID = playerDto.ID;
        infoText.text = playerDto.Name + "  " + "等级:" + playerDto.Lv;
    }
}
