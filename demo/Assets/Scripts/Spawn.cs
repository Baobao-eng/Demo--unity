using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Spawn : MonoBehaviour
{
    public GameObject[] EnemyList;
    private int EnemyCount;
    private int waveNumber = 1;
    private GameObject player;
    public TextMeshProUGUI ScoreT;
    private int Score;
    public TextMeshProUGUI GameOverT;
    public Button RestartB;
    public bool GameOverCheck = false;
    public GameObject AfterDeath;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("CharController");
        SpawnEnemy(waveNumber);
        Score = 0;
        ScoreT.text = "Score:" + Score;
    }

    // Update is called once per frame
    void Update()
    {

        EnemyCount = FindObjectsOfType<Enemy>().Length;
        if (EnemyCount == 0 && GameOverCheck == false)
        {

            waveNumber++;
            SpawnEnemy(waveNumber);
            nextStage();
        }
    }

    void SpawnEnemy(int count)
    {
        for (int i = 0; i < count; i++)
        {

            int index = Random.Range(0, EnemyList.Length);

            Instantiate(EnemyList[index], SpawnArea(), EnemyList[index].transform.rotation);
        }

    }

    Vector3 SpawnArea()
    {
        return new Vector3(Random.Range(-5, 20), 0.5f, Random.Range(13, 20));
    }

    public void UpdateScore(int Scoreadd)
    {
        if (GameOverCheck == false)
        {
            Score += Scoreadd;
            ScoreT.text = "Score:" + Score;
        }

    }
    void nextStage()
    {

        player.transform.position = new Vector3(0, 0.55f, 0);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        player.GetComponent<Player>().currentHealth = 50;
    }

    public void GameOver()
    {
        AfterDeath.SetActive(true);
        GameOverT.gameObject.SetActive(true);
        RestartB.gameObject.SetActive(true);
        GameOverCheck = true;

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
