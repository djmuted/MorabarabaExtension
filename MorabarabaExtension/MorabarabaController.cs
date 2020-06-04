using Redfox.Configs;
using Redfox.Rooms;
using Redfox.Users;
using Redfox.Users.UserVariables;
using Redfox.Zones;
using System;
using System.Collections.Generic;
using System.Text;

namespace MorabarabaExtension
{
    static class MorabarabaController
    {
        public static SortedDictionary<int, GameSession> gameSessions = new SortedDictionary<int, GameSession>();
        private static List<User> enqueuedUsers = new List<User>();

        public static int GetLowestFreeSessionID()
        {
            int lastid = -1;
            foreach(int key in gameSessions.Keys)
            {
                if(key != lastid + 1)
                {
                    return lastid + 1;
                }
                lastid = key;
            }
            return lastid + 1;
        }
        public static GameSession CreateGameSession(Zone zone, User user1, User user2)
        {
            RoomConfig cfg = new RoomConfig();
            int sessid = GetLowestFreeSessionID();
            cfg.room_name = "morabaraba_" +sessid;
            cfg.max_users = 2;
            Room gameroom = new Room(cfg);
            zone.RoomManager.AddRoom(gameroom);
            var sess = new GameSession(sessid, gameroom, user1, user2);
            gameSessions.Add(sessid, sess);
            return sess;
        }
        public static void EnqueueUser(User user)
        {
            if (enqueuedUsers.Contains(user)) return;
            enqueuedUsers.Add(user);
            if(enqueuedUsers.Count >= 2)
            {
                var sess = CreateGameSession(enqueuedUsers[0].Zone, enqueuedUsers[0], enqueuedUsers[1]);
                sess.room.Join(enqueuedUsers[0]);
                sess.room.Join(enqueuedUsers[1]);
                enqueuedUsers[0].UserVariables.Add("morabaraba_session", new UserVariable<int>("morabaraba_session", sess.id, true));
                enqueuedUsers[1].UserVariables.Add("morabaraba_session", new UserVariable<int>("morabaraba_session", sess.id, true));
                enqueuedUsers.RemoveRange(0, 2);
                sess.StartGame();
            }
        }
        public static void DequeueUser(User user)
        {
            if (!enqueuedUsers.Contains(user)) return;
            enqueuedUsers.Remove(user);
        }
        public static void LeaveSession(User user)
        {
            if(user.UserVariables.ContainsKey("morabaraba_session"))
            {
                int sessid = (user.UserVariables["morabaraba_session"] as UserVariable<int>).Value;
                GameSession sess = gameSessions[sessid];
                foreach(User gameuser in sess.users)
                {
                    if (gameuser == user)
                    {
                        sess.room.Leave(gameuser);
                    } else
                    {
                        gameuser.Zone.RoomManager.GetRoom("lobby").Join(gameuser);
                    }
                    gameuser.UserVariables.Remove("morabaraba_session");
                }
                user.Zone.RoomManager.RemoveRoom(sess.room);
                gameSessions.Remove(sessid);
            }
        }
    }
}
