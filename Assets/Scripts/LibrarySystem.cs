using UnityEngine;
using Firebase.Database;
using Firebase;
using TMPro;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Linq;
using Firebase.Extensions;


public class LibrarySystem : MonoBehaviour
{
    [Serializable]
    public class Book
    {
        public string name;
        public string ISBN;
        public string publisher;
        public string author;
        public bool IsOnLoan;
    }

    public string dbUrl = "https://robotics-5gi-c2661-default-rtdb.asia-southeast1.firebasedatabase.app/";
    public string id = "admin";
    public string pw = "admin";
    public bool isLoggedIn = false;
    public TMP_InputField idInput;
    public TMP_InputField pwInput;
    public TMP_InputField searchInput;
    public TMP_Text logTxt;
    DatabaseReference dbRef;
    public List<Book> books = new List<Book>();

    private void Start()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri(dbUrl);

        dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        print("Library System is Ready to Search! Please Log-in first.");
        logTxt.text = ("\"Library System is Ready to Search! Please Log-in first.");
    }

    public void OnLogInBtnClkEvent()
    {
        if (idInput.text != id || pwInput.text != pw)
        {
            print("Id or Password is incorrect.");
            logTxt.text = ("Id or Password is incorrect.");
            return;
        }

        print($"Hello! {id}. Start searching book!");
        logTxt.text = ($"Hello! {id}. Start searching book!");

        isLoggedIn = true;
    }

    public void OnSearchBtnClkEvent()
    {
        if (!isLoggedIn)
        {
            print("Please log-in first.");
            logTxt.text = ("Please log-in first.");
            return;
        }

        if(searchInput.text == string.Empty || !isLoggedIn)
        {
            print("Please enter the name of the book.");
            logTxt.text = ("Please enter the name of the book.");
            return;
        }

        dbRef.Child("Books").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsCanceled)
            {
                print("DB read canceled. Please try again.");
                logTxt.text = ("DB read canceled. Please try again.");
                return;
            }
            else if(task.IsFaulted)
            {
                print("DB read faulted. Please contact the manager.");
                logTxt.text = ("DB read faulted. Please contact the manager.");
                return;
            }
            else if(task.IsCompleted)
            {
                print("DB read is completed!");
                logTxt.text = ("DB read is completed!");

                DataSnapshot snapShot = task.Result;

                string json = snapShot.GetRawJsonValue();

                books = JsonConvert.DeserializeObject<List<Book>>(json);

                var bookFound = from item in books
                            where item.name == searchInput.text
                            select item;

                print("Book is found!");
                string result = "";
                foreach(var item in bookFound)
                {
                    result += $"Name: {item.name}\nISBN: {item.ISBN}\nPublisher: {item.publisher}\nAuthor: {item.author}\nisRent:{item.IsOnLoan}\n";
                }

                if(result == string.Empty)
                {
                    result = "No Book Found";
                }

                print(result);
                logTxt.text = result;
            }
        });
    }
}
