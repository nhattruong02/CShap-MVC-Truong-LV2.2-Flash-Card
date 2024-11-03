using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class LogicManager : MonoBehaviour
{
    [SerializeField] List<CardButton> cardButtons = new List<CardButton>();
    private List<Vector2> firstTransform = new List<Vector2>();
    [SerializeField] Animator animatorShuffle;
    private CardButton[] compareButton = new CardButton[2];
    [SerializeField] AudioSource sound;
    [SerializeField] AudioClip correctSound;
    [SerializeField] AudioClip wrongSound;
    [SerializeField] GameObject handSuggest;
    [SerializeField] AnimationClip clip;
    private int count = 0;
    private CardButton cardOne, cardTwo;
    private int countWrong = 0;
    private int countCorrect = 0;
    private Coroutine suggestHandCoroutine;

    private void Awake()
    {
        sound = GetComponent<AudioSource>();
        animatorShuffle = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animatorShuffle.enabled = true;
        cardButtons.Clear();
        animatorShuffle.SetTrigger(Common.Shuffle);
        GameObject[] objects = GameObject.FindGameObjectsWithTag(Common.Button);
        for (int i = 0; i < objects.Length; i++)
        {
            cardButtons.Add(objects[i].gameObject.GetComponent<CardButton>());
        }
        StartCoroutine(Shuffle(clip.length + 0.1f));

    }

    private void Start()
    {
       
    }

    public void showSuggestHandButton()
    {
        if (suggestHandCoroutine == null)
        {
            suggestHandCoroutine = StartCoroutine(countDownSuggestHandButton());
        }
    }

    private IEnumerator countDownSuggestHandButton()
    {
        yield return new WaitForSeconds(5);
        handSuggest.SetActive(true);
    }

    public void stopSuggestHandButton()
    {
        handSuggest.SetActive(false);
        if (suggestHandCoroutine != null)
        {
            StopCoroutine(suggestHandCoroutine);
            suggestHandCoroutine = null;
        }
        suggestHandCoroutine = null;
    }

    IEnumerator Shuffle(float time)
    {
        yield return new WaitForSeconds(time);
        showSuggestHandButton();
        animatorShuffle.enabled = false;
        foreach (CardButton button in cardButtons)
        {
            button.InteractButton(true);
        }
        for (int i = 0; i < cardButtons.Count; i++)
        {
            firstTransform.Add(cardButtons[i].gameObject.GetComponent<RectTransform>().anchoredPosition);
        }
        for (int i = 0; i < cardButtons.Count; i++)
        {
            CardButton tmp = cardButtons[i];
            int randomIndex = Random.Range(i, cardButtons.Count);
            cardButtons[i] = cardButtons[randomIndex];
            cardButtons[randomIndex] = tmp;
        }
        changePositionAfterShuffle(cardButtons);

    }

    private void changePositionAfterShuffle(List<CardButton> list)
    {
        for (int i = 0; i < firstTransform.Count; i++)
        {
            list[i].gameObject.GetComponent<RectTransform>().anchoredPosition = firstTransform[i];
        }
    }

    public void pickCard(CardButton button)
    {
        count++;
        if (count > 2)
            return;
        stopSuggestHandButton();
        button.gameObject.GetComponent<Animator>().SetTrigger(Common.FlipUp);
        if (count == 1)
        {
            cardOne = button;
            compareButton[0] = cardOne;
            cardOne.InteractButton(false);
            cardOne.showSuggestHandButton();
            cardOne.playSound();
        }
        if (count == 2)
        {
            cardTwo = button;
            compareButton[1] = cardTwo;
            cardTwo.playSound();
            cardOne.InteractButton(true);
            cardOne.stopSuggestHandButton();
            StartCoroutine(destroyCardAfterTime());
        }
    }

    IEnumerator destroyCardAfterTime()
    {
        yield return new WaitForSeconds(2);
        if (cardOne.nameCard.ToLower() == cardTwo.nameCard.ToLower())
        {
            compareButton[0].gameObject.SetActive(false);
            compareButton[1].gameObject.SetActive(false);
            sound.PlayOneShot(correctSound);
            countCorrect++;
            if (countCorrect == 2)
            {
                GameManager.Instance.nextLevel();
            }
        }
        else
        {
            compareButton[0].GetComponent<Animator>().SetTrigger(Common.FlipDown);
            compareButton[1].GetComponent<Animator>().SetTrigger(Common.FlipDown);
            showSuggestHandButton();
            countWrong++;
            if(countWrong == 3)
            {
                GameManager.Instance.Fail();
                countWrong = 0;
                foreach (CardButton button in cardButtons)
                {
                    button.InteractButton(false);
                }
            }
            sound.PlayOneShot(wrongSound);
        }
        compareButton = new CardButton[2];
        count = 0;

    }
    public void PlaySound(AudioClip audio)
    {
        sound.PlayOneShot(audio);
    }
}
