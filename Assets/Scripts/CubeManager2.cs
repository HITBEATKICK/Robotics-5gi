using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CubeWaveManager : MonoBehaviour
{
    public float speed = 10f; // 큐브 이동 속도
    public float interval = 1f; // 큐브 출발 간격(초)
    public List<GameObject> cubes; // 인스펙터에서 cube1, cube2, cube3, ... 할당
    public List<GameObject> targets; // 인스펙터에서 CylinderA, B, C, D 순서대로 할당

    void Start()
    {
        // 코루틴 시작
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
        print(cube.name + " 출발!");

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
        print(cube.name + " 경로 완주!");
    }
}
