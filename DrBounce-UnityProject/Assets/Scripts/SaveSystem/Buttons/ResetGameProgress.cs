using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class ResetGameProgress : MonoBehaviour
{
    bool firstClick = false;

    public void OnClickedReset()
    {

        if (!firstClick) { firstClick = true; ResetConfirmTime(3); }
        else
        {
            if (File.Exists(Application.persistentDataPath + "/save.dat"))
            {
                File.Delete(Application.persistentDataPath + "/save.dat");
                SceneManager.LoadScene(0);
            }
        }
    }

    IEnumerator ResetConfirmTime(float time)
    {
        yield return new WaitForSeconds(time);
        firstClick = false;
    }
}
