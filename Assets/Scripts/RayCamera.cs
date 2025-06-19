using UnityEngine;

public class RayCamera : MonoBehaviour
{
    Ray ray;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���ʹ�ư Ŭ����
        {

            // ��ũ�� �����̽��� �� ������ ���콺�� ����� ��
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo = new RaycastHit(); // ���� �߻� ��, �浹 ���� ����

            // ������ origin -> direction �߻����� ��, �浹�� �ִٸ�
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                print(hitInfo.collider.name);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(ray.origin, ray.direction * 100);
    }
}
