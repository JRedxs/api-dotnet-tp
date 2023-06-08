using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private List<Command> _commands;

        public CommandController()
        {
            _commands = new List<Command>
            {
                new Command { Id = 1, Items = new List<string> { "Entrée", "Plat", "Boisson" }, Status = CommandStatus.Enregistre },
                new Command { Id = 2, Items = new List<string> { "Plat", "Dessert" }, Status = CommandStatus.Preparation },
                new Command { Id = 3, Items = new List<string> { "Entrée", "Plat" }, Status = CommandStatus.Livre }

            };
        }

        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetCommands()
        {
            return Ok(_commands);
        }

        [HttpGet("{id}")]
        public ActionResult<Command> GetCommand(int id)
        {
            var command = _commands.FirstOrDefault(c => c.Id == id);

            if (command == null)
                return NotFound();

            return Ok(command);
        }

        [HttpPost]
        public ActionResult<Command> CreateCommand(Command command)
        {
            command.Id = _commands.Count + 1;
            command.Status = CommandStatus.Enregistre;
            _commands.Add(command);

            return CreatedAtAction(nameof(GetCommand), new { id = command.Id }, command);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCommand(int id, Command updatedCommand)
        {
            var command = _commands.FirstOrDefault(c => c.Id == id);

            if (command == null)
                return NotFound();

            command.Items = updatedCommand.Items;
            command.Status = updatedCommand.Status;

            return NoContent();
        }

        [HttpPut("{id}/status")]
        public IActionResult UpdateCommandStatus(int id, CommandStatus status)
        {
            var command = _commands.FirstOrDefault(c => c.Id == id);

            if (command == null)
                return NotFound();

            if (status == CommandStatus.Livre)
            {
                if (command.Status == CommandStatus.Livre)
                    return BadRequest("La commande a déjà été livrée.");

                command.Status = status;
            }
            else
            {
                command.Status = status;
            }

            return NoContent();
        }
    }

    public class Command
    {
        public int Id { get; set; }
        public List<string> Items { get; set; }
        public CommandStatus Status { get; set; }

        public string StatusString
        {
            get
            {
                switch (Status)
                {
                    case CommandStatus.Enregistre:
                        return "Enregistré";
                    case CommandStatus.Preparation:
                        return "En préparation";
                    case CommandStatus.Livre:
                        return "Livrée";
                    default:
                        return "Statut inconnu";
                }
            }
        }
    }

    public enum CommandStatus
    {
        Enregistre,
        Preparation,
        Livre
    }
}
