using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTest
{
    public class StringManipulator
    {
        private readonly Func<char, char[], Tuple<char, char[]>> _createTuple = Tuple.Create;

        private List<Tuple<char, char[]>> HashedList => new List<Tuple<char, char[]>>(new[]
                {
                    _createTuple('i', new[] { 'i', 'I', '!' }),
                    _createTuple('l', new [] { 'l', 'L', '|' }),
                    _createTuple('m', new[] { 'm', 'M', 'w', 'W' }),
                    _createTuple('w', new[] { 'm', 'M', 'w', 'W' }),
                    _createTuple('d', new[] { 'd', 'D', 'B', 'p' }),
                    _createTuple('p', new[] { 'p', 'P', 'q', 'b' }),
                    _createTuple('o', new[] { 'o', 'O', '0' }),
                    _createTuple('a', new[] { 'a', 'A', '@', '^' }),
                    _createTuple('b', new[] { 'b', 'B', '8', '#' }),
                    _createTuple('e', new[] { 'e', 'E', '3' }),
                    _createTuple('s', new[] { 's', 'S', '$', '8' }),
                    _createTuple('f', new[] { 'f', 'F', '5' })
                });
        public string StringManipulatorBy(string inputString)
        {

            string hashedString="";
            foreach (char character in inputString.ToLower())
            {
                var hl = HashedList.FirstOrDefault(c=>c.Item1==character);
                if (hl != null) hashedString += hl.Item2[new Random().Next(0, hl.Item2.Length)];
                else
                {
                    hashedString += character;
                }
            }

            return hashedString;
        }



    }
}
