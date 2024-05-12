using EventBus;
using UnityEngine;

public class RegionalManagerPanel : MonoBehaviour {
    public void ClosePanel() {
        EventBus<RegionalManagerEvent>.Raise(new RegionalManagerEvent(1, -1));
    }
}