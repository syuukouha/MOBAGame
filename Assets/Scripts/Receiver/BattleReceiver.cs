using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Receiver;
using ExitGames.Client.Photon;
using LitJson;
using MOBACommon.Codes;
using MOBACommon.Config;
using MOBACommon.Dto;
using UnityEngine;

public class BattleReceiver : MonoBehaviour,IReceiver
{
    [Header("RedTeam")]
    [SerializeField]
    private Transform[] redTeamSpawnPoints;
    [SerializeField]
    private GameObject[] redTeamBuilds;
    [Header("BlueTeam")]
    [SerializeField]
    private Transform[] blueTeamSpawnPoints;
    [SerializeField]
    private GameObject[] blueTeamBuilds;


    //缓存
    private HeroModel[] _heroModels;
    private BuildModel[] _buildModels;
    private Dictionary<int,BaseController> _createControllers = new Dictionary<int, BaseController>();
    private BattleView battleView;


    void Start()
    {
        battleView = GetComponent<BattleView>();
    }
    public void OnReceive(byte subCode, OperationResponse response)
    {
        switch (subCode)
        {
            case OpBattle.GetInfo:
                OnGetInfo(JsonMapper.ToObject<HeroModel[]>(response[0].ToString()),
                    JsonMapper.ToObject<BuildModel[]>(response[1].ToString()));
                break;
        }
    }

    private void OnGetInfo(HeroModel[] heroModels, BuildModel[] buildModels)
    {
        //战斗房间的数据保存到本地
        this._heroModels = heroModels;
        this._buildModels = buildModels;
        int myTeamID = GetTeamID(heroModels, GameData.Player.ID);
        #region 创建英雄
        GameObject createGameObject = null;
        foreach (HeroModel heroModel in heroModels)
        {
            //创建英雄游戏物体  首先加载预设
            createGameObject = Instantiate(Resources.Load(Paths.RES_Hero + heroModel.Name)) as GameObject;
            if (heroModel.TeamID == 1)
            {
                for (int i = 0; i < redTeamSpawnPoints.Length; i++)
                {
                    createGameObject.transform.position = redTeamSpawnPoints[i].position;
                    createGameObject.transform.rotation = redTeamSpawnPoints[i].rotation;
                }
            }
            else if (heroModel.TeamID == 2)
            {
                for (int i = 0; i < blueTeamSpawnPoints.Length; i++)
                {
                    createGameObject.transform.position = blueTeamSpawnPoints[i].position;
                    createGameObject.transform.rotation = blueTeamSpawnPoints[i].rotation;
                }
            }
            BaseController controller = createGameObject.GetComponent<BaseController>();
            //初始化控制器
            controller.Init(heroModel, heroModel.TeamID == myTeamID);
            //添加到字典
            _createControllers.Add(heroModel.ID, controller);

            //判断这个英雄是不是自己的英雄
            if (heroModel.ID == GameData.Player.ID)
            {
                //保存当前的英雄
                GameData.MyController = controller;
                //初始化UI
                battleView.InitView(heroModel);
            }
        }
        #endregion

        #region 创建建筑
        for (int i = 0; i < buildModels.Length; i++)
        {
            BuildModel buildModel = buildModels[i];
            if (buildModel.TeamID == 1)
            {
                createGameObject = redTeamBuilds[(buildModel.TypeID - 1)];
                createGameObject.SetActive(true);
            }
            else if (buildModel.TeamID == 2)
            {
                createGameObject = blueTeamBuilds[(buildModel.TypeID - 1)];
                createGameObject.SetActive(true);
            }
            //初始化控制器
            BaseController controller = createGameObject.GetComponent<BaseController>();
            controller.Init(buildModel, buildModel.TeamID == myTeamID);
            //添加到字典
            _createControllers.Add(buildModel.ID, controller);
        } 
        #endregion

    }
    private int GetTeamID(HeroModel[] heros, int playerID)
    {
        foreach (var item in heros)
        {
            if (item.ID == playerID)
                return item.TeamID;
        }
        return -1;
    }
}
