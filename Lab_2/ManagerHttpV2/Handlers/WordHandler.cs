using ManagerHttp;
using ManagerHttp.ResponseBodies;
using MessagesBetweenManagerAndWorker;
using System;
using System.Runtime.InteropServices.Marshalling;

namespace ManagerHttpV2.Handlers
{
    public class WordHandler
    {

        public static void Handle(ProjectionTaskRepository projectionTaskRepository, MessageForDecryptedWord word)
        {
            Console.WriteLine(word);
            if (word.ErrorTimeoutFlag)
            {
                projectionTaskRepository.UpdateTaskStatus(word.Id, word.LetterCheckArray, STATUS.ERROR);
            }
            else
            {
                projectionTaskRepository.UpdateTaskWordAndStatus(word.Id, word.LetterCheckArray, STATUS.READY, word.Word);
            }
        }
    }
}
