using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Profile : MonoBehaviour
{
    public InputField nameInput;
    public InputField emailInput;
    public InputField phoneInput;
    public Button updateButton;

    // URL format: http://<your-ip>:5000/api/auth/update/<userId>
    public string updateUrlBase = "http://192.168.0.106:5000/api/auth/update/";


    void Start()
    {
        // Populate fields with data from global storage (from login)
        nameInput.text = UserData.username;
        emailInput.text = UserData.email;
        phoneInput.text = UserData.phone;

        // Assign button action
        updateButton.onClick.AddListener(OnUpdateClick);
    }

    void OnUpdateClick()
    {
        StartCoroutine(UpdateUserInfo());
    }

    IEnumerator UpdateUserInfo()
    {
        string url = updateUrlBase + UserData.id;

        WWWForm form = new WWWForm();
        form.AddField("username", nameInput.text);
        form.AddField("email", emailInput.text);
        form.AddField("phone", phoneInput.text);

        UnityWebRequest req = UnityWebRequest.Put(url, form.data);
        req.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Profile updated in MongoDB Atlas!");
        }
        else
        {
            Debug.LogError("Update failed: " + req.error);
        }
    }
}
