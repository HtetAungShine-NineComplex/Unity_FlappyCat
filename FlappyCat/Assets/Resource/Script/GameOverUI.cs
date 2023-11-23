using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button retryBtn;
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI highScroeTxt;
    [SerializeField] private TextMeshProUGUI highScoreDigit;

    private void Awake()
    {
        retryBtn.onClick.AddListener(reatartScene);        
    }

    private void Start()
    {
        
        Bird.GetInstance.OnDied += birdOnDied;
        //highScroeTxt.text = HighScoreSystem.GetHighScore().ToString();
        hide();
    }

    private void  birdOnDied(object sender, System.EventArgs e)
    {
        scoreTxt.text = Level.GetInstance.GetPipePassNum().ToString();
        if (Level.GetInstance.GetPipePassNum() >= HighScoreSystem.GetHighScore())
        {
            highScroeTxt.text = "NEW HIGHSCORE";
            highScoreDigit.text = Level.GetInstance.GetPipePassNum().ToString();
        }
        else
        {
            highScroeTxt.text = "HIGHSCORE: ";
            highScoreDigit.text = HighScoreSystem.GetHighScore().ToString();
        }
         
        show();
    }

    private void reatartScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void show()
    {
        gameObject.SetActive(true);
    }

    private void hide()
    {
        gameObject.SetActive(false);
    }
}
