﻿using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using SDG.Unturned;
using Rocket.Unturned.Chat;
using UnityEngine;
using System;
using Logger = Rocket.Core.Logging.Logger;

namespace SimpleJump
{
    class CommandDescend : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "descend";

        public string Help => "Teleports you X meters below you.";

        public string Syntax => "[Distance]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>() { "descend" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            try
            {
                UnturnedPlayer uPlayer = (UnturnedPlayer)caller;
                float playerDirection = uPlayer.Player.look.angle;

                if (command[0].Length == 0)
                {
                    UnturnedChat.Say(caller, "Invalid use: /descend [Distance]", Color.red);
                    return;
                }
                ushort Distance = ushort.Parse(command[0]);
                if (Distance <= 0)
                {
                    UnturnedChat.Say(caller, "The Distance must be positive!", Color.red);
                    return;
                }

                if (Distance > SimpleJump.Instance.Configuration.Instance.MaxAscendDistance)
                {
                    UnturnedChat.Say(caller, $"The maximum Descend distance is {SimpleJump.Instance.Configuration.Instance.MaxDescendDistance} meters!", Color.red);
                    return;
                }   

                Vector3 teleportLocation = new Vector3(uPlayer.Position.x, uPlayer.Position.y - Distance, uPlayer.Position.z);
                uPlayer.Teleport(teleportLocation, uPlayer.Player.look.angle);
                UnturnedChat.Say(caller, $"Successfully descended {Distance} meters.", Color.yellow);
            }
            catch (Exception ex)
            {
                UnturnedChat.Say(caller, "Invalid use: /descend [Distance]", Color.red);
                if (SimpleJump.Instance.Configuration.Instance.DebugMode)
                {
                    Logger.LogWarning($"{ex}");
                }
            }

        }

    }
}
