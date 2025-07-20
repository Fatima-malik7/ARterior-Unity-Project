using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public Button togglePasswordButton;
    public TextMeshProUGUI toggleButtonText;

    private bool isPasswordVisible = false;

    private string loginURL = "http://192.168.1.12:5000/api/users/login";
    public GameObject popupPanel;
    public TextMeshProUGUI popupText;

    void ShowPopup(string message)
    {
        popupText.text = message;
        popupPanel.SetActive(true);
    }

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
        togglePasswordButton.onClick.AddListener(TogglePasswordVisibility);
        passwordInput.contentType = TMP_InputField.ContentType.Password;
        passwordInput.ForceLabelUpdate();
    }

    void OnLoginClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        StartCoroutine(SendLoginRequest(email, password));
    }

    void TogglePasswordVisibility()
    {
        isPasswordVisible = !isPasswordVisible;
        passwordInput.contentType = isPasswordVisible
            ? TMP_InputField.ContentType.Standard
            : TMP_InputField.ContentType.Password;
        toggleButtonText.text = isPasswordVisible ? "Hide" : "Show";
        passwordInput.ForceLabelUpdate();
    }

    IEnumerator SendLoginRequest(string email, string password)
    {
        ShowPopup("Logging in...");

        string jsonBody = JsonUtility.ToJson(new LoginData(email, password));
        byte[] jsonToSend = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        UnityWebRequest request = new UnityWebRequest(loginURL, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;

            if (request.responseCode == 200 && responseText.Contains("Login successful"))
            {
                ShowPopup("Login Successful!");
                yield return new WaitForSeconds(1.5f);
                SceneManager.LoadScene("HomeScene");
            }
            else
            {
                ShowPopup("Login Failed: Invalid email or password");
            }
        }
        else
        {
            ShowPopup("Network Error: " + request.error);
        }
    }

    [System.Serializable]
    public class LoginData
    {
        public string email;
        public string password;
        public LoginData(string e, string p)
        {
            email = e;
            password = p;
        }
    }
}
