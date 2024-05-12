using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsHandler : MonoBehaviour {
    public GameObject menuPanel;
    public GameObject settingsPanel;

    public void close() {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}