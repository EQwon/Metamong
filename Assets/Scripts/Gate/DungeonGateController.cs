using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGateController : GateController
{
    [SerializeField] private GameObject loadingCanvas;

    public override void UseGate()
    {
        Instantiate(loadingCanvas);

        int sceneNum = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(sceneNum);
    }
}
