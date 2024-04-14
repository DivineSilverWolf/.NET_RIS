using Md5_Selection.HashWorkers.InterfaceHashWorker;

namespace Md5_Selection
{
    public class WorkerSearchForHash
    {
        private readonly IHashWorker _hashWorker;
        public char[] Alphabet { get; private set; }
        public char[] LetterCheckArray { get; set; }
        public /*volatile*/ bool StopFlag { get; set; } = false;
        public WorkerSearchForHash(string alphabet, IHashWorker hashWorker)
        {
            this._hashWorker = hashWorker;
            this.Alphabet = alphabet.ToCharArray();
        }

        public string StartFindWordForHash(int maxLength, string hash)
        {

            for (var length = 0; length <= maxLength; length++)
            {
                Console.WriteLine(length);
                var word = SearchCorrectWordRecursive(length, "", hash);
                if (word != null)
                    return word;
                if (StopFlag)
                    return null!;
            }
            return null!;
        }

        private string SearchCorrectWordRecursive(int length, string currentWord, string hash)
        {
            if (StopFlag)
                return null!;
            if (length == 0)
            {
                return LetterCheckArray.Where(letter => _hashWorker.GetHash(letter + currentWord) == hash).Select(letter => letter + currentWord).FirstOrDefault();
    //            _hashWorker.GetHash(currentWord) == hash ? currentWord : null!;
            }
            return Alphabet.Select(letter => currentWord + letter)
                .Select(newWord => SearchCorrectWordRecursive(length - 1, newWord, hash))
                .FirstOrDefault(word => word != null);
        }
    }
}
