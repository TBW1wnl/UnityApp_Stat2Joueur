using UnityEngine;
using UnityEngine.UIElements;

public class StatisticsUIManager : MonoBehaviour
{
    public VisualTreeAsset uxmlAsset;

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

        // Exemple de données
        var data = new string[,]
        {
            { "Nom", "Âge", "Ville" },
            { "John", "30", "Paris" },
            { "Sarah", "25", "Lyon" },
            { "Mike", "35", "Marseille" },
            { "Anna", "28", "Nice" }
        };

        for (int i = 0; i < data.GetLength(0); i++)
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            for (int j = 0; j < data.GetLength(1); j++)
            {
                var cell = new Label(data[i, j]);
                row.Add(cell);
            }
            tableContainer.Add(row);
        }
    }
}
