using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Quiz : MonoBehaviour
{
    public InputField answer;
    public GameObject questionObject;
    public GameObject answerObject;
    public GameObject endPanel;
    public Text timer;
    public Text question;
    public Text pointsText;
    public Text endText;
    public Text endPoints;
    public Text endRecord;
    public TextAsset questionsFile;
    public TextAsset answersFile;
    public string[] oneQuestion;
    public string[] oneAnswer;
    public bool[] repititionCheck = new bool[10];
    public string correctAnswer;
    public int points;
    public int numOfQuestion;
    public int newRnd;
    System.Random rnd = new System.Random();

    public void Start() {
        oneQuestion = questionsFile.text.Split('\n');
        oneAnswer = answersFile.text.Split('\n');
        StartCoroutine("NewTimer");
    }

    IEnumerator NewTimer() {
        newRnd = rnd.Next(0,4);
        while (repititionCheck[newRnd])
            newRnd = rnd.Next(0,4);
        repititionCheck[newRnd] = true;
        numOfQuestion++;

        question.text = oneQuestion[newRnd];
        correctAnswer = oneAnswer[newRnd].Remove(oneAnswer[newRnd].Length-1, 1);
        answer.text = string.Empty;
        for (int i = 10; i >= 0; i--) {
            timer.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        End(false);
    }

    public void OnChangeWordValue() {
        if (answer.text.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase)) {
            points += correctAnswer.Length;
            pointsText.text = ("Points:" + points);
            StopCoroutine("NewTimer");
            if (numOfQuestion < 4)
                StartCoroutine("NewTimer");
            else End(true);
        }
    }

    public void End(bool win) {
        answerObject.SetActive(false);
        questionObject.SetActive(false);
        if (win) {
            endText.text = ("You win!\nYou answered all the questions");
            endText.color = new Color(255, 230, 0);
        }
        else endText.text = ("Game Over");
        endPoints.text = ("Points:" + points);
        int record;
        if (PlayerPrefs.HasKey("SavedInt"))
            record = PlayerPrefs.GetInt("SavedInt");
        else record = 0;
        if (points > record)
            endRecord.text = ("New record:" + points);
        else endRecord.text = ("Record:" + record);
        endPanel.SetActive(true);
    }

    public void OnClickMenuButton() {
        SceneManager.LoadScene("Scenes/StubScene");
    }

    public void OnClickResrartButton() {
        SceneManager.LoadScene("Scenes/QuizScene");
    }

}
