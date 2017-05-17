using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopQuiz
{
    public class View
    {
        private const int ColumnWidth = 25;

        public static string PrintTeams(Team team1, Team team2)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} {1}\n", "Team 1".PadLeft(ColumnWidth), "Team 2".PadLeft(ColumnWidth));
            sb.AppendLine($"{team1.Name,ColumnWidth} {team2.Name,ColumnWidth}");
            if (team1.Captain != null)
            {
                sb.AppendLine(PrintPlayers(team1.Captain, team2.Captain));
            }
            else
            {
                sb.Append(PrintTeamPlayers(team1.Players.ToList(), team2.Players.ToList()));
            }
            return sb.ToString();
        }

        private static string PrintTeamPlayers(List<Player> players1, List<Player> players2)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < players1.Count; i++)
            {
                sb.AppendLine(PrintPlayers(players1[i], players2[i]));
            }
            return sb.ToString();
        }

        private static string PrintPlayers(Player player1, Player player2)
        {
            return $"{player1.Name,ColumnWidth} {player2.Name,ColumnWidth}";
        }
    }
}
