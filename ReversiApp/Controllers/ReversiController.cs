using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReversiApp.DAL;
using ReversiApp.Data;
using ReversiApp.Models;

namespace ReversiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReversiController : ControllerBase
    {
        private readonly SpelContext spelContext;
        private readonly IdentityContext _identityContext;

        private readonly UserManager<Speler> userManager;
        private readonly Speler speler;
        private readonly Spel spel;

        private IHttpContextAccessor accessor;
        private HttpContext httpContext { get { return accessor.HttpContext; } }

        public ReversiController(SpelContext context, IdentityContext identity, UserManager<Speler> manager, IHttpContextAccessor aContext)
        {
            this.spelContext = context;
            _identityContext = identity;
            userManager = manager;
            this.accessor = aContext;
            speler = userManager.GetUserAsync(httpContext.User).Result;


            spel = spelContext.Spel.FirstOrDefault(a => a.Token == speler.Token);
        }


        // Endpoints
        // GET: api/Reversi/Bord
        [HttpGet("Bord")]
        [Authorize]
        public ActionResult<string> GetSpel()
        {
            try
            {
                return spel.JsonBord;
            }
            catch
            {
                return null;
            }
        }

        // POST: api/Reversi/Doezet/2&2
        [HttpGet("Doezet/{rij}&{kolom}")]
        [Authorize]
        public async Task<bool> DoeZet(int rij, int kolom)
        {
            spel.Pas();
            if (speler.Kleur == spel.AandeBeurt)
            {
                if (spel.ZetMogelijk(rij, kolom))
                {
                    spel.DoeZet(rij, kolom);
                    spelContext.Update(spel);
                    await spelContext.SaveChangesAsync();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // GET: api/Reversi/isAfgelopen
        [HttpGet("isAfgelopen")]
        [Authorize]
        public bool isAfgelopen()
        {
            return spel.Afgelopen();
        }

       
    }
}