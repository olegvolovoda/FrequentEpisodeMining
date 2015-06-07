using System;
using System.Collections.Generic;

namespace DiscreteApproach
{
    public class WordRepeat
    {
        public string Word;
        public int Times = 1000;
        public int MinGap = 2;
        public int MaxGap = 4;
    }

    public class ChainBuilder
    {
        public char[][] Build(WordRepeat[] wordRepeats, int size, int randomCount)
        {
            List<List<char>> chain = new List<List<char>>();

            var random = new Random(0);
            for (int i = 0; i < size; i++)
            {
                chain.Add(new List<char>());
                for (int j = 0; j < randomCount; j++)
                {
                    chain[i].Add((char) random.Next(97, 97 + 26));
                }
            }

            foreach (var wordRepeat in wordRepeats)
            {
                var pos = 0;
                for (int j = 0; j < wordRepeat.Times; j++)
                {
                    pos += random.Next(wordRepeat.MinGap, wordRepeat.MaxGap);
                    for (int i = 0; i < wordRepeat.Word.Length; i++)
                    {
                        if (pos >= chain.Count-1) break;
                        chain[pos++].Add(wordRepeat.Word[i]);
                        
                    }

                    if (pos >= chain.Count - 1) break;
                }    
            }
            
            return chain.ConvertAll(list => list.ToArray()).ToArray();
        }
    }
}