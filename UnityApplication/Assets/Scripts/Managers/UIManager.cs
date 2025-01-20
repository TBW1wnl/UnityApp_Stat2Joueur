using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void OnLoginButtonPressed()
    {
        GameManager.Instance.LoadScene("StatisticsScene");
    }

    public void OnCommentsButtonPressed()
    {
        GameManager.Instance.LoadScene("CommentsScene");
    }

    public void OnBackToLoginButtonPressed()
    {
        GameManager.Instance.LoadScene("LoginScene");
    }
}
