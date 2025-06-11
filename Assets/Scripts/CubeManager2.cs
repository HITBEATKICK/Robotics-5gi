using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CubeWaveManager : MonoBehaviour
{
    public float speed = 10f; // ť�� �̵� �ӵ�
    public float interval = 1f; // ť�� ��� ����(��)
    public List<GameObject> cubes; // �ν����Ϳ��� cube1, cube2, cube3, ... �Ҵ�
    public List<GameObject> targets; // �ν����Ϳ��� CylinderA, B, C, D ������� �Ҵ�

    void Start()
    {
        // �ڷ�ƾ ����
        StartCoroutine(SpawnCubesWithDelay());
    }

    IEnumerator SpawnCubesWithDelay()
    {
        foreach (var cube in cubes)
        {
            StartCoroutine(MoveCubeToTargets(cube, targets));
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator MoveCubeToTargets(GameObject cube, List<GameObject> targets)
    {
        int index = 0;
        print(cube.name + " ���!");

        while (index < targets.Count)
        {
            Vector3 direction = targets[index].transform.position - cube.transform.position;
            Vector3 normalizedDir = direction.normalized;

            float distance = Vector3.Magnitude(direction);

            if (distance < 0.1f)
            {
                index++;
                if (index == targets.Count) break;
                continue;
            }

            cube.transform.position += normalizedDir * speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        print(cube.name + " ��� ����!");
    }
}
