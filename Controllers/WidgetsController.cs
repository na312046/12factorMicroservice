using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ConsoleApplication.Data;
using ConsoleApplication.Entity;
using Microsoft.Extensions.Logging;

// TODO Replace direct use of database with repository.
namespace ConsoleApplication.Controllers
{
    [Route("api/[controller]")]
    public class WidgetsController : Controller
    {
        protected WidgetsContext _context;

        private readonly ILogger<WidgetsController> _logger;

        public WidgetsController(WidgetsContext context, ILogger<WidgetsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Widget> Get()
        {
            return _context.Widgets.AsEnumerable();
        }

        // GET api/values/2
        [HttpGet("{id}")]
        public Widget Get(int id)
        {
            return _context.Widgets.FirstOrDefault(w => w.Id == id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Widget widget)
        {
            System.Console.WriteLine("The Magic value is: " + widget);
            _context.Widgets.Add(widget);
            _context.SaveChanges();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var valToDelete = _context.Widgets.FirstOrDefault(w => w.Id == id);
            // TODO HTTP code 404 when null.
            if (valToDelete != null) {
                _context.Widgets.Remove(valToDelete);
                _context.SaveChanges();
            }
        }
    }
}
