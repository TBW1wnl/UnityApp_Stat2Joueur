using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class StatisticsUIManager : MonoBehaviour
{
    public VisualTreeAsset uxmlAsset;
    private string apiUrlBase = "http://localhost/api/users/";
    private Button commentButton;

    void OnEnable()
    {
        if (uxmlAsset == null)
        {
            Debug.LogError("Le fichier UXML n'est pas assigné à uxmlAsset !");
            return;
        }

        var document = GetComponent<UIDocument>();
        if (document == null)
        {
            Debug.LogError("Le composant UIDocument n'est pas attaché !");
            return;
        }

        var root = document.rootVisualElement;
        if (root == null)
        {
            Debug.LogError("rootVisualElement est null !");
            return;
        }

        var uxmlInstance = uxmlAsset.CloneTree();
        root.Add(uxmlInstance);

        var tableContainer = root.Q<VisualElement>("tableContainer");
        if (tableContainer == null)
        {
            Debug.LogError("tableContainer n'a pas été trouvé dans le UXML !");
            return;
        }

        AddTableHeader(tableContainer);

        // Vérifiez que GameManager et l'ID utilisateur sont disponibles
        if (GameManager.Instance != null)
        {
            string apiUrl = $"{apiUrlBase}{GameManager.Instance.userId}/scores";
            Debug.Log($"API URL construite : {apiUrl}");
            StartCoroutine(FetchDataFromAPI(apiUrl, tableContainer));
        }
        else
        {
            Debug.LogError("GameManager ou userId est null ou invalide !");
        }

        commentButton = root.Q<Button>("commentary");

        commentButton.clicked += CommentaryButtonClicked;
    }

    private void CommentaryButtonClicked()
    {
        Debug.Log("Commentary button clicked");
        GameManager.Instance.LoadScene("CommentsScene");
    }

    IEnumerator FetchDataFromAPI(string apiUrl, VisualElement tableContainer)
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erreur lors de l'appel API: " + request.error);
            yield break;
        }

        string jsonResponse = request.downloadHandler.text;

        try
        {
            GameData[] gamesData = JsonUtility.FromJson<GameDataWrapper>($"{{\"games\": {jsonResponse}}}").games;
            foreach (var gameData in gamesData)
            {
                AddDataToTable(tableContainer, gameData);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Erreur lors du traitement des données JSON : " + e.Message);
        }
    }

    void AddDataToTable(VisualElement tableContainer, GameData gameData)
    {
        var row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.style.alignItems = Align.Center;
        row.style.justifyContent = Justify.Center; 

        var cellStartDate = CreateTableCell(gameData.startDate);
        var cellUserTeamScore = CreateTableCell(gameData.user_team_score.ToString());
        var cellEnemyTeamScore = CreateTableCell(gameData.enemy_team_score.ToString());
        var cellKills = CreateTableCell(gameData.kills.ToString());
        var cellDeaths = CreateTableCell(gameData.deaths.ToString());
        var cellAssists = CreateTableCell(gameData.assists.ToString());

        row.Add(cellStartDate);
        row.Add(cellUserTeamScore);
        row.Add(cellEnemyTeamScore);
        row.Add(cellKills);
        row.Add(cellDeaths);
        row.Add(cellAssists);

        tableContainer.Add(row);
    }

    void AddTableHeader(VisualElement tableContainer)
    {
        var headerRow = new VisualElement();
        headerRow.style.flexDirection = FlexDirection.Row;
        headerRow.style.alignItems = Align.Center;
        headerRow.style.justifyContent = Justify.Center;

        // Créer les cellules d'en-tête
        var headerStartDate = CreateTableCell("Begin");
        var headerUserTeamScore = CreateTableCell("User Team Score");
        var headerEnemyTeamScore = CreateTableCell("Enemy Team Score");
        var headerKills = CreateTableCell("Kills");
        var headerDeaths = CreateTableCell("Deaths");
        var headerAssists = CreateTableCell("Assists");

        headerRow.Add(headerStartDate);
        headerRow.Add(headerUserTeamScore);
        headerRow.Add(headerEnemyTeamScore);
        headerRow.Add(headerKills);
        headerRow.Add(headerDeaths);
        headerRow.Add(headerAssists);

        tableContainer.Add(headerRow);
    }

    VisualElement CreateTableCell(string text)
    {
        var cell = new Label(text);
        cell.style.flexGrow = 1;
        cell.style.marginLeft = 5;
        cell.style.marginRight = 5;
        cell.style.paddingTop = 5;
        cell.style.paddingBottom = 5;
        return cell;
    }


    [System.Serializable]
    public class GameData
    {
        public int game_id;
        public string startDate;
        public int user_team_score;
        public int enemy_team_score;
        public int kills;
        public int deaths;
        public int assists;
    }

    [System.Serializable]
    public class GameDataWrapper
    {
        public GameData[] games;
    }
}
