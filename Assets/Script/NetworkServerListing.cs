using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkManager))]
public class NetworkServerListing : MonoBehaviour
{
    public int offsetX;

    public int offsetY;

    private List<UnityServer> _foundServers = new List<UnityServer>();
    private NetworkManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<NetworkManager>();
        _foundServers = ServerListService.GetServerList();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215, 9999));
        GUILayout.Label("Server list");

        foreach(var server in _foundServers) {
            if (GUILayout.Button(server.ServerName)) {
                ConnectedToServer(server);
            }
        }
        GUILayout.EndArea();
    }

    private void ConnectedToServer(UnityServer server)
    {
        manager.networkAddress = server.IpAddress;
        manager.StartClient();
    }
}
