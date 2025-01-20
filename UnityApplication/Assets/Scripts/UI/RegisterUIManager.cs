using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class RegisterUIManager : MonoBehaviour
{
    // Champs de l'UI
    private TextField emailField;
    private TextField passwordField;
    private TextField nameField;
    private Button validateButton;
    private Button loginButton;

    private string apiUrl = "http://localhost/api/register";

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        emailField = root.Q<TextField>("email");
        passwordField = root.Q<TextField>("password");
        nameField = root.Q<TextField>("name");
        validateButton = root.Q<Button>("register");
        loginButton = root.Q<Button>("login");

        validateButton.clicked += OnValidateButtonClicked;
        loginButton.clicked += LoadSceneLogin;
    }

    private void LoadSceneLogin()
    {
        GameManager.Instance.LoadScene("LoginScene");
    }

    private void OnValidateButtonClicked()
    {
        string email = emailField.value.Trim();
        string password = passwordField.value.Trim();
        string name = nameField.value.Trim();

        Debug.Log($"Email: {email}, Password: {password}, Name: {name}");

        if (IsValidInput(email, password, name))
        {
            StartCoroutine(RegisterUser(email, password, name));
        }
        else
        {
            Debug.LogError("Données invalides. Assurez-vous que tous les champs sont remplis.");
        }
    }

    private bool IsValidInput(string email, string password, string name)
    {
        return !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(name);
    }

    private IEnumerator RegisterUser(string email, string password, string name)
    {
        var jsonData = new RegisterData
        {
            email = email,
            password = password,
            name = name
        };

        string json = JsonUtility.ToJson(jsonData);
        Debug.Log($"JSON envoyé : {json}");

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.LogError($"Erreur de connexion : {request.error}");
        }
        else
        {
            Debug.Log($"Code de réponse : {request.responseCode}");
            Debug.Log($"Réponse : {request.downloadHandler.text}");

            if (request.responseCode == 201)
            {
                Debug.Log("Inscription réussie !");
                GameManager.Instance.LoadScene("LoginScene");
            }
            else
            {
                Debug.LogError($"Erreur lors de l'inscription : {request.downloadHandler.text}");
            }
        }
    }
}

// Classe pour représenter les données d'inscription
[System.Serializable]
public class RegisterData
{
    public string email;
    public string password;
    public string name;
}
