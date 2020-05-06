using System;
using System.Collections.Generic;
using System.Text;

namespace MorabarabaExtension
{
    enum MorabarabaPhase
    {
        PLACING,
        MOVING,
        FLYING
    }
    class MorabarabaPlayerContext
    {
        public int playerid;
        public MorabarabaPhase phase;
        public short mills;
        public short placingCowsLeft = 12;
        public short cows = 0;

        public MorabarabaPlayerContext(int _playerid)
        {
            this.playerid = _playerid;
            this.mills = 0;
            this.phase = MorabarabaPhase.PLACING;
        }
    }
}
