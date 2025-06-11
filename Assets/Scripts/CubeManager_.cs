using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// Cube 1,2,3,4,5를 순서대로 1초 간격으로 출발하기
// Cylinder A B C D 순서로 이동
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

    // 코루틴 메서드 : 프로세스 내에서 잠깐 기다릴 수 있는 기능
    IEnumerator CoStart()
    {
        print("시작!");
        yield return MoveCubeToTargets(cube, targets);

        yield return new WaitForSeconds(1);

        print("1초 지남!");
        yield return MoveCubeToTargets(cube1, targets);

        yield return new WaitForSeconds(1);

        print("또 1초 지남!");
        yield return MoveCubeToTargets(cube2, targets);

        yield return new WaitForSeconds(1);

        print("또 1초 지남!");
        yield return MoveCubeToTargets(cube3, targets);

        yield return new WaitForSeconds(1);

        print("또 1초 지남!");
        yield return MoveCubeToTargets(cube4, targets);

        yield return new WaitForSeconds(1);

        print("끝!");
    }



    IEnumerator MoveCubeToTargets(GameObject cube, List<GameObject> targets)
    {
        int index = 0;

        print(cube.gameObject.name + "출발!");

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
