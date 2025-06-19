using UnityEngine;
using UnityEngine.AI;

// ĸ���� �׺���̼� �Ž��� Ư�� ��ġ�� �̵��Ѵ�.
// �Ӽ�: target transform
public class NavAgent : MonoBehaviour
{
    public Transform target;
    public Transform target2;
    NavMeshAgent agent;
    Ray ray;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = 10;
        agent.angularSpeed = 1000;

        agent.SetDestination(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            agent.SetDestination(target2.position);
        }

        if (Input.GetMouseButtonDown(0)) // ���콺 ���ʹ�ư Ŭ����
        {

            // ��ũ�� �����̽��� �� ������ ���콺�� ����� ��
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo = new RaycastHit(); // ���� �߻� ��, �浹 ���� ����

            // ������ origin -> direction �߻����� ��, �浹�� �ִٸ�
            if(Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                agent.destination = hitInfo.point;
            }
        }
    }
}
