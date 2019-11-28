using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageGateController : GateController
{
    [SerializeField] private int sceneNum;

    public override void UseGate()
    {
        SceneManager.LoadScene(sceneNum);
        Contract.instance.UseChapterContract(false);
    }
}
