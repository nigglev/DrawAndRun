using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private StringBuilder _sb;
    private GameObject _start_level_panel;
    private GameObject _finish_level_panel;
    private GameObject _failed_level_panel;
    private TextMeshProUGUI _points_text;

    private void Awake()
    {
        _sb = new StringBuilder("Current Points");

        CGameManager.Instance.SetUIManager(this);
        CGameManager.Instance.OnLevelStart += HideStartLevelPanel;
        CGameManager.Instance.OnLevelFinish += ShowFinishLevelPanel;
        CGameManager.Instance.OnUnitsDead += ShowFailedLevelPanel;

        _start_level_panel = gameObject.transform.Find("StartLevelPanel").gameObject;
        _finish_level_panel = gameObject.transform.Find("FinishLevelPanel").gameObject;
        _failed_level_panel = gameObject.transform.Find("FailedLevelPanel").gameObject;
        _points_text = gameObject.transform.Find("PointsText").gameObject.GetComponent<TextMeshProUGUI>();

        _points_text.text = _sb.ToString();
        _finish_level_panel.SetActive(false);
        _failed_level_panel.SetActive(false);
    }

    private void HideStartLevelPanel()
    {
        _start_level_panel.SetActive(false);
    }

    private void ShowFinishLevelPanel()
    {
        _finish_level_panel.SetActive(true);
    }

    private void ShowFailedLevelPanel()
    {
        _failed_level_panel.SetActive(true);
    }

    public void AddPoints(int in_points)
    {
        _points_text.text = _sb.Append(in_points.ToString()).ToString();
    }

}
