using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    BehaviourTree tree;
    public GameObject player;
    public bool onCoolDown = false;
    private bool waitingForCoolDown = false;
    private bool playerInRange = false;
    private int coolDownTimer;
    private SphereCollider rangeCollider;

    [Header("Combat Settings")]
    public int currentHealth = 100;
    public int maxHealth = 100;
    public int attackTimer = 1;
    public float attackRange = 2;

    [Header("Roam Settings")]
    Vector3 roamGoal;
    public Vector3 walkLimits = new Vector3(30, 30, 30);

    private Vector3 homePosition = Vector3.zero;

    NavMeshAgent agent;
    public enum ActionState { IDLE, WORKING};
    ActionState state = ActionState.IDLE;

    Node.Status treeStatus = Node.Status.RUNNING;

    void Start()
    {
        //Time.timeScale = 5;

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

        // Setting up Combat stats
        currentHealth = maxHealth;

        // Sphere Collider around Enemy
        rangeCollider = this.gameObject.AddComponent<SphereCollider>();
        rangeCollider.center = Vector3.zero;
        rangeCollider.radius = attackRange;
        rangeCollider.isTrigger = true;


        // Behaviour Tree to Control Behaviours
        tree = new BehaviourTree();

        Selector enemyAction = new Selector("Enemy Action");

        Sequence patrol = new Sequence("Patrol Home Point");
        Leaf isPlayerInRange = new Leaf("Is Player in Range", isInRange);
        Leaf findWayPoint = new Leaf("Find Patrol Point", findPoint);
        Leaf moveToWayPoint = new Leaf("Roam around Home", moveToPoint);

        Sequence combat = new Sequence("Attack Player");
        Leaf playerInSight = new Leaf("Player Seen", playerSeen);

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

        combat.AddChild(playerInSight);
        combat.AddChild(fightPlayer);

        // Patrol Related Leaves.
        patrol.AddChild(isPlayerInRange);
        patrol.AddChild(findWayPoint);
        patrol.AddChild(moveToWayPoint);
        
        // Root Selector
        enemyAction.AddChild(combat);
        enemyAction.AddChild(patrol);

        tree.AddChild(enemyAction);


        tree.PrintTree();
    }

    public Node.Status playerSeen()
    {

        RaycastHit raycastInfo;
        Vector3 rayToTarget = player.transform.position - this.transform.position;

        // Check Distance to target first (Replicating "hearing" player)
        if (playerInRange && Physics.Raycast(this.transform.position, rayToTarget, out raycastInfo))
        {
            if (raycastInfo.transform.gameObject.tag == "Player")
            {
                return Node.Status.SUCCESS;
            }
        }

        // Check if Player is in sight of enemy instead.
        float lookAngle = Vector3.Angle(this.transform.forward, rayToTarget);
        if (lookAngle < 90 && Physics.Raycast(this.transform.position, rayToTarget, out raycastInfo))
        {
            if (raycastInfo.transform.gameObject.tag == "Player")
            {
                return Node.Status.SUCCESS;
            }
        }
        return Node.Status.FAILURE;
    }

    public Node.Status attackPlayer()
    {
        if (!playerInRange) return Node.Status.FAILURE;

        if (onCoolDown)
        {
            if (!waitingForCoolDown)
            {
                Invoke("coolDown", attackTimer);
                waitingForCoolDown = true;
            }
            return Node.Status.FAILURE;
        }


        RaycastHit raycastInfo;
        Vector3 rayToTarget = player.transform.position - this.transform.position;

        if (playerInRange && Physics.Raycast(this.transform.position, rayToTarget, out raycastInfo))
        {
            if (raycastInfo.transform.gameObject.tag == "Player")
            {
                this.transform.LookAt(player.transform.position);
                Debug.Log("Attack");
                onCoolDown = true;
                return Node.Status.SUCCESS;
            }
        }
        return Node.Status.FAILURE;
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
            state = ActionState.WORKING;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if ( distanceToTarget < 3)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
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
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    void Update()
    {
        treeStatus = tree.Process();
    }

    void coolDown()
    {
        if (onCoolDown)
        {
            onCoolDown = false;
            waitingForCoolDown = false;
        }
    }


}
