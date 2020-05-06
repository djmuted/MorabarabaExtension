using Redfox.Configs;
using Redfox.Messages;
using Redfox.Rooms;
using Redfox.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace MorabarabaExtension.Messages.Requests
{
    public class GameSessionRequest : IZoneRequestMessage
    {
        public GameSessionRequest() : base("si#gs")
        {
        }

        public override void Handle(User user)
        {
            //user.Room?.SendMessage(new PlayerMoveResponse(move));
            if(user.UserVariables.ContainsKey("morabaraba_session"))
            {
                //user already in game
            } else
            {
                MorabarabaController.EnqueueUser(user);
            }
        }
    }
}
