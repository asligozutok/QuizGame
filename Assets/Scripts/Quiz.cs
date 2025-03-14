using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    //normal TextMeshPro'dan farkı UI'da gözükecek olması oyunun içinde değil
    [SerializeField] List<QuestionSO> questions; // = new List<QuestionSO>(); da yazabilirsin ser.field. kaldırırsan örnek vererek koruma sağlamak için
    //ayrıca array [] yerine liste yapıyoruz çünkü soru çıktıktan sonra onu listeden çıkartmak istiyoruz
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;

    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;

    public bool isComplete;

    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;

    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if(timer.loadNextQuestion)
        {
            
            if(progressBar.value == progressBar.maxValue) //check if game is complete
            {
                isComplete = true;
                return; //get out of the update loop if this if statement is true to avoid trying to load another question that does not exist
            }

            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if(!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int index)
    {   
        hasAnsweredEarly = true;
        DisplayAnswer(index);

        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Skor:" + scoreKeeper.CalculateScore() + "%";
    }

    void DisplayAnswer(int index)
    {
        if(index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "Omaygat doğru cevap!";
            Image buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            TextMeshProUGUI correctAnswerText=answerButtons[currentQuestion.GetCorrectAnswerIndex()].GetComponentInChildren<TextMeshProUGUI>();
            questionText.text = string.Format(">:( Doğru cevap {0}!", correctAnswerText.text ) ;
            
            Image correctbuttonImage = answerButtons[currentQuestion.GetCorrectAnswerIndex()].GetComponent<Image>();
            //tekrar temporary image yaratmak yerine methodın başına bir tane yapabilirsin if ve else'e ayrı ayrı oluşturmak yerine
            correctbuttonImage.sprite = correctAnswerSprite;
            //başka şekilde de yapablirsin 58. vidyoyu izle
        }
    }

    void GetNextQuestion()
    {   if (questions.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            scoreKeeper.IncrementQuestionsSeen();
        }
    }

    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];

        if (questions.Contains(currentQuestion)) //genelde büyük projelerde kontrol için yapılır
        {
        questions.Remove(currentQuestion);
        }

    }

    void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();

        for(int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            //looks through all children of the answer button and returns the first textmeshpro componenent
            buttonText.text = currentQuestion.GetAnswer(i);
        }
    }

    void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void SetDefaultButtonSprites()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }
}
