using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorneioManager.Data;
using TorneioManager.Models;

namespace TorneioManager.Controllers
{
    public class TournamentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TournamentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Tournaments.Include(t => t.Participants).Include(t => t.Matches).ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var tournament = await _context.Tournaments
                .Include(t => t.Participants)
                .Include(t => t.Matches)
                .ThenInclude(m => m.Participant1)
                .Include(t => t.Matches)
                .ThenInclude(m => m.Participant2)
                .FirstOrDefaultAsync(t => t.TournamentId == id);

            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetWinner(int matchId, int winnerId)
        {
            var match = await _context.Matches.FindAsync(matchId);

            if (match == null)
            {
                return NotFound();
            }

            match.WinnerId = winnerId;
            await _context.SaveChangesAsync();

            if (AreAllMatchesComplete(match.TournamentId))
            {
                GenerateNextRoundMatches(match.TournamentId);
            }

            return RedirectToAction(nameof(Details), new { id = match.TournamentId });
        }

        private bool AreAllMatchesComplete(int tournamentId)
        {
            var tournament = _context.Tournaments
                .Include(t => t.Matches)
                .FirstOrDefault(t => t.TournamentId == tournamentId);

            return tournament.Matches.Where(m => m.Round == tournament.Matches.Max(r => r.Round)).All(m => m.WinnerId != null);
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

            // Seleciona os vencedores da última rodada
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
                return; // Não há vencedores suficientes para criar uma nova rodada
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
                        Round = currentRound + 1 // Define a rodada para as próximas partidas
                    });
                }
            }

            _context.Matches.AddRange(nextRoundMatches);
            _context.SaveChanges();
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tournament tournament, List<Participant> participants)
        {
            tournament.Participants = participants;
            _context.Add(tournament);
            await _context.SaveChangesAsync();
            GenerateInitialMatches(tournament);
            return RedirectToAction(nameof(Index));
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
                        Round = 1 // Set the round for the initial matches
                    });
                }
            }

            _context.Matches.AddRange(matches);
            _context.SaveChanges();
        }
    }
}