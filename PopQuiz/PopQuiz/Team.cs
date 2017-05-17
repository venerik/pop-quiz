using System.Collections.Generic;

namespace PopQuiz
{
    public class Team
    {
        private List<Player> _players = new List<Player>();
        public string Name { get; set; }
        public Player Captain { get; set; }
        public IEnumerable<Player> Players { get { return _players; } }
        public void AddPlayer(Player player)
        {
            _players.Add(player);
        }
    }
}