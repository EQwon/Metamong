using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageGateController : GateController
{
    public override void UseGate()
    {
        int sceneNum = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(sceneNum);
        Contract.instance.UseChapterContract(false);
    }
}
