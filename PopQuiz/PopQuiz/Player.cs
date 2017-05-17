using System.Collections.Generic;

namespace PopQuiz
{
    public class Player
    {
        public string Name { get; set; }

        public Player(string name)
        {
            Name = name;
        }

        public static Dictionary<int, Player> GetPlayers()
        {
            var players = new Dictionary<int, Player>();
            var player = new Player("Name 1");
            players.Add(1, player);
            player = new Player("Name 2");
            players.Add(2, player);
            player = new Player("Name 3");
            players.Add(3, player);
            player = new Player("Name 4");
            players.Add(4, player);
            player = new Player("Name 5");
            players.Add(5, player);
            player = new Player("Name 6");
            players.Add(6, player);
            player = new Player("Name 7");
            players.Add(7, player);
            player = new Player("Name 8");
            players.Add(8, player);

            return players;
        }
    }
}