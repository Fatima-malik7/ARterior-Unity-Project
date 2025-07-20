using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class SignupManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField mobileInput;
    public Button signupButton;
    public Button togglePasswordButton;
    public TextMeshProUGUI toggleButtonText;

    private bool isPasswordVisible = false;
    private string registerUrl = "http://192.168.1.12:5000/api/users/register";
    public GameObject popupPanel;
    public TextMeshProUGUI popupText;

    void ShowPopup(string message)
    {
        popupText.text = message;
        popupPanel.SetActive(true);
    }

    void Start()
    {
        signupButton.onClick.AddListener(OnSignupButtonClicked);
        togglePasswordButton.onClick.AddListener(TogglePasswordVisibility);
        passwordInput.contentType = TMP_InputField.ContentType.Password;
        passwordInput.ForceLabelUpdate();
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

    void OnSignupButtonClicked()
    {
        StartCoroutine(SignupUser());
    }

    IEnumerator SignupUser()
    {
        ShowPopup("Creating account...");

        string jsonData = JsonUtility.ToJson(new SignupData
        {
            username = usernameInput.text,
            email = emailInput.text,
            mobile = mobileInput.text,
            password = passwordInput.text
        });

        UnityWebRequest request = new UnityWebRequest(registerUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            if (request.responseCode == 201)
            {
                ShowPopup("Signup successful!");
            }
            else if (request.responseCode == 409)
            {
                ShowPopup("Email or mobile already registered");
            }
            else
            {
                ShowPopup("Unexpected server response");
            }
        }
        else
        {
            ShowPopup("Signup failed: " + request.error);
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