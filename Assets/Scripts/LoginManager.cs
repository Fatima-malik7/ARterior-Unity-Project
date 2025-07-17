using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public TextMeshProUGUI statusText;

    private string loginURL = "http://192.168.1.12:5000/api/users/login"; // replace with your actual URL

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
    }

    void OnLoginClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;
        StartCoroutine(SendLoginRequest(username, password));
    }

    IEnumerator SendLoginRequest(string username, string password)
    {
        statusText.text = "Logging in...";

        // Prepare JSON
        string jsonBody = JsonUtility.ToJson(new LoginData(username, password));
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonBody);

        UnityWebRequest request = new UnityWebRequest(loginURL, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Login Success: " + request.downloadHandler.text);
            statusText.text = "Login Successful!";
            // Navigate to next scene if needed
        }
        else
        {
            Debug.LogError("Login Failed: " + request.error);
            statusText.text = "Login Failed: " + request.error;
        }
    }

    // Login data structure
    [System.Serializable]
    public class LoginData
    {
        public string username;
        public string password;

        public LoginData(string u, string p)
        {
            username = u;
            password = p;
        }
    }
}
