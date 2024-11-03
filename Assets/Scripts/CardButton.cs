using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum CardType
{
    Image,
    Text
}
public class CardButton : MonoBehaviour
{
    public string nameCard;
    [SerializeField] CardType cardType;
    [SerializeField] Button button;
    [SerializeField] GameObject hand;
    [SerializeField] AudioClip audio;
    [SerializeField] Text text;
    [SerializeField] LogicManager logic;
    private Coroutine suggestHandCoroutine;
    private void Awake()
    {
        Init();
    }
    private void Start()
    {
        button.onClick.AddListener(OnCardClick);
    }

    public void Init()
    {
        button = GetComponent<Button>(); 
        button.interactable = false;
        text = GetComponentInChildren<Text>();
        if (cardType == CardType.Text)
        {
            text.text = nameCard;
        }
    }

    public void InteractButton(bool canInteract)
    {
        button.interactable = canInteract;
    }

    private void OnCardClick()
    {
        logic.pickCard(this);
    }
    public void playSound()
    {
        logic.PlaySound(audio);
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
        hand.SetActive(true);
    }

    public void stopSuggestHandButton()
    {
        hand.SetActive(false);
        StopCoroutine(suggestHandCoroutine);
        suggestHandCoroutine = null;
    }
}
