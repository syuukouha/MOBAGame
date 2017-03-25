using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Client.Photon;

/// <summary>
/// 收到服务器响应的接受接口
/// </summary>
namespace Assets.Scripts.Receiver
{
    public interface IReceiver
    {
        /// <summary>
        /// 收到服务器的响应
        /// </summary>
        /// <param name="subCode"></param>
        /// <param name="response"></param>
        void OnReceive(byte subCode, OperationResponse response);
    }
}
