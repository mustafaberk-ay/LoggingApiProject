using LoggingApiProject.Data;
using LoggingApiProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace LoggingApiProject.Controllers
{
    [ApiController]
    public class LogMessageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static readonly HttpClient client = new HttpClient();

        public LogMessageController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("receive")]
        public IActionResult Receive([FromBody] LogMessage payload)
        {
            try
            {
                _context.Logs.Add(payload);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                var errorPayload = new LogMessage { Message = $"Exception {e.Message} thrown", Timestamp = DateTime.Now };
                _context.Logs.Add(errorPayload);
                _context.SaveChanges();
                return BadRequest(e.Message);
            }
        }

        [HttpPost("sendLogs")]
        public async Task<IActionResult> SendLogs()
        {
            var logs = _context.Logs.ToList();
            var jsonContent = JsonConvert.SerializeObject(logs);
            var response = await client.PostAsync("http://localhost:5006/GetLogs", new StringContent(jsonContent, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            return Ok();
        }
    }
}
