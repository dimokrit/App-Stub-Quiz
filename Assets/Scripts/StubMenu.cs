using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StubMenu : MonoBehaviour
{
    public GameObject rules;
    public Text record;

    public void Start() {
        if (PlayerPrefs.HasKey("SavedInt")) {
            int recordValue = PlayerPrefs.GetInt("SavedInt");
            record.text = (recordValue.ToString() + " Points");
        }
    }

    public void OnClickStartButton() {
        SceneManager.LoadScene("Scenes/QuizScene");
    }

    public void OnClickRulesButton() {
        rules.SetActive(!rules.activeSelf);
    }
}