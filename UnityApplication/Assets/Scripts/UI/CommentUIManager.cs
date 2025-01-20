using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections;

public class CommentUIManager : MonoBehaviour
{
    private Button validateButton;
    private Button statisticsButton;
    private TextField inputField;

    private string apiUrl = "http://localhost/api/comments";

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        validateButton = root.Q<Button>("send");
        statisticsButton = root.Q<Button>("statistics");
        inputField = root.Q<TextField>("input");

        validateButton.clicked += OnValidateButtonClicked;
        statisticsButton.clicked += StatisticsButtonClicked;
    }

    private void OnValidateButtonClicked()
    {
        string comment = inputField.value;
        if (!string.IsNullOrEmpty(comment))
        {
            Debug.Log("Commentaire envoyé !");
            StartCoroutine(SendComment(comment));
        }
        else
        {
            Debug.Log("Le commentaire est vide !");
        }
    }

    private IEnumerator SendComment(string comment)
    {
        string userId = GameManager.Instance.userId;
        string jsonData = "{\"content\": \"" + comment + "\"}";

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Ajouter l'ID utilisateur dans les en-têtes
        request.SetRequestHeader("UserId", userId);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Réponse de l'API : " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Erreur lors de l'envoi du commentaire : " + request.error);
        }
    }




    private void StatisticsButtonClicked()
    {
        Debug.Log("Chargement de la scène des statistiques...");
        GameManager.Instance.LoadScene("StatisticsScene");
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
