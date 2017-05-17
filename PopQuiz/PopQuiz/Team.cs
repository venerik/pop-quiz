using System.Collections.Generic;

namespace PopQuiz
{
    public class Team
    {
        public string Name { get; set; }

        public Team(string name)
        {
            Name = name;
        }

        public static Dictionary<int, Team> GetPlayer()
        {
            var player = new Dictionary<int, Team>();
            var myPlayer = new Team("Name1                     ");
            player.Add(1, myPlayer);
            myPlayer = new Team("Name2                     ");
            player.Add(2, myPlayer);
            myPlayer = new Team("Name3                     ");
            player.Add(3, myPlayer);
            myPlayer = new Team("Name4                     ");
            player.Add(4, myPlayer);
            myPlayer = new Team("Name5                     ");
            player.Add(5, myPlayer);
            myPlayer = new Team("Name6                     ");
            player.Add(6, myPlayer);
            myPlayer = new Team("Name7                     ");
            player.Add(7, myPlayer);
            myPlayer = new Team("Name8                     ");
            player.Add(8, myPlayer);

            return player;
        }
    }
}