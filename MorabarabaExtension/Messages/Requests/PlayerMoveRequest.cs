using MorabarabaExtension.Messages.Responses;
using Newtonsoft.Json;
using Redfox.Messages;
using Redfox.Users;
using Redfox.Users.UserVariables;
using System;
using System.Collections.Generic;
using System.Text;

namespace MorabarabaExtension.Messages.Requests
{
    public class PlayerMoveRequest : IZoneRequestMessage
    {
        [JsonProperty]
        public string move;

        public PlayerMoveRequest() : base("si#pm")
        {
        }

        public override void Handle(User user)
        {
            Console.WriteLine("Handling si#pm message from extension!");
            if (!user.UserVariables.ContainsKey("morabaraba_session")) return;
            GameSession sess = MorabarabaController.gameSessions[(user.UserVariables["morabaraba_session"] as UserVariable<int>).Value];
            if (sess.MakeMove(user, move))
            {
                //move is possible and the board has been updated
                user.Room?.SendMessage(new PlayerMoveResponse(move, sess.users.IndexOf(user)));
            } else
            {
                Console.WriteLine("Invalid move attempted! " + move);
                //TODO: invalid move
                //notify the user or just ignore?
            }
        }
    }
}
