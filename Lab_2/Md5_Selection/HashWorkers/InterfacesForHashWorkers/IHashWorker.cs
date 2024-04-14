namespace Md5_Selection.HashWorkers.InterfaceHashWorker
{
    public interface IHashWorker
    {
        string GetHash(string word);
        void Test();
    }
}
