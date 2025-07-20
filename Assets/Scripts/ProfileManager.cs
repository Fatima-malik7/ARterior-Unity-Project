using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class ProfileManager : MonoBehaviour
{
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI emailText;
    public TextMeshProUGUI mobileText;

    private string profileUrl = "http://192.168.0.106:5000/api/users/me";

    void Start()
    {
        string token = PlayerPrefs.GetString("jwtToken");
        Debug.Log("Token from PlayerPrefs: " + token);

        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("JWT token not found");
            return;
        }

        StartCoroutine(GetProfile(token));
    }

    IEnumerator GetProfile(string token)
    {
        UnityWebRequest request = UnityWebRequest.Get(profileUrl);
        request.SetRequestHeader("Authorization", "Bearer " + token);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log("Profile JSON: " + json);

            // Wrap it for Unity JSON parsing
            string wrappedJson = "{ \"user\": " + json + " }";
            UserWrapper wrapper = JsonUtility.FromJson<UserWrapper>(wrappedJson);

            if (wrapper != null && wrapper.user != null)
            {
                usernameText.text = wrapper.user.username;
                emailText.text = wrapper.user.email;
                mobileText.text = wrapper.user.mobile;
            }
            else
            {
                Debug.LogError("Failed to parse profile data.");
            }
        }
        else
        {
            Debug.LogError("Profile fetch failed: " + request.error);
        }
    }

    [System.Serializable]
    public class UserWrapper
    {
        public UserProfile user;
    }

    [System.Serializable]
    public class UserProfile
    {
        public string username;
        public string email;
        public string mobile;
    }
}
