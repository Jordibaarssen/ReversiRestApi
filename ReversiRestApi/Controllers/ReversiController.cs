using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ReversiRestApi.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class ReversiController : ControllerBase
    {
        private List<Spel> games = new List<Spel> {
            new Spel {Token = "a1", AandeBeurt = Kleur.Wit, ID = 1, Omschrijving = "Test",},
            new Spel {Token = "a2", AandeBeurt = Kleur.Wit, ID = 2, Omschrijving = "hoi" },
            new Spel {Token = "a3", AandeBeurt = Kleur.Wit, ID = 3, Omschrijving = "doei" }
        };

        public List<SpelDTO> gameDTO1;

        public List<SpelDTO> ConvertBord(List<Spel> spel)
        {
            Console.WriteLine("-------------------------------------------------------------");
            foreach (var v in spel)
            {
                SpelDTO game = new SpelDTO
                {
                    Bord = JsonConvert.SerializeObject(v.Bord),
                    ID = v.ID,
                    Token = v.Token,
                    AandeBeurt = v.AandeBeurt,
                    Omschrijving = v.Omschrijving,
                    Spelers = v.Spelers,
                    Status = null
                };
                Console.WriteLine(game);
                gameDTO1.Add(game);
                
            }
            Console.WriteLine("-------------------------------------------------------------------------");

            return gameDTO1;
        }

        


        [HttpGet]
        public List<SpelDTO> Get()
        {
            try
            {
                return (ConvertBord(games));
            }
            catch
            {
                return null;
            }
        }

        // Endpoints
        // GET: api/Reversi/{token}
        [HttpGet("{token}")]
        public ActionResult<SpelDTO> GetSpel(string token)
        {
            try
            {
                Spel tokenGame = (from game in games
                                  where game.Token == token
                                  select game).First();

                return new SpelDTO
                {
                    Omschrijving = tokenGame.Omschrijving,
                    Token = tokenGame.Token,
                    Spelers = tokenGame.Spelers,
                    Bord = JsonConvert.SerializeObject(tokenGame.Bord),
                    AandeBeurt = tokenGame.AandeBeurt,
                    Status = null,
                    ID = tokenGame.ID
                    
                };
            }
            catch
            {
                return null;
            }
        }

        // GET: api/Reversi/Beurt/{token}
        [HttpGet("Beurt/{token}")]
        public ActionResult<Kleur> GetBeurt(string token)
        {
            try
            {
                return (from game in games
                        where game.Token == token
                        select game).First().AandeBeurt;
            }
            catch
            {
                return null;
            }
        }



        //private static List<Spel> spel = new List<Spel>()
        //{
        //    new Spel{ ID=1, Omschrijving="Dit is spel 1"},
        //    new Spel{ ID=2, Omschrijving="Dit is spel 2"},
        //    new Spel{ ID=3, Omschrijving="Dit is spel 3"}
        //};

        //[HttpGet]
        //public ActionResult<IEnumerable<Spel>> Get()
        //{
        //    return spel;
        //}

        //[HttpGet("{id}", Name = "Get")]
        //public ActionResult<Spel> Get(int id)
        //{
        //    var result = spel.FirstOrDefault(item => item.ID == id);
        //    if (result != null) return result;
        //    else return new Spel { ID = 0, Omschrijving = "Invalid Id!" };
        //}

        //[HttpPost]
        //public void Post([FromBody] Spel game)
        //{
        //    spel.Add(game);
        //}

        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] Spel game)
        //{
        //    var result = spel.FirstOrDefault(item => item.ID == id);
        //    if (result != null)
        //    {
                
        //        result.Omschrijving = game.Omschrijving;
        //    }

        //}

        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //    var ItemToRemove = spel.FirstOrDefault(item => item.ID == id);
        //    if (ItemToRemove != null)
        //    {
        //        spel.Remove(ItemToRemove);
        //    }
        //}
    }
}
