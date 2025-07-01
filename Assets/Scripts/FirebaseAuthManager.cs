using UnityEngine;
using Firebase.Auth;
using TMPro;
using System.Threading.Tasks;
using Firebase;
using System.Net.Mail;
using UnityEditor;

// 1. 로그인: 이메일, 패스워드 입력시 회원가입 여부에 따라 로그인
// 2. 회원가입: 이메일, 패스워드 입력 후 이메일 인증이 된다면, 회원가입 완료!
// 3. DB정보 불러오기: 권한에 따라 DB의 특정 정보를 가져온다.
public class FirebaseAuthManager : MonoBehaviour
{
    public TMP_InputField signInEmailInput;
    public TMP_InputField signInPasswordInput;

    public TMP_InputField signUpEmailInput;
    public TMP_InputField signUpPasswordInput;
    public TMP_InputField signUpPasswordCheckInput;

    // class diagram
    // + Initialization(초기화)
    // + SignIn(로그인)
    // + SignUp(회원가입)
    // + SendVerificationEmail(이메일인증)
    FirebaseAuth auth;
    FirebaseUser user;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private void OnDestroy()
    {
        auth.SignOut();
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
                && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                Debug.Log(user.DisplayName ?? "");
                Debug.Log(user.Email ?? "");
                Debug.Log(user.IsEmailVerified);
                Debug.Log(user.PhotoUrl);
            }
        }
    }

    public void SignIn()
    {
        if(signInEmailInput.text == string.Empty)
        {
            print("Please enter your email.");
            return;
        }
        else if(signInPasswordInput.text == string.Empty)
        {
            print("Please enter your password.");
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(signInEmailInput.text, signInPasswordInput.text)
            .ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
            });
    }

    public void SignUp()
    {
        if(signUpEmailInput.text == string.Empty || signUpPasswordInput.text == string.Empty || signUpPasswordCheckInput.text == string.Empty)
        {
            print("Email or password or password check is empty.");
            return;
        }

        if(signUpPasswordInput.text != signUpPasswordCheckInput.text)
        {
            print("Password is incorrect");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(signUpEmailInput.text, signUpPasswordInput.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            user.SendEmailVerificationAsync().ContinueWith(task =>
            {
                if(task.IsCompleted)
                {
                    print("이메일을 확인하여 인증을 끝마쳐 주세요.");
                }
            });
        });
    }

}
