using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// Cube 1,2,3,4,5�� ������� 1�� �������� ����ϱ�
// Cylinder A B C D ������ �̵�
public class CubeManager_ : MonoBehaviour
{
    public float speed;
    public GameObject cube;
    public GameObject cube1;
    public GameObject cube2;
    public GameObject cube3;
    public GameObject cube4;
    public List<GameObject> targets;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CoStart());
    }

    // Update is called once per frame
    void Update()
    {
        print("Update");
    }

    // �ڷ�ƾ �޼��� : ���μ��� ������ ��� ��ٸ� �� �ִ� ���
    IEnumerator CoStart()
    {
        print("����!");
        yield return MoveCubeToTargets(cube, targets);

        yield return new WaitForSeconds(1);

        print("1�� ����!");
        yield return MoveCubeToTargets(cube1, targets);

        yield return new WaitForSeconds(1);

        print("�� 1�� ����!");
        yield return MoveCubeToTargets(cube2, targets);

        yield return new WaitForSeconds(1);

        print("�� 1�� ����!");
        yield return MoveCubeToTargets(cube3, targets);

        yield return new WaitForSeconds(1);

        print("�� 1�� ����!");
        yield return MoveCubeToTargets(cube4, targets);

        yield return new WaitForSeconds(1);

        print("��!");
    }



    IEnumerator MoveCubeToTargets(GameObject cube, List<GameObject> targets)
    {
        int index = 0;

        print(cube.gameObject.name + "���!");

        while (true)
        {
            Vector3 direction = targets[index].transform.position - cube.transform.position;
            Vector3 normalizedDir = direction.normalized;

            float distance = Vector3.Magnitude(direction);

            if (distance < 0.1f)
            {
                index++;

                if(index == targets.Count)
                {
                    break;
                }
            }

            cube.transform.position += normalizedDir * speed * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
