using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorneioManager.Models;

namespace TorneioManager.Services
{
    public interface ITournamentService
    {
        Task<List<Tournament>> GetAllTournamentsAsync();
        Task<Tournament> GetTournamentDetailsAsync(int id);
        Task<bool> SetMatchWinnerAsync(int matchId, int winnerId);
        Task CreateTournamentAsync(Tournament tournament, List<Participant> participants);
        Task<Match> FindById(int id);
    }
}