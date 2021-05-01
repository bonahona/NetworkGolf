using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkManager))]
public class NetworkServerListing : MonoBehaviour
{
    public int offsetX;

    public int offsetY;

    public float UpdateTimeTimer = 15;

    private List<UnityServer> _foundServers = new List<UnityServer>();
    private NetworkManager manager;

    private float timeToUpdate;
    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<NetworkManager>();
        _foundServers = ServerListService.GetServerList();
        timeToUpdate = UpdateTimeTimer;
    }

    // Update is called once per frame
    void Update()
    {
        timeToUpdate -= Time.deltaTime;
        if(timeToUpdate <= 0) {
            _foundServers = ServerListService.GetServerList();
            timeToUpdate = UpdateTimeTimer;
        }
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

        if (GUILayout.Button("Refresh")) {
            _foundServers = ServerListService.GetServerList();
        }

        GUILayout.EndArea();
    }

    private void ConnectedToServer(UnityServer server)
    {
        manager.networkAddress = server.IpAddress;
        manager.StartClient();
    }
}
