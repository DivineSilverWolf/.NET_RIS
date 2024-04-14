using ManagerHttp.Requests;
using ManagerHttp.ResponseBodies;
using MessagesBetweenManagerAndWorker;
using Microsoft.AspNetCore.Mvc;

namespace ManagerHttp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManagerController : ControllerBase
    {

        private readonly ILogger<ManagerController> _logger;
        private readonly SingletonDictonaryIdToWord _singletonDictonaryIdToWord;
        private readonly IRequest _request;

        public ManagerController(ILogger<ManagerController> logger, SingletonDictonaryIdToWord singletonDictonaryIdToWord, IRequest request)
        {
            _singletonDictonaryIdToWord = singletonDictonaryIdToWord;
            _logger = logger;
            _request = request;

        }

        [HttpPost("/api/hash/crack")]
        public IActionResult Post(string hash, int maxLength)
        {
            if (maxLength > 10)
                return BadRequest("Bad request: " +  maxLength + " > 10. 0 < MaxLength <= 10");
            var id = new UniqueIdentifierForUser();
            if(!_request.Request(maxLength, hash, id.RequestId))
                return StatusCode(500, "Ошибка сервера");
            _singletonDictonaryIdToWord.IdtoWord.TryAdd(id.RequestId, null!);
            return Ok(id);
        }

        [HttpPatch("/internal/api/manager/hash/crack/request")]
        public IActionResult Patch([FromBody] MessageForDecryptedWord word)
        {
            if (word.ErrorTimeoutFlag)
            {
                _singletonDictonaryIdToWord.IdtoWord.TryRemove(word.Id!, out _);
                _singletonDictonaryIdToWord.IdtoException.Add(word.Id!);
            }
            else
                _singletonDictonaryIdToWord.IdtoWord[word.Id!] = word.Word!;
            return Ok();
        }
        [HttpGet("/api/hash/status")]
        public IActionResult Get(string requestId)
        {
            if(_singletonDictonaryIdToWord.IdtoException.Contains(requestId))
                return BadRequest("TimeOut");
            if (!_singletonDictonaryIdToWord.IdtoWord.ContainsKey(requestId))
                return BadRequest("Такого идентификатора не существует");
            string word = _singletonDictonaryIdToWord.IdtoWord[requestId];
            if (word == null)
                return Ok(new StatusWordResponseBody(STATUS.IN_PROGRESS, word)) ;
            return Ok(new StatusWordResponseBody(STATUS.READY, word));
        }

    }
}
