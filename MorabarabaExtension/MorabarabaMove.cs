using System;
using System.Collections.Generic;
using System.Text;

namespace MorabarabaExtension
{
    enum MoveType
    {
        PLACE,
        PLACESHOOT,
        MOVE,
        MOVESHOOT
    }
    class MorabarabaMove
    {
        public MoveType moveType;
        public MorabarabaField source;
        public MorabarabaField destination;
        public MorabarabaField target;
    }
}
