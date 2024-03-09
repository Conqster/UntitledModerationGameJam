using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : UIManager
{
    #region Variables
    [Header("In Game UI")]
    [SerializeField] GameObject pauseObject;
    [Header("Score")]
    [SerializeField] int score;
    [SerializeField] int scoreSubtractionWhenPaused = 1;
    [SerializeField] float timeUntilScoreReductionStartsWhenPausedSeconds = 60f;
    [SerializeField] TextMeshProUGUI ratingText;
    [SerializeField] Slider freeTimeSlider;

    Coroutine scoreSubtractionWhilePausedRoutine;
    float timer = 10f;

    public int Score
    {
        get
        { return score; }
        private set
        {
            score = value < 0 ? 0 : value;
        }
    }

    [Header("Readonly")]
    // keep track if game is paused
    [SerializeField] bool paused = false;
    #endregion

    protected override void Start()
    {
        base.Start();

        timer = timeUntilScoreReductionStartsWhenPausedSeconds;
        freeTimeSlider.maxValue = timeUntilScoreReductionStartsWhenPausedSeconds;
        freeTimeSlider.value = freeTimeSlider.maxValue;
        UpdateRatingText();
    }

    protected override void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentActiveObject == rootMenu)
                TogglePauseMenu();
            else
                ChangeObject(rootMenu);
        }
        else if(!paused && timer < timeUntilScoreReductionStartsWhenPausedSeconds)
        {
            timer += Time.deltaTime;
            freeTimeSlider.value = Mathf.Clamp(timer, 0, timeUntilScoreReductionStartsWhenPausedSeconds);
        }
    }

    public void UpdateRatingText()
    {
        ratingText.text = $"<color=#FFFFFF>Rating: <color=#7CFF00>{Score}";
    }

    void TogglePauseMenu()
    {
        paused = !paused;

        pauseObject.SetActive(paused);

        if(paused )
        {
            if(scoreSubtractionWhilePausedRoutine != null)
                StopCoroutine(scoreSubtractionWhilePausedRoutine);

            scoreSubtractionWhilePausedRoutine = StartCoroutine(ScoreSubtractionWhilePaused());
        }
    }

    IEnumerator ScoreSubtractionWhilePaused()
    {
        //timer = timeUntilScoreReductionStartsWhenPausedSeconds;
        while (paused)
        {
            if(timer > 0)
            {
                timer -= Time.unscaledDeltaTime;
                freeTimeSlider.value = Mathf.Clamp(timer, 0, timeUntilScoreReductionStartsWhenPausedSeconds);
                yield return new WaitForEndOfFrame();
                continue;
            }

            Score -= scoreSubtractionWhenPaused;
            UpdateRatingText();
            yield return new WaitForSecondsRealtime(1f);

            
        }
    }
}
