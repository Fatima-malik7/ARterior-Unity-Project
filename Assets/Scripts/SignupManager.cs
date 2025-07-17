using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class SignupManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField mobileInput;
    public TextMeshProUGUI messageText;

    private string registerUrl = "http://192.168.1.12:5000/api/users/register"; // Replace with your IP address

    public void OnSignupButtonClicked()
    {
        StartCoroutine(SignupUser());
    }

    IEnumerator SignupUser()
    {
        // Create user JSON
        string jsonData = JsonUtility.ToJson(new SignupData
        {
            username = usernameInput.text,
            email = emailInput.text,
            mobile = mobileInput.text,
            password = passwordInput.text
        });

        // Send POST request
        UnityWebRequest request = new UnityWebRequest(registerUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Signup successful!");
            messageText.text = "Signup successful!";
        }
        else
        {
            Debug.LogError("Signup failed: " + request.error);
            messageText.text = "Signup failed: " + request.downloadHandler.text;
        }
    }

    [System.Serializable]
    class SignupData
    {
        public string username;
        public string email;
        public string mobile;
        public string password;
    }
}
