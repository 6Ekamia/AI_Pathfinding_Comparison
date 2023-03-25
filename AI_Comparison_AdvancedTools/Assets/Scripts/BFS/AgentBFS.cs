using UnityEngine;

public class AgentBFS : MonoBehaviour
{
    public float speed = 5f;
    public float nextWaypointDistance = 1f;

    private int currentWaypointIndex = 0;
    private Vector3[] path;
    private Vector3 targetPosition;
    BFSPathfinding pathfinding;

    bool startCounting = false;
    float time = 0;

    private void Start()
    {
        pathfinding = GetComponent<BFSPathfinding>();
        pathfinding.pathReady.AddListener(SetPath);
        targetPosition = pathfinding.goal.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            pathfinding.FindPath(transform.position, targetPosition);
            startCounting = true;
        }

        if (startCounting) time += Time.deltaTime;

        if(path != null)
        {
            startCounting = false;
            Debug.Log("Time took to find path: " + time);
        }
    }

    private void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypointIndex >= path.Length)
        {
            return;
        }

        Vector3 direction = (path[currentWaypointIndex] - transform.position).normalized;
        Vector3 velocity = direction * speed;
        transform.position += velocity * Time.fixedDeltaTime;

        if (Vector3.Distance(transform.position, path[currentWaypointIndex]) < nextWaypointDistance)
        {
            currentWaypointIndex++;
        }
    }

    public void SetPath(Vector3[] newPath)
    {
        path = newPath;
        currentWaypointIndex = 0;
    }
}
