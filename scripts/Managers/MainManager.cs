using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MainManager : MonoBehaviour
{
    public delegate void Debuff();
    public event Debuff DebuffAll;
    public static MainManager Instance;
    [System.Serializable]
    public class Wave
    {
        public int enemyIndex = 0;
        public int enemyAmount = 0;
        public float delay = 1;
    }
    [SerializeField] private List<GameObject> towers = new List<GameObject>();
    [SerializeField] private List<Wave> enemyWave1 = new List<Wave>();
    [SerializeField] private List<Wave> enemyWave2 = new List<Wave>();
    [SerializeField] private List<Wave> enemyWave3 = new List<Wave>();
    [SerializeField] private List<Wave> enemyWave4 = new List<Wave>();
    [SerializeField] private List<Wave> enemyWave5 = new List<Wave>();

    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private List<Wave> currentWave = new List<Wave>();
    [SerializeField] public float towerHealth = 150;
    [SerializeField] private int waveNum = 0;
    [SerializeField] private List<int> prices = new List<int>();
    [SerializeField] private List<int> enemyPrices = new List<int>();
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject lose;
    [SerializeField] private GameObject twist;
    private int enemyNum = 0;
    private bool waveSpawning = false;
    private int spawnTimes = 0;
    [SerializeField] private int money = 999;
    private bool waveFinished = true;
    private int enemiesKilled = 0;
    public int phase = 0;
    [SerializeField] private GameObject shop1Panel;
    [SerializeField] private GameObject shop2Panel;
    [SerializeField] private GameObject button;
    public bool debuffAll = false;

    private void Start()
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
    private void Update()
    {
        moneyText.text = money + " Coins";
        healthText.text = towerHealth + " Health";
        if(towerHealth <= 0 && phase == 0){
            lose.SetActive(true);
            StartCoroutine(menu());
        }
        if(towerHealth <= 0 && phase == 1){
            win.SetActive(true);
            StartCoroutine(menu());
        }
        if (!waveFinished)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
            if (enemies.Length == 0 && waveSpawning == false)
            {
                waveFinished = true;
                GetComponent<SoundManager>().PlayClip("win");
                money += (int)Mathf.Ceil(10 + enemiesKilled / 3);
                button.SetActive(true);
            }
        }
        if (phase == 0)
        {
            shop1Panel.SetActive(true);
            shop2Panel.SetActive(false);
        }
        else
        {
            shop1Panel.SetActive(false);
            shop2Panel.SetActive(true);
        }

        
    }
    public void StartWave()
    {
        if (phase != 0) return;
        enemiesKilled = 0;
        Debug.Log("wave started");
        waveSpawning = true;
        enemyNum = 0;
        spawnTimes = 0;
        switch (waveNum)
        {
            case 0:
                currentWave = enemyWave1;
                break;
            case 1:
                currentWave = enemyWave2;
                break;
            case 2:
                currentWave = enemyWave3;
                break;
            case 3:
                currentWave = enemyWave4;
                break;
            case 4:
                currentWave = enemyWave5;
                break;
            case 5:
                DebuffAll?.Invoke();
                phase=1;
                StartCoroutine(GiveMoney());
                twist.SetActive(true);
                return;

        }
        StartCoroutine(SpawnEnemies());
        waveNum++;
        waveFinished = false;
        button.SetActive(false);
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
    }
    public void PlaceTower(int towerNum)
    {
        if (money >= prices[towerNum])
        {
            GetComponent<SoundManager>().PlayClip("buy");
            money -= prices[towerNum];
            Vector2 mousePos = InputManager.Instance.mousePosition;
            Instantiate(towers[towerNum], mousePos, Quaternion.identity);
        }
    }
    public void SpawnEnemy(int enemyNum)
    {
        if (money >= enemyPrices[enemyNum])
        {
            GetComponent<SoundManager>().PlayClip("buy");
            money -= enemyPrices[enemyNum];
            Instantiate(enemies[enemyNum], new Vector2(-14, 0), Quaternion.identity);
        }
    }
    public void KilledEnemy()
    {
        enemiesKilled++;
    }

    IEnumerator GiveMoney(){
        yield return new WaitForSeconds(3);
        money += 20;
        StartCoroutine(GiveMoney());
    }
    IEnumerator menu(){
        yield return new WaitForSeconds(3);
        GetComponent<sceneManager>().ChangeScene("menu");
    }
}
