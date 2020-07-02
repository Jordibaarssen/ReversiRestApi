using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReversiApp.DAL;
using ReversiApp.Data;
using ReversiApp.Models;

namespace ReversiApp.Controllers
{
    public class SpellenController : Controller
    {
        private readonly SpelContext _context;
        private readonly IdentityContext _identityContext;

        private readonly UserManager<Speler> userManager;
        private readonly Speler speler;

        private IHttpContextAccessor accessor;
        private HttpContext httpContext { get { return accessor.HttpContext; } }

        public SpellenController(SpelContext context, IdentityContext identity, UserManager<Speler> manager, IHttpContextAccessor aContext)
        {
            _context = context;
            _identityContext = identity;
            userManager = manager;
            this.accessor = aContext;
            speler = userManager.GetUserAsync(httpContext.User).Result;
        }

      
        // GET: Spellen
        [Authorize]
        public async Task<IActionResult> Index()
        {
            List<Spel> games = await _context.Spel.ToListAsync();
            List<Spel> spellenMetEen = new List<Spel>();

            foreach (var game in games)
            {
                if (speler.Token == game.Token)
                {
                    return RedirectToAction(nameof(Bord), new { id = game.Id });
                }

                var aantalSpelers = _identityContext.Spelers.Where(a => a.Token == game.Token).Select(b => b.Token).Count();
                if(aantalSpelers < 2)
                {
                    spellenMetEen.Add(game);
                }
            }
            return View(spellenMetEen);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Bord(int? id)
        {
            var game = await _context.Spel.FirstOrDefaultAsync(a => a.Id == id);
            var spelerEen = await _identityContext.Spelers.FirstOrDefaultAsync(a => a.Token == game.Token);
            if(spelerEen.Kleur == Kleur.Wit)
            {
                speler.Kleur = Kleur.Zwart;
            }
            else
            {
                speler.Kleur = Kleur.Wit;
            }

            speler.Token = game.Token;

            _identityContext.Update(speler);
            await _identityContext.SaveChangesAsync();
            return View(game);
        }

        // GET: Spellen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spel = await _context.Spel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spel == null)
            {
                return NotFound();
            }

            return View(spel);
        }

        


        // GET: Spellen/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Spellen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Omschrijving,Token,AandeBeurt")] Spel spel)
        {
            if (ModelState.IsValid)
            {

                _context.Add(spel);
                await _context.SaveChangesAsync();

                speler.Token = spel.Token;
                speler.Kleur = Kleur.Wit;
                _identityContext.Update(speler);
                await _identityContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(spel);
        }

        // GET: Spellen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spel = await _context.Spel.FindAsync(id);
            if (spel == null)
            {
                return NotFound();
            }
            return View(spel);
        }

        // POST: Spellen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Omschrijving,Token,AandeBeurt,JsonBord")] Spel spel)
        {
            if (id != spel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpelExists(spel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(spel);
        }

        // GET: Spellen/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spel = await _context.Spel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spel == null)
            {
                return NotFound();
            }

            return View(spel);
        }

        // POST: Spellen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var spel = await _context.Spel.FindAsync(id);
            _context.Spel.Remove(spel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Spellen/Leave/5
        [HttpPost, ActionName("Leave")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Leave(int id)
        {
            var spel = await _context.Spel.FindAsync(id);
            var aantalSpelers = _identityContext.Spelers.Where(a => a.Token == spel.Token).Select(b => b.Token).Count();

            speler.Token = null;
            speler.Kleur = 0;
            _identityContext.Update(speler);
            await _identityContext.SaveChangesAsync();

            if (aantalSpelers < 2)
            {
                _context.Spel.Remove(spel);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));

        }

        [HttpPost, ActionName("SpeelOpnieuw")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> SpeelOpnieuw(int id)
        {
            var newSpel = new Spel();
            var spel = await _context.Spel.FindAsync(id);

            spel.JsonBord = newSpel.JsonBord;

            _context.Update(spel);
            await _context.SaveChangesAsync();
           
            return RedirectToAction(nameof(Index));

        }

        private bool SpelExists(int id)
        {
            return _context.Spel.Any(e => e.Id == id);
        }
    }
}
