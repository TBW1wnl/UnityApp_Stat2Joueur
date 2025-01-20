using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections;

public class LoginUIManager : MonoBehaviour
{
    private TextField emailField;
    private TextField passwordField;
    private Button validateButton;

    private string apiUrl = "http://localhost/api/login";

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        emailField = root.Q<TextField>("email");
        passwordField = root.Q<TextField>("password");
        validateButton = root.Q<Button>("login");

        validateButton.clicked += OnValidateButtonClicked;
    }

    private void OnValidateButtonClicked()
    {
        string email = emailField.value;
        string password = passwordField.value;

        Debug.Log($"Email: {email}, Password: {password}");

        StartCoroutine(ValidateLoginAsync(email, password, (message, token) =>
        {
            if (token != null)
            {
                // Connexion réussie
                Debug.Log("Login réussi ! Token: " + token);
                GameManager.Instance.LoadScene("StatisticsScene");
            }
            else
            {
                // Connexion échouée
                Debug.Log("Login échoué: " + message);
            }
        }));
    }

    /// <summary>
    /// Fonction asynchrone pour valider les identifiants via l'API
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="onComplete"></param>
    /// <returns></returns>
    private IEnumerator ValidateLoginAsync(string email, string password, System.Action<string, string> onComplete)
    {
        var loginData = new LoginData
        {
            email = email,
            password = password
        };

        string jsonData = JsonUtility.ToJson(loginData);

        // Créer la requête POST
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Attendre la réponse de l'API
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Traiter la réponse JSON
            string jsonResponse = request.downloadHandler.text;
            ApiResponse response = JsonUtility.FromJson<ApiResponse>(jsonResponse);

            // Appeler le callback avec le message et le token
            onComplete?.Invoke(response.message, response.token);
        }
        else
        {
            onComplete?.Invoke("Erreur de connexion: " + request.error, null);
        }
    }

    /// <summary>
    /// Classe représentant les données de login envoyées à l'API
    /// </summary>
    [System.Serializable]
    public class LoginData
    {
        public string email;
        public string password;
    }

    /// <summary>
    /// Classe représentant la réponse de l'API
    /// </summary>
    [System.Serializable]
    public class ApiResponse
    {
        public string message;
        public string token;
    }
}
