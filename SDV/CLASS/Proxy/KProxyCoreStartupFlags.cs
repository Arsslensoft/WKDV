namespace BrowserWeb
{
    using System;

    [Flags]
    public enum SDVPStartupFlags
    {
        AllowRemoteClients = 8,
        CaptureLocalhostTraffic = 0x80,
        ChainToUpstreamGateway = 0x10,
        DecryptSSL = 2,
        Default = 0xbb,
        MonitorAllConnections = 0x20,
        None = 0,
        RegisterAsSystemProxy = 1
    }
}

