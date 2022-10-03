using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieEffect : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke(nameof(AutoDeactivate), 2f);
    }

    void AutoDeactivate()
    {
        gameObject.SetActive(false);
    }
}
