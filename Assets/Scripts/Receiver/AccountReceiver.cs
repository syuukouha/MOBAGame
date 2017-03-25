using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Client.Photon;
using MOBACommon.Codes;
using UnityEngine;

namespace Assets.Scripts.Receiver
{
    public class AccountReceiver:MonoBehaviour,IReceiver
    {
        public void OnReceive(byte subCode, OperationResponse response)
        {
            switch (subCode)
            {
                case OpAccount.Login:
                    OnLogin(response.ReturnCode,response.DebugMessage);
                    break;
                case OpAccount.Register:
                    OnRegister(response.ReturnCode,response.DebugMessage);
                    break;
            }
        }
        /// <summary>
        /// 登录处理
        /// </summary>
        /// <param name="returnCode"></param>
        private void OnLogin(short returnCode,string message)
        {
            switch (returnCode)
            {
                case 0:
                    //成功
                    UIManager.Instance.HideUI(Paths.RES_UIAccount);
                    UIManager.Instance.ShowUI(Paths.RES_UIMain);
                    break;
                case -1:
                    //玩家在线
                    Tips.Instance.Show(message,null);
                    GetComponent<AccountView>()._loginButton.interactable = true;
                    GetComponent<AccountView>().ClearText();
                    break;
                case -2:
                    //账号密码错误
                    Tips.Instance.Show(message, null);
                    GetComponent<AccountView>()._loginButton.interactable = true;
                    GetComponent<AccountView>().ClearText();
                    break;
            }
        }
        /// <summary>
        /// 注册处理
        /// </summary>
        /// <param name="returnCode"></param>
        private void OnRegister(short returnCode,string message)
        {
            switch (returnCode)
            {
                case 0:
                    //成功
                    Tips.Instance.Show(message, null);              
                    break;
                case -1:
                    //账号重复
                    Tips.Instance.Show(message, null);
                    break;
            }
        }
    }
}
