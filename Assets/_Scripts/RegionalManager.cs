using EventBus;
using UnityEngine;

public class RegionalManager : MonoBehaviour {
    [SerializeField] private GameObject popup;


    private void OnEnable() {
        EventBus<RegionalManagerEvent>.Subscribe(OnRegionalManagerEvent);
    }

    private void OnDisable() {
        EventBus<RegionalManagerEvent>.Unsubscribe(OnRegionalManagerEvent);
    }

    private void OnRegionalManagerEvent(RegionalManagerEvent e) {
        switch (e.action) {
            case 0:
                popup.SetActive(true);
                break;
            case 1:
                popup.SetActive(false);
                break;
            default:
                throw new System.NotImplementedException("INCORRECT ACTION");
        }
    }
}