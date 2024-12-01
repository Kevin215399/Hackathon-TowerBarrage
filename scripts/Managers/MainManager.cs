using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MainManager : MonoBehaviour
{
    public delegate void WaveEnd();
    public event WaveEnd waveEnd;

    public static MainManager Instance;

    [Header("Prefabs")]
    [SerializeField] private List<GameObject> towers = new List<GameObject>();
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    [Header("Waves")]
    [SerializeField] private List<WaveSO> waves = new List<WaveSO>();
    [SerializeField] private List<SpawnChunk> currentWave = new List<SpawnChunk>();

    [Header("stats")]

    [SerializeField] public float towerHealth = 150;
    [SerializeField] private int waveNum = 0;
    [SerializeField] private List<int> prices = new List<int>();
    [SerializeField] private int money = 999;
    [SerializeField] private GameObject enemyNumShow;
    [SerializeField] private Text enemyNumberText;


    [Header("Show Stats")]
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI healthText;
    private int totalEnemiesKilled = 0;
    private int totalMoneySpent = 0;

    [Header("End screen")]
    [SerializeField] private GameObject lose;
    [SerializeField] private Text waveText;
    [SerializeField] private Text enemyText;
    [SerializeField] private Text spentText;

    [Header("Other")]
    [SerializeField] private GameObject startButton;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private ParticleSystem towerDamage;
    [SerializeField] private GameObject TowerBuy;


    private float timeSpeed = 1;
    private List<SpawnChunk> extraSpawn = new List<SpawnChunk>();
    private int enemyNum = 0;
    private bool waveSpawning = false;
    private int spawnTimes = 0;

    private bool waveFinished = true;
    private int enemiesKilled = 0;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        enemyNumberText.text = (NumberOfEnemies(waves[waveNum].enemySequence) + NumberOfEnemies(extraSpawn)).ToString();

    }
    private void Update()
    {

        if (towerHealth <= 0)
        {
            DebuffManager.Instance.Stop();
            towerHealth = 0;
            lose.SetActive(true);
            waveText.text = "Wave: " + waveNum;
            enemyText.text = "Enemies Killed: " + totalEnemiesKilled;
            spentText.text = "Coins Spent: " + totalMoneySpent;
            timeSpeed = 1;
            startButton.SetActive(false);
            TowerBuy.SetActive(false);
        }

        moneyText.text = Mathf.Floor(money) + " Coins";
        healthText.text = Mathf.Ceil(towerHealth) + " Health";
        healthSlider.value = towerHealth / 100;

        if (!waveFinished)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
            if (enemies.Length == 0 && waveSpawning == false)
            {
                DebuffManager.Instance.enemyHealthMultiplier += 0.2f;
                DebuffManager.Instance.enemySpeedMultiplier *= 0.95f;
                waveFinished = true;
                GetComponent<SoundManager>().PlayClip("win");
                money += (int)Mathf.Ceil((15 + Mathf.Min(30, enemiesKilled * 1.6f)) * DebuffManager.Instance.coinMultiplier);
                if (money > 0)
                    waveEnd?.Invoke();
                startButton.SetActive(true);
                enemyNumShow.SetActive(true);

            }
        }
        else
        {
            if (waveNum < waves.Count)
            {
                enemyNumberText.text = (NumberOfEnemies(waves[waveNum].enemySequence) + NumberOfEnemies(extraSpawn)).ToString();
            }
            else
            {
                enemyNumberText.text = (NumberOfEnemies(waves[waves.Count - 1].enemySequence) + NumberOfEnemies(extraSpawn)).ToString();
            }
        }

        speedText.text = timeSpeed + " >>";
        Time.timeScale = timeSpeed;
    }
    private int NumberOfEnemies(List<SpawnChunk> enemies)
    {
        int amount = 0;
        foreach (SpawnChunk chunk in enemies)
        {
            amount += chunk.enemyAmount;
        }
        return amount;
    }
    public void StartWave()
    {
        enemyNumShow.SetActive(false);
        enemiesKilled = 0;
        Debug.Log("wave started");
        waveSpawning = true;
        enemyNum = 0;
        spawnTimes = 0;
        if (waveNum < waves.Count)
        {
            currentWave = waves[waveNum].enemySequence;
        }
        else
        {
            Debug.LogWarning("reached end of waves. Repeating last wave");
            currentWave = waves[waves.Count - 1].enemySequence;
        }
        currentWave.AddRange(extraSpawn.ToArray());
        StartCoroutine(SpawnEnemies());
        waveNum++;
        waveFinished = false;
        startButton.SetActive(false);
        GetComponent<SoundManager>().PlayClip("startWave");

    }
    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(currentWave[enemyNum].delay);

        StartCoroutine(SpawnSection(enemyNum));


    }
    IEnumerator SpawnSection(int section)
    {
        yield return new WaitForSeconds(0.5f);
        if (spawnTimes < currentWave[section].enemyAmount && waveSpawning)
        {
            Instantiate(enemies[currentWave[section].enemyIndex], new Vector2(-14, 0), Quaternion.identity);
            spawnTimes++;
            StartCoroutine(SpawnSection(section));
        }
        else
        {
            spawnTimes = 0;
            enemyNum++;
            if (enemyNum == currentWave.Count)
            {
                waveSpawning = false;
                yield break;
            }
            else
            {
                StartCoroutine(SpawnEnemies());
            }
        }

    }
    public void HitTower(float dmg)
    {
        Debug.Log("Hit Function Run");
        towerHealth -= dmg;
        towerDamage.Play();
    }
    public void PlaceTower(int towerNum)
    {
        if (money >= prices[towerNum])
        {
            GetComponent<SoundManager>().PlayClip("buy");
            totalMoneySpent += prices[towerNum];
            money -= prices[towerNum];
            Vector2 mousePos = InputManager.Instance.mousePosition;
            Instantiate(towers[towerNum], mousePos, Quaternion.identity);
        }
    }
    public void KilledEnemy()
    {
        enemiesKilled++;
        totalEnemiesKilled++;
    }

    public void ReturnToMenu()
    {
        GetComponent<sceneManager>().ChangeScene("menu");
    }

    public void AddEnemy(SpawnChunk spawnChunk)
    {
        extraSpawn.Add(spawnChunk);
    }
    public int GetWaveNum()
    {
        return waveNum;
    }
    public void ChangeSpeed()
    {
        timeSpeed += 0.5f;
        if (timeSpeed > 4)
        {
            timeSpeed = 1;
        }
    }
}
