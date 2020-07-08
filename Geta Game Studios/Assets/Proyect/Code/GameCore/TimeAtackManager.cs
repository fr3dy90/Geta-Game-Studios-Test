using System;
using System.Collections;
using Cinemachine;
using KartGame.KartSystems;
using TMPro;
using UnityEngine;

public class TimeAtackManager : MonoBehaviour
{
    //Singleton
    public static TimeAtackManager Instance;
    
    [Header("GameCore")] public int maxLaps;
    private int lapcount;
    private int checkPointCount;
    private AddTime[] checkPoints;
    private bool isPlay;
    private IEnumerator startGame;
    private IEnumerator closeGame;
    private bool doOnce = false;

    [Header("Timers")] 
    public float raceTime;
    public float timeToFade;
    public float countDown;

    [Header("UI")] public TextMeshProUGUI tmp_TimeLeft;
    public TextMeshProUGUI tmp_InfoLaps;
    public TextMeshProUGUI tmp_CountDown;

    [Header("Screen Mesages")] public string win;
    public string lose;

    [Header("Player Spawn Point")] 
    public Transform spawnPoint;
    private GameObject player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        lapcount = 1;
        checkPointCount = 0;
        checkPoints = FindObjectsOfType<AddTime>();
        isPlay = false;
    }

    private void Start()
    {
        player = FindObjectOfType<Loader>().CreatePlayer();
        player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        player.GetComponent<ArcadeKart>().enabled = false;

        CinemachineVirtualCamera cmvc = FindObjectOfType<CinemachineVirtualCamera>();
        cmvc.Follow = player.transform;
        cmvc.LookAt = player.transform;
        
        ActualizeLapCount();
        FadeManager.Instance.StartFade(false);
    }

    void Update()
    {
        if (!isPlay)
        {
            return;
        }
        SetTimers();
    }

    private void SetTimers()
    {
        raceTime -= Time.deltaTime;
        tmp_TimeLeft.text = string.Format("{0:.00}", raceTime);
        if (raceTime <= 0)
        {
            EndGame(false);
            tmp_TimeLeft.gameObject.SetActive(false);
        }
    }
    
    private IEnumerator StartGame()
    {
        tmp_CountDown.gameObject.SetActive(true);
        while (countDown > 0)
        {
            yield return null;
            SetUiCountDown((int) countDown);
            countDown -= Time.deltaTime;
        }

        tmp_CountDown.gameObject.SetActive(false);
        isPlay = true;
        player.GetComponent<ArcadeKart>().enabled = true;
    }

    private IEnumerator CloseGame(bool isWinner)
    {
        tmp_CountDown.text = isWinner ? win : lose;
        tmp_CountDown.gameObject.SetActive(true);
        
        while (timeToFade > 0)
        {
            yield return null;
            timeToFade -= Time.deltaTime;
        }
        FadeManager.Instance.StartFade(true);
    }

    private void StartCloseGame(bool isWinner)
    {
        if (closeGame != null)
        {
            StopCoroutine(closeGame);
        }
        closeGame = CloseGame(isWinner);
        StartCoroutine(closeGame);
    }
    
    private void SetUiCountDown(int value)
    {
        tmp_CountDown.text = value != 0 ? value.ToString() : "GO!!!";
    }
    public void ActualizeLapCount()
    {
        if (checkPointCount >= checkPoints.Length)
        {
            checkPointCount = 0;
            lapcount++;
            for (int i = 0; i < checkPoints.Length; i++)
            {
                checkPoints[i].gameObject.SetActive(true);
            }
        }

        if (lapcount > maxLaps)
        {
            EndGame(true);
            return;
        }

        tmp_InfoLaps.text = lapcount.ToString() + "/" + maxLaps.ToString();
    }
    
    public void AddTime(float timeAdd)
    {
        raceTime += timeAdd;
        checkPointCount++;
    }
    
    public void SetStartGame()
    {
        if (startGame != null)
        {
            StopCoroutine(startGame);
        }
        startGame = StartGame();
        StartCoroutine(startGame);
    }

    private void EndGame(bool isWinner)
    {
        if (!doOnce)
        {
            doOnce = true;
            isPlay = false;
            StartCloseGame(isWinner);
            player.GetComponent<ArcadeKart>().enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPoint.position, .5f);
    }
}