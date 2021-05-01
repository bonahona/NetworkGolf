using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DiscoveryServerUpdates : MonoBehaviour
{
    public const int UpdateInterval = 15;

    // Start is called before the first frame update
    void Start()
    {
        Task.Run(async () => {
            while (true) {

                await ServerListService.SilentUpdate();
                await Task.Delay(UpdateInterval * 1000);
            }
        });
    }
}
