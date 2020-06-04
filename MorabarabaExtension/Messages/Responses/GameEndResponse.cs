using Redfox.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MorabarabaExtension.Messages.Responses
{
    class GameEndResponse : IZoneResponseMessage
    {
        public bool didYouWin;
        public GameEndResponse(bool _didYouWin) : base("si#ge")
        {
            this.didYouWin = _didYouWin;
        }
    }
}
