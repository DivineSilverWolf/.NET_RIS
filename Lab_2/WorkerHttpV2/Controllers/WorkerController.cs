using Md5_Selection;
using Md5_Selection.HashWorkers.InterfaceHashWorker;
using MessagesBetweenManagerAndWorker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkerHttp.TimeoutPerformers;
using WorkerHttp.Requests;

namespace WorkerHttp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkerController : ControllerBase
    {
        private readonly ILogger<WorkerController> _logger;

        private readonly string _myValue;
        private readonly WorkerSearchForHash workerSearchForHash;
        private readonly IExecutorTimeOut _executorTime;
        private readonly IRequest _request;
        public WorkerController(ILogger<WorkerController> logger, IOptions<AlphabetSetting> appSettings, IHashWorker hashWorker, IExecutorTimeOut executor, IRequest request)
        {
            _logger = logger;
            _myValue = appSettings.Value.Alphabet;
            workerSearchForHash = new WorkerSearchForHash(appSettings.Value.Alphabet, hashWorker);
            _executorTime = executor;
            _request = request;
        }


        [HttpPost("/search/word/for/hash")]
        public IActionResult Post([FromBody] HashCodeMessage hash)
        {
            ProcessHash(hash);
            return Ok();
        }
        private void ProcessHash(HashCodeMessage hash)
        {
            Task.Run(() =>
            {
                string word = _executorTime.GetWordForHash(workerSearchForHash, hash, out bool flag);
                _request.SendWordToManager(word, hash, flag);
            });
        }

    }
}
