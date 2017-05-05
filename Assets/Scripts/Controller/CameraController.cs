using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //相机可移动的范围
    private  float X_MIN = 70f;
    private  float X_MAX = 230f;
    private  float Z_MIN = 40f;
    private  float Z_MAX = 110f;


    /// <summary>
    /// 相机移动速度
    /// </summary>
    [SerializeField]
    private float speed = 15f;
    /// <summary>
    /// 敏感区域 （屏幕百分比）
    /// </summary>
    private float area = 0.2f;
    // Use this for initialization
    void Awake ()
    {
        Cursor.lockState = CursorLockMode.Confined;
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        //目标点
        Vector3 target = Vector3.zero;
        Vector3 mousePos = Input.mousePosition;
        //检测垂直方向
        if (mousePos.y > Screen.height * (1 - area))
        {
                target.z = 2;
        }else if (mousePos.y < Screen.height * area)
        {
                target.z = -2;
        }
        else
        {
            target.z = 0;

        }
        //检测水平方向
        if (mousePos.x > Screen.width * (1 - area))
        {
                target.x = 2;

        }
        else if (mousePos.x < Screen.width * area)
        {
                target.x = -2;
        }
        else
        {
            target.x = 0;
        }
        //如果两个方向都有检测到，那么我们就把移动速度同步，平滑移动
        if (target.x!=0 && target.z != 0)
        {
            target = target.normalized * Mathf.Max(Mathf.Abs(target.x), Mathf.Abs(target.z));
        }
        //开始移动
        transform.position += target * Time.deltaTime * speed;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, X_MIN, X_MAX), transform.position.y, Mathf.Clamp(transform.position.z, Z_MIN, Z_MAX));
    }

    /// <summary>
    /// 聚焦到英雄
    /// </summary>
    public void FocusHero()
    {
        if (GameData.MyController == null)
            return;
        Vector3 heroPostion = GameData.MyController.transform.position;
        transform.position = new Vector3(heroPostion.x, transform.position.y, heroPostion.z - 20.0f);
    }
}
