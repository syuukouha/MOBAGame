using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchCompleteView : MonoBehaviour
{
    [SerializeField]
    private Text timerText;

    private float timer;
    private bool timerStart;
	// Use this for initialization
	void Start ()
	{
	    timer = 10.0f;
	    timerStart = false;
	    gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (timerStart)
	    {
	        timer -= Time.deltaTime;
	        if (timer <= 0)
	        {
                Hide();
	        }
	    }
	    timerText.text = "请在" + (int) timer + "秒内确定进入房间";
	}

    public void Show()
    {
        gameObject.SetActive(true);
        timerStart = true;
    }

    public void Hide()
    {
        timer = 10;
        timerStart = false;
        gameObject.SetActive(false);
    }
}
