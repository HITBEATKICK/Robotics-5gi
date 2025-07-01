using UnityEngine;
using Firebase.Database;
using Firebase;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

// Firebase DB에 접속, 데이터를 읽고 쓴다.
public class FirebaseDBEx : MonoBehaviour
{
    [Serializable]
    public class Factory
    {
        public List<Robot> robots = new List<Robot>();
    }

    [Serializable]
    public class Robot
    {
        public string name;
        public int id;
        public string serialNum;
        public string managerName;
        public float cycleTime;

        // Json.Net 에서는 UnityEngine.Vector3 사용불가
        //public List<Vector3> steps = new List<Vector3>();
        public List<Step> steps = new List<Step>();
    }

    [Serializable]
    public struct Step
    {
        public float posX;
        public float posY;
        public float posZ;
        public float rotX;
        public float rotY;
        public float rotZ;
        public float duration;
        public bool isGripperOn;
    }


    [SerializeField] string dbURL;
    public Factory factory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Firebase URL 연동
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri(dbURL);

        // DB에서 가장 상위 폴더
        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        // 1. DB에 데이터 읽기 요청
        GetDBData(dbRef);

        // 2. DB에 데이터 쓰기 요청
        string json = @"
{
    ""address"":""구로구"",
    ""id"":12345
}";
        //dbRef.SetValueAsync(json);          // 문자열 자체를 넣기
        //dbRef.SetRawJsonValueAsync(json).ContinueWith(task =>
        //{
        //    if (task.IsCanceled)
        //    {
        //        print("데이터 설정 취소");
        //    }
        //    else if (task.IsFaulted)
        //    {
        //        print("데이터 설정 실패");
        //    }
        //    else if (task.IsCompleted)
        //    {
        //        print("데이터 설정 완료");
        //    }
        //});   // json 파일 포멧으로 넣기

        dbRef.Child("Books").GetValueAsync().ContinueWith(task =>
        {

        });

        // 객체(object) -> Json 포멧 변경
        SetDBRobotData(dbRef);

        GetDBRobotData(dbRef);
    }

    // Firebase DB RootReference의 데이터를 역직렬화 -> 객체로 변환
    private void GetDBRobotData(DatabaseReference dbRef)
    {
        dbRef.GetValueAsync().ContinueWith(task =>
        {
            if(task.IsCanceled)
            {

            }
            else if(task.IsFaulted)
            {

            }
            else if(task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;

                string json = snapShot.GetRawJsonValue();

                factory = JsonConvert.DeserializeObject<Factory>(json);
            }
        });
    }

    private static void SetDBRobotData(DatabaseReference dbRef)
    {
        Robot robotA = new Robot
        {
            name = "Cobot1",
            id = 0,
            serialNum = "123456",
            managerName = "신태욱",
            cycleTime = 0
        };

        Step step0 = new Step()
        {
            posX = 1,
            posY = 2,
            posZ = 3,
            rotX = 50,
            rotY = 23,
            rotZ = 66,
            duration = 3,
            isGripperOn = true,
        };
        robotA.steps.Add(step0);
        robotA.steps.Add(step0);
        robotA.steps.Add(step0);

        Factory factory = new Factory();
        factory.robots.Add(robotA);
        factory.robots.Add(robotA);
        factory.robots.Add(robotA);

        // JsonUtility: 1개의 레이어의 계층구조를 가진 클래스 까지 변환가능
        // 예시) Robot 클래스 json으로 변환 가능, Factory 클래스는 변환 불가


        // 객체(object) -> Json 포멧 변경
        //string jsonRobot = JsonUtility.ToJson(factory);

        // json.Net 클래스(직렬화: Object -> Json)
        string jsonRobot = JsonConvert.SerializeObject(factory);
        dbRef.SetRawJsonValueAsync(jsonRobot).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                print("Factory Data 업로드 완료");
            }
        });
    }

    private static void GetDBData(DatabaseReference dbRef)
    {
        dbRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;

                print(snapShot.GetRawJsonValue());

                foreach (var item in snapShot.Children)
                {
                    string json = item.GetRawJsonValue();

                    //print($"{item.Key}: {json}");
                    print($"{item.Key}: {item.Value}");
                }
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
