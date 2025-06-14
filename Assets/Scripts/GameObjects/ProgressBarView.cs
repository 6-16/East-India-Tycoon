using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ProgressBarView : MonoBehaviour
{
    [SerializeField] private List<Image> _fillBars = new();
    private Camera _mainCamera;



    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.forward = _mainCamera.transform.forward;
    }

    public void SetProgress(int index, float progress)
    {
        if (index < 0 || index >= _fillBars.Count) return;
        _fillBars[index].fillAmount = Mathf.Clamp01(progress);
    }

    public void Show(int index)
    {
        if (index < 0 || index >= _fillBars.Count) return;
        _fillBars[index].transform.parent.gameObject.SetActive(true);
        SetProgress(index, 0);
    }

    public void Hide(int index)
    {
        if (index < 0 || index >= _fillBars.Count) return;
        _fillBars[index].transform.parent.gameObject.SetActive(false);
    }
}

