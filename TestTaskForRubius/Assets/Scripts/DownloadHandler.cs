using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadHandler : MonoBehaviour
{
    private List<Card> _cards;
    private CancellationTokenSource source;
    private List<Task> _tasks;
    
    public void Init(List<Card> cards)
    {
        _cards = cards;
        _tasks = new List<Task>(_cards.Count);
    }
    
    public async Task Load(string url, int typeDownload)
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            _tasks.Add(_cards[i].HideContent());
        }
        await WaitAllAndClear(_tasks);
        
        source = new CancellationTokenSource();
        switch (typeDownload)
        {
            case 0:
            {
                await LoadAllAtOnce(url);
                break;
            }
            case 1:
            {
                await LoadOneByOne(url);
                break;
            }
            case 2:
            {
                await LoadWhenReady(url);
                break;
            }
            default:
            {
                await LoadAllAtOnce(url);
                break;
            }
        }
    }

    public void CancelLoad()
    {
        source.Cancel();
    }

    private async Task LoadAllAtOnce(string url)
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            _tasks.Add(GetTexture(url, _cards[i])); 
        }
        await WaitAllAndClear(_tasks);
            
        for (int i = 0; i < _cards.Count; i++)
        {
            _tasks.Add(_cards[i].ShowContent());
        }
        await WaitAllAndClear(_tasks);
    }
    
    private async Task LoadOneByOne(string url)
    {
        for (int i = _cards.Count - 1; i >= 0 ; i--)
        {
            await GetTexture(url, _cards[i]); 
            
            _tasks.Add(_cards[i].ShowContent());
        }
        await WaitAllAndClear(_tasks);
    }

    private async Task LoadWhenReady(string url)
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            _tasks.Add(LoadWhenReadyOne(url,_cards[i])); 
        }

        await WaitAllAndClear(_tasks);
    }

    private async Task LoadWhenReadyOne(string url,Card card)
    {
        await GetTexture(url, card); 
        
        await card.ShowContent();
    }
    
    private async Task GetTexture(string url,Card card)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        await request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            card.SetTexture(DownloadHandlerTexture.GetContent(request));
        }
    }

    private async Task WaitAllAndClear(List<Task> tasks)
    {
        await Task.WhenAll(tasks);
                
        _tasks.Clear();
    }
}
