using UnityEngine;
using Firebase;
using Firebase.Database;
using TMPro;
using System;
using System.Collections;
using System.Threading.Tasks;

public class BulletinBoardManager : MonoBehaviour
{
    [Serializable]
    public class Post
    {
        public string userName;
        public string content;
        public long timestamp; // Unix Timestamp (ms)로 시간 저장

        public string ToFormattedString()
        {
            // timestamp를 사람이 읽을 수 있는 시간으로 변환
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(timestamp).ToLocalTime(); // 로컬 시간으로 변환

            return $"[{userName}] ({dateTime:yyyy-MM-dd HH:mm})\n{content}\n" +
                   "--------------------------------\n";
        }
    }

    public string dbUrl;
    public TMP_InputField publicTxt;
    public TMP_InputField privateTxt;
    public TMP_InputField nameTxt;
    public TMP_InputField dateTimeTxt;
    DatabaseReference dbRef;

    private void Awake()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri(dbUrl);

        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("LoadData", 1);
    }

    void LoadData()
    {
        StartCoroutine(ReadPublicData());

        StartCoroutine(ReadPrivateData(FirebaseAuthManager.instance.user.UserId));
    }

    public IEnumerator ReadPublicData()
    {
        string content = "";

        Task task = dbRef.Child("PublicData").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;

                content = snapShot.GetRawJsonValue();

                print(content);
            }
        });

        yield return new WaitUntil(() => task.IsCompleted && content != "");

        publicTxt.text = content;

    }

    public IEnumerator ReadPrivateData(string uID)
    {
        string content = "";

        Task t = dbRef.Child("PrivateData").Child(uID).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;

                content = snapShot.GetRawJsonValue();

                print(content);
            }
        });


        yield return new WaitUntil(() => t.IsCompleted && content != "");

        privateTxt.text = content;
    }

    public void OnPublicDataWriteBtnClkEvent()
    {
        string content = publicTxt.text;

        dbRef.Child("PublicData").SetValueAsync(content).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {

            }
        });
    }

    public void OnPrivateDataWriteBtnClkEvent()
    {

    }
}
