using NLog;
using Redfox.Extensions;
using Redfox.Rooms;
using Redfox.Users;
using Redfox.Zones;

namespace MorabarabaExtension
{
    public class MorabarabaExt : RedfoxExtension
    {
        public MorabarabaExt() : base("MorabarabaExtension") { }

        public override void Initialize()
        {
            this.extensionEventManager.ZoneReady += OnZoneReady;
            this.extensionEventManager.ZoneLeave += OnZoneLeave;
            this.extensionEventManager.RoomLeave += OnRoomLeave;
        }

        private void OnZoneReady()
        {
            LogManager.GetCurrentClassLogger().Info("Morabaraba server extension ready!");
        }
        private void OnZoneLeave(User user, Zone zone)
        {
            MorabarabaController.DequeueUser(user);
            MorabarabaController.LeaveSession(user);
        }
        private void OnRoomLeave(User user, Room room)
        {
        }
    }
}
