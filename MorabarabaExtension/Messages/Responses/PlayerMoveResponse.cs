using Redfox.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MorabarabaExtension.Messages.Responses
{
    class PlayerMoveResponse : IZoneResponseMessage
    {
        string move;
        public PlayerMoveResponse(string _move) : base("si#pm")
        {
            this.move = _move;
        }
    }
}
