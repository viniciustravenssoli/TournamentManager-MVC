using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TorneioManager.Models
{
    public class Tournament
    {
        public int TournamentId { get; set; }
        public string Name { get; set; }
        public ICollection<Participant> Participants { get; set; }
        public ICollection<Match> Matches { get; set; }
    }
}