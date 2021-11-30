using System;
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
                await LoadAllAtOnce(url,source.Token);
                break;
            }
            case 1:
            {
                await LoadOneByOne(url, source.Token);
                break;
            }
            case 2:
            {
                await LoadWhenReady(url, source.Token);
                break;
            }
            default:
            {
                await LoadAllAtOnce(url,source.Token);
                break;
            }
        }
    }

    public void CancelLoad()
    {
        source.Cancel();
    }

    private async Task LoadAllAtOnce(string url, CancellationToken token)
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            _tasks.Add(GetTexture(url, _cards[i], token)); 
        }
        await WaitAllAndClear(_tasks);
            
        for (int i = 0; i < _cards.Count; i++)
        {
            _tasks.Add(_cards[i].ShowContent());
        }
        await WaitAllAndClear(_tasks);
    }
    
    private async Task LoadOneByOne(string url, CancellationToken token)
    {
        for (int i = _cards.Count - 1; i >= 0 ; i--)
        {
            await GetTexture(url, _cards[i], token); 
            
            _tasks.Add(_cards[i].ShowContent());
        }
        await WaitAllAndClear(_tasks);
    }

    private async Task LoadWhenReady(string url, CancellationToken token)
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            _tasks.Add(LoadWhenReadyOne(url,_cards[i],token)); 
        }

        await WaitAllAndClear(_tasks);
    }

    private async Task LoadWhenReadyOne(string url,Card card, CancellationToken token)
    {
        await GetTexture(url, card, token); 
        
        await card.ShowContent();
    }
    
    private async Task GetTexture(string url,Card card, CancellationToken token)
    {
        card.SetTexture(null);
        Resources.UnloadUnusedAssets();
        
        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        try
        {
            await request.SendWebRequest().WithCancellation(token);
        }
        catch (Exception e)
        {
            if (e is OperationCanceledException)
            {
                //TODO download cancel
            }
        }
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
