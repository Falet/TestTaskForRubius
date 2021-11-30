using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitCards : MonoBehaviour
{
    [SerializeField] private Dropdown dropdown;
    [SerializeField] private Button loadBtn;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Transform horizontalLayoutGroup;
    [SerializeField] private DownloadHandler downloadHandler;
    [SerializeField] private Card prefabCard;
    [SerializeField] private int countCard;
    [SerializeField] private string url;

    private List<Card> _cards;

    private void Awake()
    {
        _cards = new List<Card>(countCard);
    }

    private void Start()
    {
        loadBtn.onClick.AddListener(Load);
        cancelBtn.onClick.AddListener(() => downloadHandler.CancelLoad());
        for (int i = 0; i < countCard; i++)
        {
            Card newCard = Instantiate(prefabCard,horizontalLayoutGroup);
            _cards.Add(newCard);
        }
        
        downloadHandler.Init(_cards);
    }

    private async void Load()
    {
        loadBtn.interactable = false;
        await downloadHandler.Load(url, dropdown.value);
        loadBtn.interactable = true;
    }
}
