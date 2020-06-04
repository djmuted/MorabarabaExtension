using Redfox.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MorabarabaExtension.Messages.Responses
{
    class PlayerMoveResponse : IZoneResponseMessage
    {
        public int playerId;
        public string move;
        public PlayerMoveResponse(string _move, int _playerId) : base("si#pm")
        {
            this.playerId = _playerId;
            this.move = _move;
        }
    }
}
