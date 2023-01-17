using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Quiz : MonoBehaviour
{
    public InputField answer;
    public GameObject questionObject;
    public GameObject answerObject;
    public GameObject startPanel;
    public GameObject endPanel;
    public Text timer;
    public Text question;
    public Text pointsText;
    public Text endText;
    public Text endPoints;
    public Text endRecord;
    public Text startTimer;
    public TextAsset questionsFile;
    public TextAsset answersFile;
    public AudioSource audioSource;
    public AudioClip startTimerSound;
    public AudioClip winSound;
    public AudioClip gameOverSound;
    public AudioClip rightAnswerSound;
    public string[] oneQuestion;
    public string[] oneAnswer;
    public bool[] repititionCheck;
    public string correctAnswer;
    public int points;
    public int numOfQuestion;
    public int newRnd;
    public bool firstLaunch = true;
    System.Random rnd = new System.Random();

    public void Start() {
        oneQuestion = questionsFile.text.Split('\n');
        oneAnswer = answersFile.text.Split('\n');
        StartCoroutine("NewTimer");
    }

    IEnumerator NewTimer() {
        if (firstLaunch) {
            audioSource.clip = startTimerSound;
            audioSource.PlayDelayed(0.5f);
            for (int i = 3; i >= 1; i--) {
                startTimer.text = i.ToString();
                yield return new WaitForSeconds(1);
            }
        }
        firstLaunch = false;
        startPanel.SetActive(false);
        answerObject.SetActive(true);
        questionObject.SetActive(true);

        newRnd = rnd.Next(0,100);
        while (repititionCheck[newRnd])
            newRnd = rnd.Next(0,100);
        repititionCheck[newRnd] = true;
        numOfQuestion++;

        question.text = oneQuestion[newRnd];
        correctAnswer = oneAnswer[newRnd].Remove(oneAnswer[newRnd].Length-1, 1);
        for (int i = 10; i >= 0; i--) {
            timer.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        End(false);
    }

    public void End(bool win) {
        answerObject.SetActive(false);
        questionObject.SetActive(false);
        if (win) {
            audioSource.clip = winSound;
            endText.text = ("You win!\nYou answered all the questions");
            endText.color = new Color(255, 230, 0);
        } else {
            audioSource.clip = gameOverSound;
            endText.text = ("Game Over");
        }
        audioSource.Play();
        endPoints.text = ("Points:" + points);
        int record;
        if (PlayerPrefs.HasKey("SavedInt"))
            record = PlayerPrefs.GetInt("SavedInt");
        else record = 0;

        if (points > record) {
            endRecord.text = ("New record:" + points);
            PlayerPrefs.SetInt("SavedInt", points);
            PlayerPrefs.Save();
        } else endRecord.text = ("Record:" + record);
        endPanel.SetActive(true);
    }

    public void OnChangeWordValue() {
        if (answer.text.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase)) {
            points += correctAnswer.Length;
            pointsText.text = ("Points:" + points);
            audioSource.clip = rightAnswerSound;
            audioSource.Play();
            answer.text = string.Empty;
            StopCoroutine("NewTimer");
            if (numOfQuestion < 100)
                StartCoroutine("NewTimer");
            else End(true);
        }
    }

    public void OnClickMenuButton() {
        SceneManager.LoadScene("Scenes/StubScene");
    }

    public void OnClickResrartButton() {
        SceneManager.LoadScene("Scenes/QuizScene");
    }

}
