using ManagerHttp.Requests;
using ManagerHttp.ResponseBodies;
using ManagerHttpV2.Handlers;
using MassTransit;
using MessagesBetweenManagerAndWorker;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ManagerHttp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManagerController : ControllerBase
    {

        private readonly ILogger<ManagerController> _logger;
        private readonly ProjectionTaskRepository _projectionTaskRepository;
        private readonly IRequest _request;
        private readonly IBus _bus;

        public ManagerController(ILogger<ManagerController> logger, ProjectionTaskRepository projectionTaskRepository, IRequest request, IBus bus)
        {
            _projectionTaskRepository = projectionTaskRepository;
            _logger = logger;
            _request = request;
            _bus = bus;
            Console.WriteLine(bus);

        }

        [HttpPost("/api/hash/crack")]
        public IActionResult Post(string hash, int maxLength)
        {
            if (maxLength < 1 || maxLength > 10)
                return BadRequest("Bad request: " +  maxLength + " > 10. 0 < MaxLength <= 10");
            var id = new UniqueIdentifierForUser();
            var (requestBool, tasks) = _request.Request(maxLength - 1, hash, id.RequestId);
            if (!requestBool)
                return StatusCode(500, "Ошибка сервера");
            foreach (string task in tasks) {
                _projectionTaskRepository.AddProjectionTask(new ManagerHttpV2.ProjectionTask(id.RequestId, task.ToCharArray()));
            }
            return Ok(id);
        }

        [HttpPatch("/internal/api/manager/hash/crack/request")]
        public IActionResult Patch([FromBody] MessageForDecryptedWord word)
        {
            WordHandler.Handle(_projectionTaskRepository, word);
            return Ok();
        }
        [HttpGet("/api/hash/status")]
        public IActionResult Get(string requestId)
        {
            List<ManagerHttpV2.ProjectionTask>? tasks = _projectionTaskRepository.SearchFromUserId(requestId);
            if (tasks.Count == 0)
                return BadRequest("Такого идентификатора не существует");

            STATUS status = STATUS.ERROR;
            string word = "";
            foreach (var task in tasks){
                if (task.Status > status) {
                    status = task.Status;
                    if (status == STATUS.READY) {
                        word += task.Word;
                    }      
                }
            }
            
            if (status == STATUS.ERROR)
                return BadRequest("TimeOut");
            else if(status == STATUS.IN_PROGRESS)
                return Ok(new StatusWordResponseBody(STATUS.IN_PROGRESS, null));
            return Ok(new StatusWordResponseBody(STATUS.READY, word));
        }

    }
}
