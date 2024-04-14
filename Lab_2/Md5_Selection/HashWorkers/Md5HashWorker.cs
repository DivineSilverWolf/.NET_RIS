using Md5_Selection.HashWorkers.InterfaceHashWorker;
using static System.Security.Cryptography.MD5;
using System.Text;

namespace Md5_Selection.HashWorkers
{
    public class Md5HashWorker : IHashWorker
    {
        public string GetHash(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = HashData(inputBytes);
            var sb = new StringBuilder();

            foreach (var t in hashBytes)
            {
                sb.Append(t.ToString("x2"));
            }

            return sb.ToString();
        }
        public void Test()
        {
            Console.WriteLine("Hello MD5");
        }
    }
}
