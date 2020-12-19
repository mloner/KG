using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class MainMenu : MonoBehaviour {

    public Slider slider;
    public struct score {
        public string datetime;
        public string scoreTime;
    };
    public TextMeshProUGUI LastScoreText;
    public TextMeshProUGUI Recordtable;

    public void Start() {
        string scListStr = "";
        LastScoreText.text = "Последнее время:" + PlayerPrefs.GetString("ScoreTime");
        List<score> scList = new List<score>();
        
        string[] scores = PlayerPrefs.GetString("ScoreTable").Split('\n');
        foreach(string sc in scores) {
            if (sc != "")
            {
                var tmp = new score();
                tmp.datetime = sc.Split('|')[0];
                tmp.scoreTime = sc.Split('|')[1];
                scList.Add(tmp);
            }
            scList = scList.OrderBy(o => o.scoreTime).ToList();
            
        }
        foreach (var el in scList)
        {
            scListStr += el.datetime + "|" + el.scoreTime + "\n";

        }

        Recordtable.text = scListStr;
    }
    public void PlayGame() {
        SceneManager.LoadScene("Minigame");
    }

    public void ExitGame()
    {
        Debug.Log("Вышли из игры!");
        Application.Quit();
    }

    public void SaveSettings() {
        PlayerPrefs.SetInt("BallSize", Convert.ToInt32(slider.value));
    }
}
