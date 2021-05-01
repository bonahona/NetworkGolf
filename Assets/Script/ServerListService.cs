using kcp2k;
using Mirror;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ServerListService
{
    public const string ExternalIpUrl = "https://api.ipify.org?format=json";
    public const string ServerListUrl = "https://176.10.152.112:5001/server/";

    public static UnityServer MyServer = new UnityServer { ServerName = "This is a server" };

    public class IpListing
    {
        public string ip { get; set; }
    }

    private static string GetExternalIp()
    {
        var httpClient = new HttpClient();

        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        var result = httpClient.GetAsync(ExternalIpUrl).ConfigureAwait(false).GetAwaiter().GetResult();

        try {
            return JsonConvert.DeserializeObject<IpListing>(result.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult()).ip;
        } catch (Exception e) {
            Debug.LogWarning($"Failed to parse server response: {e.Message}");
            return "";
        }
    }

    public static List<UnityServer> GetServerList()
    {
        var httpClient = new HttpClient();
        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

        try {
            var result = httpClient.GetAsync(ServerListUrl).ConfigureAwait(false).GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<List<UnityServer>>(result.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult());
        }catch(Exception e) {
            Debug.LogWarning($"Failed to parse server listing: {e.Message}");
            return new List<UnityServer>();
        }
    }

    public static void PostServer(NetworkManager manager, UnityServer server = null)
    {
        var httpClient = new HttpClient();

        if (server == null) {
            server = MyServer;
        }

        MyServer.IpAddress = GetExternalIp();
        MyServer.Port = manager.GetComponent<KcpTransport>().Port;
        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        var result = httpClient.PostAsync(ServerListUrl, new StringContent(JsonConvert.SerializeObject(server), Encoding.UTF8, "application/json")).ConfigureAwait(false).GetAwaiter().GetResult();

        try {
            MyServer = JsonConvert.DeserializeObject<UnityServer>(result.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult());
        } catch (Exception e) {
            Debug.LogWarning($"Failed to parse server response: {e.Message}");
        }
    }


    public static void DeleteServer(UnityServer server = null)
    {
        var httpClient = new HttpClient();

        if (server == null) {
            server = MyServer;
        }

        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        var result = httpClient.DeleteAsync($"{ServerListUrl}{server.Id}").ConfigureAwait(false).GetAwaiter().GetResult();
    }
}
