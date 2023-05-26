using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    public Animator enemyAnimator;

    BehaviourTree tree;
    public GameObject player;
    public bool onCoolDown = false;
    private bool waitingForCoolDown = false;
    public bool playerInRange = false;
    private SphereCollider rangeCollider;
    private PlayerHealthController playerHealth;

    [Header("Combat Settings")]
    public int currentHealth = 100;
    public int maxHealth = 100;
    public int attackTimer = 1;
    public float attackRange = 2;
    public float attackDamage = 10;

    [Header("Roam Settings")]
    Vector3 roamGoal;
    public Vector3 walkLimits = new Vector3(30, 30, 30);

    private Vector3 homePosition = Vector3.zero;

    NavMeshAgent agent;
    public enum ActionState { IDLE, WORKING };
    ActionState state = ActionState.IDLE;

    Node.Status treeStatus = Node.Status.RUNNING;

    void Start()
    {
        // Animator
        enemyAnimator = gameObject.GetComponent<Animator>();

        // Get player health controller
        Debug.Log("Player Health for Enemy AI: " + playerHealth.ToString());

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject;
            playerHealth = player.GetComponent<PlayerHealthController>();
            Debug.Log("Player Health for Enemy AI: " + playerHealth.ToString());
        }

        // Sets homepoint so enemy knows where to return to.
        if (homePosition == Vector3.zero)
        {
            homePosition = this.transform.position;
        }
        agent = this.GetComponent<NavMeshAgent>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        playerHealth = player.gameObject.GetComponent<PlayerHealthController>();

        // Setting up Combat stats
        currentHealth = maxHealth;

        // Sphere Collider around Enemy
        rangeCollider = this.gameObject.AddComponent<SphereCollider>();
        rangeCollider.center = Vector3.zero;
        rangeCollider.radius = attackRange;
        rangeCollider.isTrigger = true;

        // Behaviour Tree to Control Behaviours
        tree = new BehaviourTree();

        Sequence enemyBehaviour = new Sequence("Enemy Behaviour");
        Leaf isEnemyAlive = new Leaf("Is the Enemy Alive?", isEnemyLiving);

        Selector enemyAction = new Selector("Enemy Action");

        Sequence patrol = new Sequence("Patrol Home Point");
        Leaf isPlayerInRange = new Leaf("Is Player in Range", isInRange);
        Leaf findWayPoint = new Leaf("Find Patrol Point", findPoint);
        Leaf moveToWayPoint = new Leaf("Roam around Home", moveToPoint);

        Sequence combat = new Sequence("Attack Player");
        Leaf ableToAttackPlayer = new Leaf("Player Seen", playerCanBeAttacked);

        // Selects between fighting and moving.
        Selector fightPlayer = new Selector("Player in Range");

        // Enemy decides if they are in range or need to move.
        Sequence moveToPlayer = new Sequence("Move to Player");
        Leaf ableToMove = new Leaf("Can move into Range", canMove);
        Leaf move = new Leaf("Moving to Player", enemyMove);

        // Enemy attacks if in range and insight of player.
        Leaf attack = new Leaf("Attack Player", attackPlayer);

        // Combat Related Leaves.
        moveToPlayer.AddChild(ableToMove);
        moveToPlayer.AddChild(move);

        fightPlayer.AddChild(attack);
        fightPlayer.AddChild(moveToPlayer);

        combat.AddChild(ableToAttackPlayer);
        combat.AddChild(fightPlayer);

        // Patrol Related Leaves.
        patrol.AddChild(isPlayerInRange);
        patrol.AddChild(findWayPoint);
        patrol.AddChild(moveToWayPoint);

        // Root Selector
        enemyAction.AddChild(combat);
        enemyAction.AddChild(patrol);

        enemyBehaviour.AddChild(isEnemyAlive);
        enemyBehaviour.AddChild(enemyAction);

        tree.AddChild(enemyBehaviour);


        tree.PrintTree();
    }

    public Node.Status isEnemyLiving()
    {
        if (currentHealth <= 0)
        {
            agent.SetDestination(this.transform.position);
            enemyAnimator.SetTrigger("isDead");
            Destroy(this.gameObject, 0.90f);
            return Node.Status.FAILURE;
        }
        return Node.Status.SUCCESS;
    }

    public Node.Status playerCanBeAttacked()
    {
        if (playerInRange)
        {
            return Node.Status.SUCCESS;
        }
        else if (canSeePlayer())
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    public Node.Status attackPlayer()
    {
        if (!playerInRange) return Node.Status.FAILURE;

        if (playerHealth.health == 0) return Node.Status.FAILURE;


        if (onCoolDown)
        {
            if (!waitingForCoolDown)
            {
                enemyAnimator.SetTrigger("isAttacking");
                int randomNumber = Random.Range(1, 3);
                enemyAnimator.SetInteger("pickAttack", randomNumber);

                if (player != null && playerHealth.health > 0)
                {
                    playerHealth.health -= attackDamage;
                    if (playerHealth.health < 0)
                    {
                        playerHealth.health = 0;
                    }
                }
                Invoke("coolDown", attackTimer);
                waitingForCoolDown = true;
            }
        }
        onCoolDown = true;
        //enemyAnimator.SetTrigger("isIdle");
        return Node.Status.SUCCESS;
    }

    public Node.Status canMove()
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        Vector3 playerPosition = player.transform.position;
        if (agent.CalculatePath(playerPosition, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            if (playerInRange) { return Node.Status.FAILURE; }
            return Node.Status.SUCCESS;
        }
        else
        {
            return Node.Status.FAILURE;
        }
    }

    public Node.Status enemyMove()
    {
        Node.Status s = GoToLocation(new Vector3(player.transform.position.x, 0, player.transform.position.z));
        return s;
    }

    public Node.Status isInRange()
    {
        if (playerInRange) return Node.Status.FAILURE;
        return Node.Status.SUCCESS;
    }

    public Node.Status findPoint()
    {
        roamGoal = Vector3.zero;

        float randomXPoint = Random.Range(-walkLimits.x, walkLimits.x);
        float randomZPoint = Random.Range(-walkLimits.z, walkLimits.z);

        roamGoal = new Vector3(homePosition.x + randomXPoint, 0, homePosition.z + randomZPoint);

        NavMeshPath navMeshPath = new NavMeshPath();
        if (agent.CalculatePath(roamGoal, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            return Node.Status.SUCCESS;
        }
        return Node.Status.FAILURE;
    }

    public Node.Status moveToPoint()
    {
        Node.Status s = GoToLocation(roamGoal);
        return s;
    }

    Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, this.transform.position);
        if (state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            enemyAnimator.SetBool("isWalking", true);
            state = ActionState.WORKING;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.IDLE;
            enemyAnimator.SetBool("isWalking", false);
            return Node.Status.FAILURE;
        }
        else if (distanceToTarget < 3 || canSeePlayer() || currentHealth <= 0)
        {
            state = ActionState.IDLE;
            enemyAnimator.SetBool("isWalking", false);
            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
    }

    bool canSeePlayer()
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = player.transform.position - this.transform.position;

        //Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        //Debug.DrawRay(transform.position, forward, Color.green, 5);

        // Check if Player is in sight of enemy instead.
        float lookAngle = Vector3.Angle(this.transform.forward, rayToTarget);
        if (lookAngle < 65 && Physics.Raycast(this.transform.position, rayToTarget, out raycastInfo))
        {
            if (raycastInfo.transform.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }


    public void takeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            enemyAnimator.SetTrigger("isHurt");
        }
    }

    void coolDown()
    {
        if (onCoolDown)
        {
            onCoolDown = false;
            waitingForCoolDown = false;
        }
    }

    void Update()
    {
        treeStatus = tree.Process();
    }

}