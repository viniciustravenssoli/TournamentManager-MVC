using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TorneioManager.Data;
using TorneioManager.Models;

namespace TorneioManager.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ApplicationDbContext _context;

        public TournamentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tournament>> GetAllTournamentsAsync()
        {
            return await _context.Tournaments
                .Include(t => t.Participants)
                .Include(t => t.Matches)
                .ToListAsync();
        }

        public async Task<Tournament> GetTournamentDetailsAsync(int id)
        {
            return await _context.Tournaments
                .Include(t => t.Participants)
                .Include(t => t.Matches)
                    .ThenInclude(m => m.Participant1)
                .Include(t => t.Matches)
                    .ThenInclude(m => m.Participant2)
                .FirstOrDefaultAsync(t => t.TournamentId == id);
        }

        public async Task<bool> SetMatchWinnerAsync(int matchId, int winnerId)
        {
            var match = await _context.Matches.FindAsync(matchId);

            if (match == null)
            {
                return false;
            }

            match.WinnerId = winnerId;
            await _context.SaveChangesAsync();

            if (AreAllMatchesComplete(match.TournamentId))
            {
                GenerateNextRoundMatches(match.TournamentId);
            }

            return true;
        }

        public async Task CreateTournamentAsync(Tournament tournament, List<Participant> participants)
        {
            tournament.Participants = participants;
            _context.Add(tournament);
            await _context.SaveChangesAsync();
            GenerateInitialMatches(tournament);
        }

        private bool AreAllMatchesComplete(int tournamentId)
        {
            var tournament = _context.Tournaments
                .Include(t => t.Matches)
                .FirstOrDefault(t => t.TournamentId == tournamentId);

            return tournament.Matches
                .Where(m => m.Round == tournament.Matches.Max(r => r.Round))
                .All(m => m.WinnerId != null);
        }

        private void GenerateNextRoundMatches(int tournamentId)
        {
            var tournament = _context.Tournaments
                .Include(t => t.Matches)
                    .ThenInclude(m => m.Winner)
                .FirstOrDefault(t => t.TournamentId == tournamentId);

            if (tournament == null)
            {
                return;
            }

            var winners = tournament.Matches
                .Where(m => m.WinnerId != null)
                .GroupBy(m => m.Round)
                .OrderByDescending(g => g.Key)
                .FirstOrDefault()
                ?.Select(m => m.Winner)
                .Distinct()
                .ToList();

            if (winners == null || winners.Count <= 1)
            {
                return;
            }

            var currentRound = tournament.Matches.Max(m => m.Round);
            var nextRoundMatches = new List<Match>();

            for (int i = 0; i < winners.Count; i += 2)
            {
                if (i + 1 < winners.Count)
                {
                    nextRoundMatches.Add(new Match
                    {
                        TournamentId = tournamentId,
                        Participant1Id = winners[i].ParticipantId,
                        Participant2Id = winners[i + 1].ParticipantId,
                        Round = currentRound + 1
                    });
                }
            }

            _context.Matches.AddRange(nextRoundMatches);
            _context.SaveChanges();
        }

        private void GenerateInitialMatches(Tournament tournament)
        {
            var shuffledParticipants = tournament.Participants.OrderBy(p => Guid.NewGuid()).ToList();

            var matches = new List<Match>();
            for (int i = 0; i < shuffledParticipants.Count; i += 2)
            {
                if (i + 1 < shuffledParticipants.Count)
                {
                    matches.Add(new Match
                    {
                        TournamentId = tournament.TournamentId,
                        Participant1Id = shuffledParticipants[i].ParticipantId,
                        Participant2Id = shuffledParticipants[i + 1].ParticipantId,
                        Round = 1
                    });
                }
            }

            _context.Matches.AddRange(matches);
            _context.SaveChanges();
        }

        public async Task<Match> FindById(int id)
        {
            return await _context.Matches.FindAsync(id);
        }
    }
}