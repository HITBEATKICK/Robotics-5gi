using UnityEngine;

public class MovementEx3 : MonoBehaviour
{
    public GameObject[] targets; // A, B, C, D
    [SerializeField] private float speed = 2f;

    private int currentTargetIndex = 0;

    void Update()
    {
        MoveNext(transform.gameObject, targets[currentTargetIndex]);
    }

    private void MoveNext(GameObject start, GameObject end)
    {
        Vector3 direction = (end.transform.position - start.transform.position).normalized;
        float distance = Vector3.Distance(start.transform.position, end.transform.position);

        if (distance < 0.1f)
        {
            // 다음 목표 지점으로 전환
            currentTargetIndex = (currentTargetIndex + 1) % targets.Length;
            return;
        }

        transform.position += direction * (speed * Time.deltaTime);
    }
}