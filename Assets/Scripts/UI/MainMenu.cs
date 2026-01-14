using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    Button newGameBtn;
    Button continueBtn;
    Button quitBtn;
    PlayableDirector director;
    
    // 新增状态标识
    private bool isContinue = false;

    void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();

        // 修改监听器
        newGameBtn.onClick.AddListener(PlayTimelineForNewGame);
        continueBtn.onClick.AddListener(PlayTimelineForContinue);
        quitBtn.onClick.AddListener(QuitGame);

        director = FindObjectOfType<PlayableDirector>();
        // 修改为统一的事件处理
        director.stopped += OnTimelineStopped;
    }

    // 新游戏按钮调用
    void PlayTimelineForNewGame()
    {
        isContinue = false;
        director.Play();
    }

    // 继续游戏按钮调用
    void PlayTimelineForContinue()
    {
        isContinue = true;
        director.Play();
    }

    // 统一的动画结束回调
    void OnTimelineStopped(PlayableDirector obj)
    {
        if (isContinue)
        {
            ContinueGame();
        }
        else
        {
            NewGame(obj);
        }
    }

    void NewGame(PlayableDirector obj)
    {
        PlayerPrefs.DeleteAll();
        SceneController.Instance.TransitionToFirstLevel();
    }

    void ContinueGame()
    {
        SceneController.Instance.TransitionToLoadGame();
    }
    
    void QuitGame()
    {
        Application.Quit();
        Debug.Log("退出游戏");
    }
}