using System;
using EventBus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegionalManager : MonoBehaviour {
    [SerializeField] private GameObject popup;

    [SerializeField] private TMPro.TMP_Text evaluationText;
    [SerializeField] private TMPro.TMP_Text expectedRevenueText;
    [SerializeField] private TMPro.TMP_Text actualRevenueText;

    [Header("Evaluation Strings")] [SerializeField]
    private String mad;

    [SerializeField] private String okay;
    [SerializeField] private String happy;
    [SerializeField] private String final;

    private bool fired;
    private bool finished;


    private void OnEnable() {
        EventBus<RegionalManagerEvent>.Subscribe(OnRegionalManagerEvent);
    }

    private void OnDisable() {
        EventBus<RegionalManagerEvent>.Unsubscribe(OnRegionalManagerEvent);
    }

    private void OnRegionalManagerEvent(RegionalManagerEvent e) {
        switch (e.action) {
            case 0:
                evaluate(e.expectedRevenue);
                break;
            case 1:
                close();
                break;
            default:
                throw new System.NotImplementedException("INCORRECT ACTION");
        }
    }


    /// <summary>
    /// this function evaluates the revenue of the store
    /// </summary>
    private void evaluate(int expectedRevenue) {
        popup.SetActive(true);

        int revenue = ShopManager.GetRevenueFunc();
        if (revenue < expectedRevenue) {
            // if the revenue is less than the expected revenue, do stuff
            evaluationText.SetText(mad);
            fired = true;
        }
        else if (revenue == expectedRevenue) {
            // the revenue is equal to the expected revenue
            evaluationText.SetText(okay);
            
            if(TurnManager.turnIndex >= TurnManager.GetTurns().Count) {
                finished = true;
            }
        }
        else {
            // the revenue is more than expected
            evaluationText.SetText(happy);
            
            if(TurnManager.turnIndex >= TurnManager.GetTurns().Count) {
                finished = true;
                evaluationText.SetText(final);
            }
        }

        expectedRevenueText.SetText("Expected Revenue: " + expectedRevenue);
        actualRevenueText.SetText("Actual Revenue: " + revenue);
    }

    /// <summary>
    /// function that takes care of closing the panel
    /// </summary>
    private void close() {
        evaluationText.SetText("");
        popup.SetActive(false);
        
        if (fired)
        {
            SceneManager.LoadScene("MainMenu");
        }
        
        if (finished)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}