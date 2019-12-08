using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageGateController : GateController
{
    [SerializeField] private bool progressContractLevel;

    public override void UseGate()
    {
        int sceneNum = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(sceneNum);

        if (progressContractLevel)
        {
            Contract.instance.ProgressContract();
            Contract.instance.InitializeKillCount();
        }
    }
}
