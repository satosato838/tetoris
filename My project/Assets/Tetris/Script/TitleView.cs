using UnityEngine;
using UnityEngine.UI;

public class TitleView : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private GameView _gameView;
    void Start()
    {
        _startButton.onClick.AddListener(() =>
        {
            _gameView.InitGame();
        });
        _exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
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
