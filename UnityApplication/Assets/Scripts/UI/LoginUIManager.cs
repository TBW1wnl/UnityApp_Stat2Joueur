using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections;

public class LoginUIManager : MonoBehaviour
{
    private TextField emailField;
    private TextField passwordField;
    private Button validateButton;
    private Button registerButton;

    private string apiUrl = "http://localhost/api/login";

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        emailField = root.Q<TextField>("email");
        passwordField = root.Q<TextField>("password");
        validateButton = root.Q<Button>("login");
        registerButton = root.Q<Button>("register");

        validateButton.clicked += OnValidateButtonClicked;
        registerButton.clicked += registerButton;
    }

    private void registerButton()
    {
        GameManager.Instance.LoadScene("RegisterScene");
    }

    private void OnValidateButtonClicked()
    {
        string email = emailField.value;
        string password = passwordField.value;

        Debug.Log($"Email: {email}, Password: {password}");

        StartCoroutine(ValidateLoginAsync(email, password, (message, token, userId) =>
        {
            if (token != null)
            {
                // Connexion r�ussie
                Debug.Log($"Login r�ussi ! Token: {token}, ID de l'utilisateur: {userId}");

                GameManager.Instance.token = token;
                GameManager.Instance.userId = userId;

                // Charger la sc�ne des statistiques
                GameManager.Instance.LoadScene("StatisticsScene");
            }
            else
            {
                // Connexion �chou�e
                Debug.Log("Login �chou�: " + message);
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
    private IEnumerator ValidateLoginAsync(string email, string password, System.Action<string, string, string> onComplete)
    {
        var loginData = new LoginData
        {
            email = email,
            password = password
        };

        string jsonData = JsonUtility.ToJson(loginData);

        // Cr�er la requ�te POST
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Attendre la r�ponse de l'API
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Traiter la r�ponse JSON
            string jsonResponse = request.downloadHandler.text;
            ApiResponse response = JsonUtility.FromJson<ApiResponse>(jsonResponse);

            // Appeler le callback avec le message, le token et l'ID
            onComplete?.Invoke(response.message, response.token, response.id);
        }
        else
        {
            onComplete?.Invoke("Erreur de connexion: " + request.error, null, null);
        }
    }

    /// <summary>
    /// Classe repr�sentant les donn�es de login envoy�es � l'API
    /// </summary>
    [System.Serializable]
    public class LoginData
    {
        public string email;
        public string password;
    }

    /// <summary>
    /// Classe repr�sentant la r�ponse de l'API
    /// </summary>
    [System.Serializable]
    public class ApiResponse
    {
        public string message;
        public string token;
        public string id; 
    }
}
