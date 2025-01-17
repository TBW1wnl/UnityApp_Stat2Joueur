using UnityEngine;
using MySql.Data.MySqlClient;

public class DBManger : MonoBehaviour
{
    private string Server { get; } = "localhost";
    private string Database { get; } = "UserStatsProject";
    private string User { get; } = "UnityUser";
    private string Password { get; } = "Unity123!";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string connectionString = $"Server={Server};Database={Database};User ID={User};Password={Password};Pooling=true;";

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                Console.WriteLine("Connexion réussie à la base de données !");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
