using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Paused,
    Running,
    Win,
    Lose
}

public class GameController : MonoBehaviour
{
    public GameState GameState;
    public ToFixList ToFixList;
    public TextMeshProUGUI EndText;
    public GameObject EndPanel;
    public TextMeshProUGUI TimeText;
    private Image TimePanelImage;
    public int StartTime;
    private float RemainingTime;
    public int CurrentLevel;
    public int FinalLevel;
    public Button NextLevelButton;

    public void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene($"Level{CurrentLevel + 1}");
    }

    public void GoToMainMenu() 
    {
        SceneManager.LoadScene($"MainMenu");
    }

    private void Awake()
    {
        // HACK Super ugly hack, but I didn't have time to properly fix this;
        if(Machine.numOfInstances != 0)
        {
            Machine.numOfInstances = 0;
        }

    }

    private void Start()
    {
        ToFixList.MachineWasFixed += CheckIfAllMachinesFixed;
        RemainingTime = StartTime;
        GameState = GameState.Running;
        TimeText.fontSize = 14;
        TimePanelImage = TimeText.gameObject.GetComponentInParent<Image>();
        TimePanelImage.color = new Color32(255, 255, 255, 255); // white
        NextLevelButton.interactable = true;
    }

    private void Update()
    {
        if(GameState != GameState.Running)
        {
            return;
        }

        RemainingTime -= Time.deltaTime;
        var percentOfTimePassed = RemainingTime / StartTime;
        if(RemainingTime < 0)
        {
            TimeText.text = $"0";
            GameState = GameState.Lose;
            EndText.text = "Too late! \nThe station is destroyed!";
            EndPanel.SetActive(true);
            EndText.gameObject.SetActive(true);
            NextLevelButton.interactable = false;
            return;
        }
        else if(percentOfTimePassed  >= 0.333 && percentOfTimePassed < 0.666)
        {
            TimeText.fontSize = 16;
            TimePanelImage.color = new Color32(255, 165,0,255); // orange
        }
        else if (percentOfTimePassed >= 0 && percentOfTimePassed < 0.333)
        {
            TimeText.fontSize = 18;
            TimePanelImage.color = new Color32(255, 0, 0, 255); // red
        }
        TimeText.text = $"{(int)RemainingTime}";
    }

    private void CheckIfAllMachinesFixed()
    {
        if (ToFixList.machines.Any(x => !x.IsFixed))
        {
            // if at least 1 is broken, then obviously not all machines are fixed
            return;
        }
        // Level won
        if (SceneManager.GetActiveScene().name == $"Level{FinalLevel}")
        {
            // End game
            SceneManager.LoadScene("Credits");
        }
        GameState = GameState.Win;
        EndPanel.SetActive(true);
        EndText.gameObject.SetActive(true);
    }


}
