using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    private int CurrentHealhloss = 0;
    private Spawn SpawnManager;
    public Slider healthBar;
    public int healhToLoss;

    void Start()
    {
        healthBar.maxValue = healhToLoss;
        healthBar.value = 0;
        healthBar.fillRect.gameObject.SetActive(false);

        SpawnManager = GameObject.Find("SpawnManager").GetComponent<Spawn>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LossHealth(int amount)
    {
        CurrentHealhloss += amount;
        healthBar.fillRect.gameObject.SetActive(true);
        healthBar.value = CurrentHealhloss;

    }
}
