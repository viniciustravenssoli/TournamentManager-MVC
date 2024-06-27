using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TorneioManager.Models
{
    public class Match
    {
        public int MatchId { get; set; }
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        public int Participant1Id { get; set; }
        public Participant Participant1 { get; set; }

        public int Participant2Id { get; set; }
        public Participant Participant2 { get; set; }

        public int? WinnerId { get; set; }
        public Participant Winner { get; set; }

        public int Round { get; set; } // New property to store the round
    }
}