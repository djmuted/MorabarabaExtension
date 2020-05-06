using Redfox.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MorabarabaExtension.Messages.Responses
{
    class GameEndResponse : IZoneResponseMessage
    {
        bool didYouWin;
        public GameEndResponse(bool _didYouWin) : base("si#ge")
        {
            this.didYouWin = _didYouWin;
        }
    }
}
