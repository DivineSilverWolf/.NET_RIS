namespace MessagesBetweenManagerAndWorker
{
    public class MessageForDecryptedWord(string Word, string Id, bool ErrorTimeoutFlag = false)
    {
        public bool ErrorTimeoutFlag { get; set; } = ErrorTimeoutFlag;
        public string? Word { get; set; } = Word;
        public string? Id { get; set; } = Id;
    }
}
