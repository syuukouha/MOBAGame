using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchView : MonoBehaviour
{
    [SerializeField]
    private Text timeText;

    private bool start = false;
    private float minute = 0;
    private float second = 0;

    //开始
    public void StartMatch()
    {
        minute = 0;
        second = 1;
        start = true;
        gameObject.SetActive(true);
    }
    //取消
    public void StopMatch()
    {
        start = false;
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update () {
        if (start)
        {
            second += Time.deltaTime;
            if (second >= 60)
            {
                minute++;
                second = 0;
            }
            timeText.text = minute.ToString().PadLeft(2, '0') + ":" + second.ToString("00");
        }
	}

}
