using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using static Niwatori.CustomPropertiesTag;

namespace Niwatori
{
    public static class PlayerPropertiesExtensions
    {

        private static readonly Hashtable props = new Hashtable();

        public static int GetPlayerNumber(this Player player)
        {
            return (player.CustomProperties[PLAYER_NUMBER] is int n) ? n : 0;
        }

        public static int GetTeamNumber(this Player player)
        {
            return (player.CustomProperties[TEAM_NUMBER] is int n) ? n : 0;
        }

        public static void SetPlayerNumber(this Player player, int value)
        {
            props[PLAYER_NUMBER] = value;
            player.SetCustomProperties(props);
            props.Clear();
        }
        public static void SetTeamNumber(this Player player, int value)
        {
            props[TEAM_NUMBER] = value;
            player.SetCustomProperties(props);
            props.Clear();
        }
    }

    public static class RoomPropertiesExtensions
    {
        private static readonly Hashtable props = new Hashtable();

        public static int GetNumberOfPlayers(this Room room)
        {
            return (room.CustomProperties[NUMBER_OF_PLAYER] is int n) ? n : 0;
        }

        public static void SetNumberOfPlayers(this Room room, int value)
        {
            props[NUMBER_OF_PLAYER] = value;
            room.SetCustomProperties(props);
            props.Clear();
        }

        public static bool TryGetStartTime(this Room room, out int timestamp)
        {
            if (room.CustomProperties[START_TIME] is int value)
            {
                timestamp = value;
                return true;
            }
            else
            {
                timestamp = 0;
                return false;
            }
        }

        public static void SetStartTime(this Room room, int timestamp)
        {
            props[START_TIME] = timestamp;
            room.SetCustomProperties(props);
            props.Clear();
        }

        public static void TeamDeath(this Room room, int teamNumber)
        {
            props[TEAM_DEATH + teamNumber] = true;
            room.SetCustomProperties(props);
            props.Clear();
        }

        public static bool GetTeamDeath(this Room room, int teamNumber)
        {
            return (room.CustomProperties[TEAM_DEATH + teamNumber] is bool b) ? b : false;
        }

        public static void InitTeamDeath(this Room room)
        {
            for (int i = 0; i < GameSettings.maxPlayer / 2; i++)
            {
                props[TEAM_DEATH + i] = false;
            }
            room.SetCustomProperties(props);
            props.Clear();
        }
    }

    public static class CustomPropertiesTag
    {
        public static string PLAYER_NUMBER = "pn";
        public static string TEAM_NUMBER = "tn";
        public static string NUMBER_OF_PLAYER = "np";
        public static string START_TIME = "st";
        public static string TEAM_DEATH = "td";
    }
}