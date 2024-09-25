using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    [SerializeField] List<GameObject> levels;
    private int currentLv = 0;
    [SerializeField] Text textLv;
    [SerializeField] GameObject endGame;
    [SerializeField] GameObject reloadLevel;
    [SerializeField] AnimationClip flipDown;
    private void Awake()
    {
        _instance = this;

    }
    void Start()
    {
        showCurrentLevel();
    }
    private void showCurrentLevel()
    {
        foreach (GameObject level in levels)
        {
            level.SetActive(false);
        }
        if (currentLv < levels.Count)
        {
            levels[currentLv].SetActive(true);
            textLv.text = Common.Turn + (currentLv + 1);
        }
        else
        {
            endGame.SetActive(true);
        }
    }

    public void nextLevel()
    {
        if (currentLv < levels.Count)
        {
            currentLv++;
            showCurrentLevel();
        }
    }
    public void Restart()
    {
        currentLv = 0;
        showCurrentLevel();
    }
    public void Fail()
    {
        StartCoroutine(waitDeactiveReload(flipDown.length));
    }

    IEnumerator waitDeactiveReload(float time)
    {
        yield return new WaitForSeconds(time);
        reloadLevel.SetActive(true);
        levels[currentLv].SetActive(false);
    }

    public void onclickReload()
    {
        levels[currentLv].SetActive(true);
        reloadLevel.SetActive(false);
    }
}

