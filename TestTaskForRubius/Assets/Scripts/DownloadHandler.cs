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
        
    }

    private async Task LoadAllAtOnce(string url)
    {
        
    }
    
    private async Task LoadOneByOne(string url)
    {
        
    }

    private async Task LoadWhenReady(string url)
    {
        
    }

    private async Task LoadWhenReadyOne(string url,Card card)
    {
       
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
}
