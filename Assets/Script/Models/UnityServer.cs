using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityServer
{
    public int Id { get; set; }
    public string ServerName { get; set; }
    public string IpAddress { get; set; }
    public int Port { get; set; }

    public List<string> Users { get; set; } = new List<string>();
}
