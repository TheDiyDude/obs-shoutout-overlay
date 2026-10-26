using System;
using Newtonsoft.Json;

// Shoutout-Overlay (OBS-Szene _Shoutout, C:\Streaming\shoutout\shoutout.html): schickt den Ziel-Kanal
// über den WebSocket-Server an das Overlay, das sich Profil, Spiel und Clip selbst von Twitch holt.
// Aufgerufen aus [Auto-Shoutout] Shoutout und [Auto-Shoutout] Auto-Shoutout nach "Get User Info for Target".
public class CPHInline
{
    public bool Execute()
    {
        string login = Get("targetUserName");
        if (login == "") login = Get("input0").TrimStart('@');
        if (login == "") login = Get("userName");
        if (login == "")
        {
            CPH.LogWarn("[Shoutout] Overlay: kein Kanal in den Argumenten");
            return false;
        }

        var payload = new
        {
            shoutout = new
            {
                login = login.ToLowerInvariant(),
                displayName = Get("targetUser"),
                game = Get("game"),
                profileImage = Get("targetUserProfileImageUrl")
            }
        };
        CPH.WebsocketBroadcastJson(JsonConvert.SerializeObject(payload));
        return true;
    }

    private string Get(string name)
    {
        object v;
        return args.TryGetValue(name, out v) && v != null ? v.ToString().Trim() : "";
    }
}
