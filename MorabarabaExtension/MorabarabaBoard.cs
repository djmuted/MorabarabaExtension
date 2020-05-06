using Redfox.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace MorabarabaExtension
{
    class MorabarabaBoard
    {
        public Dictionary<string, MorabarabaField> fields;
        public List<string[]> millCombos;
        public short turn;
        public MorabarabaPlayerContext[] playerContexts;

        public MorabarabaBoard()
        {
            this.fields = new Dictionary<string, MorabarabaField>();
            var fieldlist = new string[] { "a7", "d7", "g7", "b6", "d6", "f6", "c5", "d5", "e5", "a4", "b4", "c4", "e4", "f4", "g4", "c3", "d3", "e3", "b2", "d2", "f2", "a1", "d1", "g1" };
            foreach(var fieldname in fieldlist)
            {
                this.fields.Add(fieldname, new MorabarabaField(fieldname));
            }
            this.millCombos = new List<string[]>();
            this.millCombos.Add(new string[] { "a7", "d7", "g7" });
            this.millCombos.Add(new string[] { "b6", "d6", "f6" });
            this.millCombos.Add(new string[] { "c5", "d5", "e5" });
            this.millCombos.Add(new string[] { "a4", "b4", "c4" });
            this.millCombos.Add(new string[] { "e4", "f4", "g4" });
            this.millCombos.Add(new string[] { "c3", "d3", "e3" });
            this.millCombos.Add(new string[] { "b2", "d2", "f2" });
            this.millCombos.Add(new string[] { "a1", "d1", "g1" }); //end horizontals
            this.millCombos.Add(new string[] { "a7", "a4", "a1" });
            this.millCombos.Add(new string[] { "b6", "b4", "b2" });
            this.millCombos.Add(new string[] { "c5", "c4", "c3" });
            this.millCombos.Add(new string[] { "d7", "d6", "d5" });
            this.millCombos.Add(new string[] { "d3", "d2", "d1" });
            this.millCombos.Add(new string[] { "e5", "e4", "e3" });
            this.millCombos.Add(new string[] { "f6", "f4", "f2" });
            this.millCombos.Add(new string[] { "g7", "g4", "g1" }); //end verticals
            this.millCombos.Add(new string[] { "a7", "b6", "c5" });
            this.millCombos.Add(new string[] { "e5", "f6", "g7" });
            this.millCombos.Add(new string[] { "e3", "f2", "g1" });
            this.millCombos.Add(new string[] { "a1", "b2", "c3" }); //end diagonals

            this.turn = 0;
            this.playerContexts = new MorabarabaPlayerContext[] { new MorabarabaPlayerContext(0), new MorabarabaPlayerContext(1) };
        }
        public bool MakeMove(int playerid, MorabarabaMove move)
        {
            int enemyid = GetEnemyId(playerid);

            if (move.moveType == MoveType.PLACE)
            {
                move.destination.value = playerid;
            }
            else if(move.moveType == MoveType.MOVE)
            {
                move.source.value = -1;
                move.destination.value = playerid;
            }
            else if (move.moveType == MoveType.PLACESHOOT)
            {
                move.destination.value = playerid;
                move.target.value = -1;
            }
            else if (move.moveType == MoveType.MOVESHOOT)
            {
                move.source.value = -1;
                move.destination.value = playerid;
                move.target.value = -1;
                
            }
            if(move.moveType == MoveType.PLACE || move.moveType == MoveType.PLACESHOOT)
            {
                this.playerContexts[playerid].placingCowsLeft--;
                if(this.playerContexts[playerid].placingCowsLeft == 0)
                {
                    this.playerContexts[playerid].phase = MorabarabaPhase.MOVING;
                }
                this.playerContexts[playerid].cows++;
            }
            if (move.moveType == MoveType.PLACESHOOT || move.moveType == MoveType.MOVESHOOT)
            {
                this.playerContexts[playerid].mills++;
                this.playerContexts[enemyid].cows--;
                if (this.playerContexts[enemyid].cows <= 3)
                {
                    this.playerContexts[enemyid].phase = MorabarabaPhase.FLYING;
                }
            }
            if (this.turn == 0)
            {
                this.turn = 1;
            } else
            {
                this.turn = 0;
            }
            if(this.playerContexts[enemyid].cows == 0)
            {
                //Player won
                return true;
            }
            return false;
        }
        public MorabarabaMove ParseAndValidateMove(int playerid, string move)
        {
            if (playerid < 0 || playerid > 1) return null; //invalid player id
            if (turn != playerid) return null; //not player's turn
            if(move.Length == 2)
            {
                //Place a cow (no shooting)
                if (this.playerContexts[playerid].phase == MorabarabaPhase.PLACING)
                {
                    if (this.playerContexts[playerid].placingCowsLeft > 0)
                    {
                        if (this.fields.ContainsKey(move))
                        {
                            if (this.fields[move].value == -1)
                            {
                                MorabarabaMove moveObj = new MorabarabaMove();
                                moveObj.moveType = MoveType.PLACE;
                                moveObj.destination = this.fields[move];
                                return moveObj;
                            }
                        }
                    }
                }
            } 
            else if(move.Length == 5 && move.Contains('-'))
            {
                //Move a cow (no shooting)
                if (this.playerContexts[playerid].phase == MorabarabaPhase.MOVING)
                {
                    var moveFields = move.Split('-');
                    if (this.fields.ContainsKey(moveFields[0]) && this.fields.ContainsKey(moveFields[1]))
                    {
                        if (this.fields[moveFields[0]].value == playerid && this.fields[moveFields[1]].value == -1)
                        {
                            MorabarabaMove moveObj = new MorabarabaMove();
                            moveObj.moveType = MoveType.MOVE;
                            moveObj.source = this.fields[moveFields[0]];
                            moveObj.destination = this.fields[moveFields[1]];
                            return moveObj;
                        }
                    }
                }
            } 
            else if(move.Length == 5 && move.Contains('x'))
            {
                //Place a cow and shoot
                if (this.playerContexts[playerid].phase == MorabarabaPhase.PLACING)
                {
                    if (this.playerContexts[playerid].placingCowsLeft > 0)
                    { 
                        var shotspl = move.Split('x');
                        if (this.fields.ContainsKey(shotspl[0]) && this.fields.ContainsKey(shotspl[1]))
                        {
                            if (this.fields[shotspl[0]].value == -1 && this.fields[shotspl[1]].value != playerid && this.fields[shotspl[1]].value != -1)
                            {
                                MorabarabaMove moveObj = new MorabarabaMove();
                                moveObj.moveType = MoveType.PLACESHOOT;
                                moveObj.destination = this.fields[shotspl[0]];
                                moveObj.target = this.fields[shotspl[1]];
                                return moveObj;
                            }
                        }
                    }
                }
            } 
            else if(move.Length == 8 && move.Contains('-') && move.Contains('x'))
            {
                //Move a cow and shoot
                if (this.playerContexts[playerid].phase == MorabarabaPhase.MOVING)
                {
                    var shotspl = move.Split('x');
                    var moveFields = shotspl[0].Split('-');
                    if (this.fields.ContainsKey(moveFields[0]) && this.fields.ContainsKey(moveFields[1]) && this.fields.ContainsKey(shotspl[1]))
                    {
                        if (this.fields[moveFields[0]].value == playerid && this.fields[moveFields[1]].value == -1 && this.fields[shotspl[1]].value != playerid && this.fields[shotspl[1]].value != -1)
                        {
                            MorabarabaMove moveObj = new MorabarabaMove();
                            moveObj.moveType = MoveType.MOVESHOOT;
                            moveObj.source = this.fields[moveFields[0]];
                            moveObj.destination = this.fields[moveFields[1]];
                            moveObj.target = this.fields[shotspl[1]];
                            return moveObj;
                        }
                    }
                }
            }
            return null;
        }
        public int GetEnemyId(int playerid)
        {
            if (playerid == 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
