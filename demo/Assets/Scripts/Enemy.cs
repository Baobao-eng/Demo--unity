using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator enemyAnimator;
    private float minDist = 1.5f;
    private Transform playerPos;
    public float Speed;
    public ParticleSystem ExploPartical;
    public int maxHealth;
    private int currentHealth;
    public int Damage;
    private Player PlayerScript;
    public int Point;
    private Spawn SpawnScript;
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public float attackRange;
    public ParticleSystem EnemyPar;
    // Start is called before the first frame update
    void Start()
    {
        SpawnScript = GameObject.Find("SpawnManager").GetComponent<Spawn>();
        enemyAnimator = GetComponent<Animator>();
        playerPos = GameObject.Find("CharController").GetComponent<Transform>();
        PlayerScript = GameObject.Find("CharController").GetComponent<Player>();

        currentHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y < -20)
        {
            Destroy(gameObject);
        }

        Chase();

    }


    void Chase()
    {
        if (SpawnScript.GameOverCheck == false)
        {
            transform.LookAt(playerPos);


            if (Vector3.Distance(playerPos.position, transform.position) > minDist)
            {
                enemyAnimator.SetBool("Near", false);
                transform.position += transform.forward * Speed * Time.deltaTime;

            }
            else if (Vector3.Distance(playerPos.position, transform.position) <= minDist)
            {
                enemyAnimator.SetBool("Near", true);
                Collider[] hitEnemy = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
                foreach (Collider player in hitEnemy)
                {
                    Vector3 awayFormPlayer = (player.GetComponent<Transform>().transform.position - transform.position);
                    player.GetComponent<Player>().Takedamge(Damage);
                    player.GetComponent<Rigidbody>().AddForce(awayFormPlayer * 3f, ForceMode.Impulse);
                    player.GetComponent<Rigidbody>().AddForce(Vector3.up * 4f, ForceMode.Impulse);

                }


            }
        }

    }


    void OnDrawGizmosSelected()
    {

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    }

    public void Takedamge(int damage)
    {
        currentHealth -= damage;
        enemyAnimator.SetTrigger("Hurt");
        Instantiate(EnemyPar, transform.position, EnemyPar.transform.rotation);
        if (currentHealth <= 0)
        {
            SpawnScript.UpdateScore(Point);
            Instantiate(ExploPartical, transform.position, ExploPartical.transform.rotation);
            enemyAnimator.SetBool("Die", true);
            Die();
        }

    }


    void Die()
    {

        Destroy(gameObject);
    }




}

