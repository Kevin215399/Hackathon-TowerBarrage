using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffManager : MonoBehaviour
{
    public static DebuffManager Instance { get; private set; }
    [SerializeField] private GameObject chooseDebuffPanel;
    [SerializeField] private GameObject anchor1;
    [SerializeField] private GameObject anchor2;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private List<DebuffCardSO> cards;
    [SerializeField] private Color unselectedColor;
    public float playerDamageMultiplier = 1; // implementation done
    public float enemySpeedMultiplier = 1;// implementation done
    public float enemyDamageMultiplier = 1;// implementation done
    public float enemyHealthMultiplier = 1;// implementation done
    public float coinMultiplier = 1;// implementation done

    private DebuffCardSO cardOption1;
    private DebuffCardSO cardOption2;
    private DebuffCardSO cardInAction;

    private GameObject cardObject1;
    private GameObject cardObject2;

    private List<DebuffCardSO> cardRandomBag = new List<DebuffCardSO>();
    public void Stop(){
        chooseDebuffPanel.SetActive(false);
        Destroy(cardObject1);
        Destroy(cardObject2);
    }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("DebuffManager was duplicated: " + this);
            Destroy(this);
        }
        MainManager.Instance.waveEnd += WaveComplete;
        cardRandomBag = new List<DebuffCardSO>(cards);
    }
    private void WaveComplete()
    {
        chooseDebuffPanel.SetActive(true);

        cardOption1 = GetRandomCard();
        cardOption2 = GetRandomCard();

        cardObject1 = Instantiate(cardPrefab, anchor1.transform);
        cardObject1.GetComponent<CardDisplayer>().SetCard(cardOption1);

        cardObject2 = Instantiate(cardPrefab, anchor2.transform);
        cardObject2.GetComponent<CardDisplayer>().SetCard(cardOption2);
    }
    private DebuffCardSO GetRandomCard()
    {
        int index = Random.Range(0, cardRandomBag.Count);
        while (index == 4 && MainManager.Instance.GetWaveNum() < 4)
        {
            index = Random.Range(0, cardRandomBag.Count);
        }
        DebuffCardSO card = cardRandomBag[index];
        cardRandomBag.RemoveAt(index);
        if (cardRandomBag.Count == 0)
        {
            cardRandomBag = new List<DebuffCardSO>(cards);
        }
        Debug.Log("random card: " + card.cardName);
        return card;
    }
    public void SelectCard(DebuffCardSO card)
    {

        if (card == cardOption1)
        {
            cardObject2.GetComponent<CardDisplayer>().SetOverallColor(unselectedColor);
            cardInAction = cardOption1;
        }
        else if (card == cardOption2)
        {
            cardObject1.GetComponent<CardDisplayer>().SetOverallColor(unselectedColor);
            cardInAction = cardOption2;
        }
        else
        {
            Debug.LogWarning("Selected card was not an option");
        }

        StartCoroutine(ClosePanel());
        GetComponent<SoundManager>().PlayClip("selectDebuff");
        ActivateCard(cardInAction);
    }

    public void ActivateCard(DebuffCardSO card)
    {
        switch (cards.IndexOf(card))
        {
            case 0:
                Debug.Log("Coin Lose");
                coinMultiplier *= 0.75f;
                break;
            case 1:
                Debug.Log("Damage reduced");
                playerDamageMultiplier *= 0.8f;
                break;
            case 2:
                Debug.Log("Add enemy health");
                enemyHealthMultiplier *= 1.2f;
                break;
            case 3:
                Debug.Log("Lose one tower");
                GameObject[] towers = GameObject.FindGameObjectsWithTag("tower");
                Destroy(towers[Random.Range(0, towers.Length)]);
                break;
            case 4:
                Debug.Log("Add ten new enemies");
                int totalEnemies = 0;
                int addChunk = 0;
                while (totalEnemies < 10)
                {
                    addChunk = Random.Range(1, 5);
                    while (addChunk + totalEnemies > 10)
                    {
                        addChunk = Random.Range(1, 5);
                    }
                    SpawnChunk chunk = new SpawnChunk(Random.Range(0, 4), addChunk, Random.Range(1, 5));
                    MainManager.Instance.AddEnemy(chunk);
                    totalEnemies += addChunk;
                }

                break;
            case 5:
                Debug.Log("Double damage to tower");
                enemyDamageMultiplier *= 1.5f;
                break;
            default:
                Debug.LogWarning("card not in list: " + card.cardName);
                break;
        }
    }
    IEnumerator ClosePanel()
    {
        yield return new WaitForSeconds(1f);
        chooseDebuffPanel.SetActive(false);
        Destroy(cardObject1);
        Destroy(cardObject2);
    }
}
