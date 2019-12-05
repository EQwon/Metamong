using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGateController : GateController
{
    [SerializeField] private int sceneNum;
    [SerializeField] private GameObject loadingCanvas;

    protected override void OnDrawGizmos()
    {
        
    }

    public override void UseGate()
    {
        Instantiate(loadingCanvas);
        SceneManager.LoadScene(sceneNum);
        Contract.instance.UseChapterContract(true);
    }
}
