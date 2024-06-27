using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorneioManager.Data;
using TorneioManager.Models;
using TorneioManager.Services;

namespace TorneioManager.Controllers
{
    public class TournamentsController : Controller
    {
        private readonly ITournamentService _tournamentService;

        public TournamentsController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public async Task<IActionResult> Index()
        {
            var tournaments = await _tournamentService.GetAllTournamentsAsync();
            return View(tournaments);
        }

        public async Task<IActionResult> Details(int id)
        {
            var tournament = await _tournamentService.GetTournamentDetailsAsync(id);

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
            var result = await _tournamentService.SetMatchWinnerAsync(matchId, winnerId);
            var match = await _tournamentService.FindById(matchId);

            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Details), new { id = match.TournamentId });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tournament tournament, List<Participant> participants)
        {

            await _tournamentService.CreateTournamentAsync(tournament, participants);
            return RedirectToAction(nameof(Index));


        }
    }
}