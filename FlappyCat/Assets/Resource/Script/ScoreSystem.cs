using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTxt;

    private void Update()
    {
        scoreTxt.text = Level.GetInstance.GetPipePassNum().ToString();
    }
}
