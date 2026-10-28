
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject quizPanel;

    [Header("Question UI")]
    public TMP_Text questionText;
    public Button[] answerButtons;
    public TMP_Text feedbackText;
    public Button nextButton;

    [Header("Result UI")]
    public TMP_Text scoreText;
    public Button restartQuizButton;

    private string[] questions =
    {
        "Which principle does a Stack follow?",
        "Which operation adds an element to a Stack?",
        "What happens when POP is performed on an empty Stack?"
    };

    private string[,] options =
    {
        { "FIFO", "LIFO", "Random order", "Sorted order" },
        { "POP", "PEEK", "PUSH", "DELETE" },
        { "Overflow", "Underflow", "Element added", "Stack sorted" }
    };

    // Zero-based indexes of correct answers
    private int[] correctAnswers = { 1, 2, 1 };

    private string[] explanations =
    {
        "LIFO means Last In, First Out.",
        "PUSH adds an element to the top of a Stack.",
        "Underflow occurs when POP is used on an empty Stack."
    };

    private int currentQuestion = 0;
    private int score = 0;
    private bool answered = false;

    void Start()
    {
        quizPanel.SetActive(false);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answerIndex = i;
            answerButtons[i].onClick.AddListener(
                () => CheckAnswer(answerIndex)
            );
        }

        nextButton.onClick.AddListener(NextQuestion);
        restartQuizButton.onClick.AddListener(StartQuiz);
    }

    public void StartQuiz()
    {
        currentQuestion = 0;
        score = 0;
        quizPanel.SetActive(true);
        ShowQuestion();
    }

    void ShowQuestion()
    {
        answered = false;

        questionText.gameObject.SetActive(true);
        questionText.text =
            "Question " + (currentQuestion + 1) +
            "/3\n\n" + questions[currentQuestion];

        feedbackText.text = "";
        nextButton.gameObject.SetActive(false);
        restartQuizButton.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(true);
            answerButtons[i].interactable = true;

            TMP_Text buttonText =
                answerButtons[i].GetComponentInChildren<TMP_Text>();

            buttonText.text = options[currentQuestion, i];
        }
    }

    public void CheckAnswer(int selectedAnswer)
    {
        if (answered) return;

        answered = true;

        int correct = correctAnswers[currentQuestion];

        if (selectedAnswer == correct)
        {
            score++;
            feedbackText.text =
                "Correct!\n" + explanations[currentQuestion];
        }
        else
        {
            feedbackText.text =
                "Wrong!\nCorrect answer: " +
                options[currentQuestion, correct] +
                "\n" + explanations[currentQuestion];
        }

        foreach (Button button in answerButtons)
        {
            button.interactable = false;
        }

        nextButton.gameObject.SetActive(true);

        TMP_Text nextText =
            nextButton.GetComponentInChildren<TMP_Text>();

        nextText.text =
            currentQuestion == questions.Length - 1
            ? "Show Marks"
            : "Next";
    }

    public void NextQuestion()
    {
        currentQuestion++;

        if (currentQuestion >= questions.Length)
        {
            ShowResults();
        }
        else
        {
            ShowQuestion();
        }
    }

    void ShowResults()
    {
        questionText.gameObject.SetActive(false);
        feedbackText.text = "";
        nextButton.gameObject.SetActive(false);

        foreach (Button button in answerButtons)
        {
            button.gameObject.SetActive(false);
        }

        scoreText.gameObject.SetActive(true);
        scoreText.text =
            "Quiz Completed!\n\nYour Marks: " +
            score + " / " + questions.Length;

        restartQuizButton.gameObject.SetActive(true);
    }

    public void CloseQuiz()
    {
        quizPanel.SetActive(false);
    }
}
