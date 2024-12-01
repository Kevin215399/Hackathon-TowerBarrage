using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDisplayer : MonoBehaviour
{
    [SerializeField] private DebuffCardSO cardData;

    [SerializeField] private GameObject cardVisual;
    [SerializeField] private Text description;
    [SerializeField] private Text title;
    [SerializeField] private Image image;
    [SerializeField] private float speed = 20;
    [SerializeField] private int startY = -250;
    private Image cardRenderer;
    private Color titleColorMultiplier = Color.white;
    private RectTransform rectTransform;
    private void Start()
    {
        cardRenderer = cardVisual.GetComponent<Image>();
        rectTransform = cardVisual.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, startY, 0);
    }
    private void Update()
    {
        if (rectTransform.localPosition.y < 0)
        {
            rectTransform.localPosition += new Vector3(0, speed * Time.deltaTime, 0);
        } else {
            rectTransform.localPosition = Vector3.zero;
        }
    }
    public void SetCard(DebuffCardSO data)
    {
        cardData = data;
        Refresh();
    }
    private void Refresh()
    {

        if (cardData != null)
        {
            description.text = cardData.description;
            title.text = cardData.cardName;
            title.color = cardData.titleColor * titleColorMultiplier;
            image.sprite = cardData.image;
        }
        else
        {
            Debug.LogWarning("Card has no Card Data");
            title.text = "ERROR";
            description.text = "ERROR";
            image.sprite = null;
            title.color = Color.white;
        }
    }
    public void CardSelected()
    {
        DebuffManager.Instance.SelectCard(cardData);
    }
    public void SetOverallColor(Color color)
    {
        cardRenderer.color = color;

        image.color = color;
        titleColorMultiplier = color;
        description.color = Color.black * color;
    }

}
