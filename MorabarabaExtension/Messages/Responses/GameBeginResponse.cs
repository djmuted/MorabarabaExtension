using Redfox.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MorabarabaExtension.Messages.Responses
{
    class GameBeginResponse : IZoneResponseMessage
    {
        bool isYourTurn;
        public GameBeginResponse(bool _isYourTurn) : base("si#gb")
        {
            this.isYourTurn = _isYourTurn;
        }
    }
}
