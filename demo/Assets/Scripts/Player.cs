using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private bool isGrouded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float attackRange;
    [SerializeField] private float SpeedRotate;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private AudioClip Slash;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private int maxHealth = 30;
    public int currentHealth;
 
    [SerializeField] private float gravity;
    [SerializeField] private LayerMask groundMask;
    private Vector3 Velocity;
    private Vector3 Direction;

    private AudioSource PlayerAudi;
    private Spawn SpawnScript;
    private Animator PlayerAnimator;
    private CharacterController Controller;
    // Start is called before the first frame update
    void Start()
    {

        SpawnScript = GameObject.Find("SpawnManager").GetComponent<Spawn>();
        PlayerAudi = GetComponent<AudioSource>();
        PlayerAnimator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        Moving();
        
    }

    private void Moving()
    {

        isGrouded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        float vertical = Input.GetAxis("Vertical");
        Direction = new Vector3(0f, 0f, vertical);
        Direction = transform.TransformDirection(Direction);
        if (isGrouded)
        {
            if (Direction != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (Direction != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else if (Direction == Vector3.zero)
            {
                Idle();
            }

            Direction *= Speed;
        }
        Controller.Move(Direction * Time.deltaTime);
        Gravity();

    }

    private void OnMouseDown()
    {
        PlayerAnimator.SetTrigger("attack");
        Collider[] hitEnemy = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider enemy in hitEnemy)
        {
            Vector3 awayFormPlayer = (enemy.GetComponent<Transform>().transform.position - transform.position);
            enemy.GetComponent<Enemy>().Takedamge(attackDamage);
            enemy.GetComponent<HealthBar>().LossHealth(attackDamage);
            enemy.GetComponent<Rigidbody>().AddForce(awayFormPlayer * 3f, ForceMode.Impulse);
            enemy.GetComponent<Rigidbody>().AddForce(Vector3.up * 4f, ForceMode.Impulse);
            PlayerAudi.PlayOneShot(Slash, 1.0f);

        }


    }
    private void Gravity()
    {
        //check is on ground if is then stop apply gravity
        if (isGrouded && Velocity.y < 0)
        {
            Velocity.y = -2;
        }
        //caculate gravity
        Velocity.y += gravity * Time.deltaTime;
        Controller.Move(Velocity * Time.deltaTime);
    }

    private void Walk()
    {
        Speed = walkSpeed;
        PlayerAnimator.SetFloat("animation", 0.25f, 0.1f, Time.deltaTime);

    }

    private void Run()
    {
        Speed = runSpeed;
        PlayerAnimator.SetFloat("animation", 0.5f, 0.1f, Time.deltaTime);
    }

    private void Idle()
    {
        PlayerAnimator.SetFloat("animation", 0, 0.1f, Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);


    }

    
    
    public void Takedamge(int damage)
    {
        currentHealth -= damage;
       
        PlayerAnimator.SetFloat("animation", 0.75f, 0.1f, Time.deltaTime);
        if (currentHealth <= 0)
        {

            PlayerAnimator.SetFloat("animation", 1f, 0.1f, Time.deltaTime);
            SpawnScript.GameOver();
            PlayerAnimator.SetBool("Die", true);
            Invoke("Die", 3);
        }

    }
    void Die()
    {

        Destroy(gameObject);
    }

}
