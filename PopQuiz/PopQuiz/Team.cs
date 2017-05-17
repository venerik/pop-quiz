using System.Collections.Generic;

namespace PopQuiz
{
    public class Team
    {
        private List<Player> _players = new List<Player>();
        public string Name { get; set; }
        public Player Captain { get; set; }
        public IEnumerable<Player> Players { get { return _players; } }
        public int Score { get; private set; }
        public void AddPlayer(Player player)
        {
            _players.Add(player);
        }

        public void IncreaseScore(int points)
        {
            Score += points;
        }

        public void DecreaseScore(int penalty)
        {
            Score -= penalty;
        }
    }

    public enum TeamType
    {
        Players,
        Captains
    }
}