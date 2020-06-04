using MorabarabaExtension.Messages.Responses;
using Redfox.Rooms;
using Redfox.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace MorabarabaExtension
{
    class GameSession
    {
        public int id;
        public Room room;
        public List<User> users;
        public MorabarabaBoard board;
        public GameSession(int _sessid, Room _room, User user1, User user2)
        {
            this.id = _sessid;
            this.room = _room;
            this.users = new List<User>();
            this.users.Add(user1);
            this.users.Add(user2);
        }
        public void StartGame()
        {
            this.board = new MorabarabaBoard();
            foreach (User user in users)
            {
                user.SendMessage(new GameBeginResponse(user == users[0]));
            }
        }
        public bool MakeMove(User user, string movestr)
        {
            int playerid = this.users.IndexOf(user);
            MorabarabaMove move = board.ParseAndValidateMove(playerid, movestr);
            if(move != null)
            {
                bool playerWon = board.MakeMove(playerid, move);
                board.addToHistory(movestr);
                if (playerWon)
                {
                    //Player won
                    foreach(User target in users)
                    {
                        target.SendMessage(new GameEndResponse(target == user));
                        user.Zone.RoomManager.GetRoom("lobby").Join(target);
                    }
                }
                return true;
            } else
            {
                return false;
            }
        }
    }
}
