using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;




namespace ReversiRestApi.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class ReversiController : ControllerBase
    {
        
        
        private static List<Spel> spel = new List<Spel>()
        {
            new Spel{ ID=1, Omschrijving="Dit is spel 1"},
            new Spel{ ID=2, Omschrijving="Dit is spel 2"},
            new Spel{ ID=3, Omschrijving="Dit is spel 3"}
        };

        [HttpGet]
        public ActionResult<IEnumerable<Spel>> Get()
        {
            return spel;
        }

        [HttpGet("{id}", Name = "Get")]
        public ActionResult<Spel> Get(int id)
        {
            var result = spel.FirstOrDefault(item => item.ID == id);
            if (result != null) return result;
            else return new Spel { ID = 0, Omschrijving = "Invalid Id!" };
        }

        [HttpPost]
        public void Post([FromBody] Spel game)
        {
            spel.Add(game);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Spel game)
        {
            var result = spel.FirstOrDefault(item => item.ID == id);
            if (result != null)
            {
                
                result.Omschrijving = game.Omschrijving;
            }

        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var ItemToRemove = spel.FirstOrDefault(item => item.ID == id);
            if (ItemToRemove != null)
            {
                spel.Remove(ItemToRemove);
            }
        }
    }
}
