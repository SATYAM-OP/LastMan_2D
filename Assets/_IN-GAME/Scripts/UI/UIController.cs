using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : Singleton<UIController>
{
   

    [Header("Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject joyStickPanel;
    [SerializeField] private GameObject abilityPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;

    [Header("GamePanel")]
    [SerializeField] private TMPro.TMP_Text killsText;
    [SerializeField] private TMPro.TMP_Text TimeText;
    [SerializeField] private TMPro.TMP_Text LevelText;

    [Header("Score")]
    [SerializeField] private TMPro.TMP_Text KillCountScoreText;
    [SerializeField] private TMPro.TMP_Text TimeScoreText;

    private int minutes = 0;
    private int seconds = 0;
    private int killsCount = 0;
   [HideInInspector] public bool isGameStarted = false;

    private void Start()
    {
        CloseAllPanels();
        startPanel.SetActive(true);
        Time.timeScale = 0;
      //  Enemy.EnemyDeathCount = 0;
    
    }

    void CloseAllPanels()
    {
        startPanel.SetActive(false);
        gamePanel.SetActive(false);
        joyStickPanel.SetActive(false);
        abilityPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
    }
  /*  private void Update()
    {
        killsText.text = Enemy.EnemyDeathCount.ToString();
    }*/
    public void StartGame()
    {
        SoundManager.instance.Play("ui");
        CloseAllPanels();
        gamePanel.SetActive(true);
        joyStickPanel.SetActive(true);
        StartCoroutine("UpdateTimer");
        Time.timeScale = 1;
        isGameStarted = true;
    }

    public void OpenAbilityPanel()
    {
        Time.timeScale = 0.0f;
        abilityPanel.SetActive(true);
    }

    public void CloseAbilityPanel()
    {
        Time.timeScale = 1.0f;
        abilityPanel.SetActive(false);
    }

    public void GameOver()
    {
       
        CloseAllPanels();
        gameOverPanel.SetActive(true);
     
    }

    public void Pause()
    {
        SoundManager.instance.Play("ui");
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void resume()
    {
        SoundManager.instance.Play("ui");
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void exit()
    {
        SoundManager.instance.Play("ui");
        Application.Quit();
    }

    public void Addkill()
    {
        killsCount++;
        killsText.text = killsCount.ToString();
    }

    public void restart()
    {
        SoundManager.instance.Play("ui");
        CloseAllPanels();
        gamePanel.SetActive(true);
        joyStickPanel.SetActive(true);
        seconds = 0;
        minutes = 0;
        TimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        killsCount = 0;
        killsText.text = killsCount.ToString();
        SceneManager.LoadScene(0);
        
    }

    public void gameOver()
    {
        SoundManager.instance.Play("gameover");
        CloseAllPanels();
        gameOverPanel.SetActive(true);
        KillCountScoreText.text = killsCount.ToString();
        TimeScoreText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public int GetKillCounts()
    {
        return killsCount;
    }

    public string GetTime()
    {
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private IEnumerator UpdateTimer()
    {
        while (true)
        {
            // Increment seconds and handle overflow
            seconds++;
            if (seconds >= 60)
            {
                minutes++;
                seconds = 0;
            }

            // Update the timer text
            TimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Wait for 1 second
            yield return new WaitForSeconds(1f);
        }
    }

}
