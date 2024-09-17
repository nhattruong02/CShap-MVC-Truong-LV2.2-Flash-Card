using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] Sprite bgImage;
    public List<Button> buttonList = new List<Button>();
    public List<Sprite> spriteList = new List<Sprite>();
    private bool firstGuess, secondGuess;
    private int firstGuessIndex, secondGuessIndex;
    private string firstGuessPuzzle, secondGuessPuzzle;

    void Start()
    {
        GetButtons();
        AddListener();
    }
    private void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(Common.puzzleButton);
        for (int i = 0; i < objects.Length; i++)
        {
            buttonList.Add(objects[i].GetComponent<Button>());
            buttonList[i].image.sprite = bgImage;
        }
    }
    void AddListener()
    {
        foreach (Button btn in buttonList)
        {
            btn.onClick.AddListener(() => PickPuzzle());
        }
    }
    private void PickPuzzle()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(name);
        if (!firstGuess)
        {
            firstGuess = true;
            
        }
        if (!secondGuess)
        {
            secondGuess = true;
            
        }
    }



    void Update()
    {
        
    }


    
}
