using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 60;
        HighScoreSystem.Start();
    }
}
