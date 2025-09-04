using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private GameManager _gameManager;
    void Start()
    {
        _startButton.onClick.AddListener(() =>
        {
            _gameManager.InitGame();
        });
        Show();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        SoundManager.Instance.PlayBGM(BGMSoundData.BGM.title);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
