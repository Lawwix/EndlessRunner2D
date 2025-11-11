using UnityEngine;

public class FollowPlayerBackground : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 0.5f;

    private Vector3 offset;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        offset = transform.position - player.position;
    }

    void Update()
    {
        if (player != null)
        {
            // Фон следует за игроком по X
            Vector3 targetPosition = new Vector3(player.position.x + offset.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}