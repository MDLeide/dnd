using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM
{
    public class LorumIpsumGenerator
    {
        const string DefaultText =
            @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus molestie, nisl ac pretium venenatis, sem lacus cursus velit, a pretium urna nisl quis diam. Pellentesque faucibus posuere quam in volutpat. Etiam eu tempor orci. In quis diam nisi. Fusce pharetra, urna sed luctus ultricies, lacus augue tincidunt libero, eu porta libero felis id mauris. Vivamus vitae nisi vehicula, dapibus purus ut, fringilla urna. Etiam mi lectus, aliquet eu cursus in, rutrum ac justo. Ut pharetra dolor et elit imperdiet, ac blandit nunc laoreet. Etiam id dolor odio. Quisque et elit eget nisi pellentesque elementum. Quisque pharetra odio ligula, eget venenatis metus bibendum vel. Donec tincidunt nulla velit.

Nullam egestas nisl pharetra tincidunt euismod. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Etiam interdum vehicula velit ut tempus. Suspendisse suscipit ac enim vel elementum. Sed quis justo nisi. Nunc fringilla accumsan eros at venenatis. Curabitur mollis, est ac euismod interdum, arcu neque commodo neque, in rhoncus diam orci at neque. Sed velit turpis, congue eget faucibus sit amet, dictum a turpis. Vivamus venenatis nulla dictum nisi luctus, at pulvinar elit pulvinar. Quisque eget condimentum magna. Aliquam pretium eleifend tempor. In dictum nibh dui, eu pharetra dui sagittis at.

Aenean eu congue velit. Integer sit amet vulputate justo, in iaculis odio. In hac habitasse platea dictumst. Morbi ac aliquam nisl, viverra ullamcorper erat. Mauris porta orci tortor, nec bibendum urna ullamcorper sit amet. Donec aliquam elit non nunc ullamcorper egestas. Morbi nulla odio, egestas ac quam id, varius viverra augue. Pellentesque ultrices in nunc id gravida. Cras ut sagittis dui. Suspendisse sed egestas tellus, et semper risus. Aenean mollis imperdiet erat, eu eleifend metus lacinia nec. Donec ultrices quis sapien sed convallis. Maecenas laoreet enim sit amet fermentum feugiat. Suspendisse tristique malesuada ligula, id euismod metus. Vestibulum elementum posuere nisi vel volutpat. Proin quis nulla tincidunt neque luctus tempor.

Integer ultricies lorem lorem, in iaculis odio convallis id. Morbi facilisis dignissim nisl ut laoreet. Praesent laoreet vulputate ligula et eleifend. Pellentesque blandit fringilla sem, sodales faucibus lectus luctus nec. Mauris auctor augue id ipsum rhoncus cursus. Mauris nec metus tincidunt, maximus orci sed, luctus nulla. In hac habitasse platea dictumst. Nunc sed leo nulla. Suspendisse magna velit, vulputate in tortor vel, auctor tincidunt turpis. Vivamus consectetur vehicula lectus sagittis vehicula. Ut non nibh id justo ornare laoreet. Etiam posuere auctor lacus, vitae mollis mi tincidunt ut. Quisque sit amet euismod lorem, et consectetur mi. Sed molestie velit tortor, id dictum purus maximus eu. Suspendisse maximus purus nulla, nec pellentesque metus lobortis sed. Vestibulum eget tortor vitae est vehicula gravida.

Proin tincidunt vestibulum felis, vitae auctor lorem consequat quis. Aenean pulvinar eget purus semper fermentum. Integer tincidunt dapibus elit sed maximus. Praesent vel lectus non dolor consequat laoreet et non lorem. Donec pretium justo magna, at elementum magna vestibulum et. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Phasellus sollicitudin, nulla eu convallis vulputate, libero mauris imperdiet dui, elementum pulvinar dolor metus nec tellus. Vestibulum in facilisis neque.";

        readonly Random _rand;

        int _minLength = int.MaxValue;
        int _maxLength = 0;
        string[] _words;
        Dictionary<int, string[]> _wordsByLength;

        int _minWords = int.MaxValue;
        int _maxWords = 0;
        string[] _sentences;
        Dictionary<int, string[]> _sentencesByWordCount;

        int _minSentences = int.MaxValue;
        int _maxSentences = 0;
        string[] _paragraphs;
        Dictionary<int, string[]> _paragraphsBySentenceCount;


        public LorumIpsumGenerator(string textToSample = null, int? seed = null)
        {
            if (seed.HasValue)
                _rand = new Random(seed.Value);
            else
                _rand = new Random();

            if (!string.IsNullOrEmpty(textToSample))
                TextToSample = textToSample;
            else
                TextToSample = DefaultText;
        }


        string _textToSample;

        public string TextToSample
        {
            get => _textToSample;
            set
            {
                _textToSample = value;
                OnTextToSampleChanged(value);
            }
        }

        void OnTextToSampleChanged(string newValue)
        {
            _words = newValue
                .Replace(".", string.Empty)
                .Replace(",", string.Empty)
                .Replace(Environment.NewLine, string.Empty)
                .Split(' ')
                .Select(p => p.ToLower())
                .Distinct()
                .ToArray();

            var wordsByLength = new Dictionary<int, List<string>>();
            foreach (var word in _words)
            {
                if (word.Length < _minLength)
                    _minLength = word.Length;
                if (word.Length > _maxLength)
                    _maxLength = word.Length;

                if (!wordsByLength.ContainsKey(word.Length))
                    wordsByLength.Add(word.Length, new List<string>());
                wordsByLength[word.Length].Add(word);
            }

            _wordsByLength = wordsByLength.ToDictionary(p => p.Key, p => p.Value.ToArray());

            _sentences = newValue.Split('.').Select(p => p.Replace(Environment.NewLine, string.Empty).Trim() + ".")
                .ToArray();
            var sentencesByWordCount = new Dictionary<int, List<string>>();
            foreach (var sentence in _sentences)
            {
                var wordCount = sentence.Count(p => p == ' ') + 1;
                if (wordCount < _minWords)
                    _minWords = wordCount;
                if (wordCount > _maxWords)
                    _maxWords = wordCount;

                if (!sentencesByWordCount.ContainsKey(wordCount))
                    sentencesByWordCount.Add(wordCount, new List<string>());
                sentencesByWordCount[wordCount].Add(sentence);
            }

            _sentencesByWordCount = sentencesByWordCount.ToDictionary(p => p.Key, p => p.Value.ToArray());

            _paragraphs = newValue.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim()).ToArray();
            var paragraphsBySentenceCount = new Dictionary<int, List<string>>();
            foreach (var para in _paragraphs)
            {
                var sentenceCount = para.Count(p => p == '.');
                if (sentenceCount < _minSentences)
                    _minSentences = sentenceCount;
                if (sentenceCount > _maxSentences)
                    _maxSentences = sentenceCount;

                if (!paragraphsBySentenceCount.ContainsKey(sentenceCount))
                    paragraphsBySentenceCount.Add(sentenceCount, new List<string>());
                paragraphsBySentenceCount[sentenceCount].Add(para);
            }

            _paragraphsBySentenceCount = paragraphsBySentenceCount.ToDictionary(p => p.Key, p => p.Value.ToArray());
        }

        /// <summary>
        /// Gets a random word from the word pool.
        /// </summary>
        /// <param name="capitalize"></param>
        /// <returns></returns>
        public string GetRandomWord(bool capitalize = false)
            => GetRandomWord(0, capitalize);

        /// <summary>
        /// Gets a random word from the word pool.
        /// </summary>
        /// <param name="minLength">Inclusive</param>
        /// <param name="capitalize"></param>
        /// <returns></returns>
        public string GetRandomWord(int minLength, bool capitalize = false)
            => GetRandomWord(minLength, int.MaxValue, capitalize);

        /// <summary>
        /// Gets a random word from the word pool.
        /// </summary>
        /// <param name="minLength">Inclusive</param>
        /// <param name="maxLength">Exclusive</param>
        /// <param name="capitalize"></param>
        /// <returns></returns>
        public string GetRandomWord(int minLength, int maxLength, bool capitalize = false)
        {
            if (minLength >= maxLength || minLength < 1)
                throw new ArgumentOutOfRangeException();

            minLength = Math.Max(minLength, _minLength);
            maxLength = Math.Min(maxLength, _maxLength + 1);

            var tried = new List<int>();
            while (true)
            {
                var len = _rand.Next(minLength, maxLength);
                if (tried.Contains(len))
                    continue;

                tried.Add(len);
                if (_wordsByLength.ContainsKey(len))
                {
                    if (capitalize)
                        return Capitalize(_wordsByLength[len][_rand.Next(0, _wordsByLength[len].Length)]);

                    return _wordsByLength[len][_rand.Next(0, _wordsByLength[len].Length)];
                }

                if (tried.Count >= maxLength - minLength)
                    return null;
            }
        }

        /// <summary>
        /// Gets an array of random words from the word pool.
        /// </summary>
        /// <param name="minWordCount"></param>
        /// <param name="maxWordCount"></param>
        /// <param name="capitalize"></param>
        /// <param name="minWordLength"></param>
        /// <param name="maxWordLength"></param>
        /// <returns></returns>
        public string[] GetRandomWords(
            int minWordCount = 1,
            int maxWordCount = 2,
            bool capitalize = false,
            int minWordLength = 0,
            int maxWordLength = int.MaxValue)
        {
            if (minWordCount >= maxWordCount || minWordCount < 1)
                throw new ArgumentOutOfRangeException();

            if (minWordLength >= maxWordLength || minWordLength < 1)
                throw new ArgumentOutOfRangeException();

            var wordCount = _rand.Next(minWordCount, maxWordCount);
            var words = new string[wordCount];

            while (wordCount > 0)
            {
                var word = GetRandomWord(minWordLength, maxWordLength, capitalize);
                if (word == null)
                    return words.Take(words.Length - wordCount).ToArray();

                words[words.Length - wordCount] = word;
                --wordCount;
            }

            return words;
        }

        /// <summary>
        /// Constructs a sentence by choosing random words from the word pool.
        /// </summary>
        /// <param name="minWordCount"></param>
        /// <param name="maxWordCount"></param>
        /// <param name="capitalizeFirst"></param>
        /// <param name="capitalizeAll"></param>
        /// <param name="minWordLength"></param>
        /// <param name="maxWordLength"></param>
        /// <param name="endingString"></param>
        /// <returns></returns>
        public string ConstructRandomSentence(
            int minWordCount = 1,
            int maxWordCount = 2,
            bool capitalizeFirst = true,
            bool capitalizeAll = false,
            int minWordLength = 1,
            int maxWordLength = int.MaxValue,
            string endingString = ".")
        {
            if (minWordCount >= maxWordCount || minWordCount < 1)
                throw new ArgumentOutOfRangeException();

            if (minWordLength >= maxWordLength || minWordLength < 1)
                throw new ArgumentOutOfRangeException();

            var words = GetRandomWords(
                minWordCount,
                maxWordCount,
                capitalizeAll,
                minWordLength,
                maxWordLength);

            if (capitalizeFirst && !capitalizeAll && words.Length > 0)
                words[0] = Capitalize(words[0]);

            return string.Join(" ", words) + (string.IsNullOrEmpty(endingString) ? string.Empty : endingString);
        }

        /// <summary>
        /// Gets a random sentence from the sentence pool.
        /// </summary>
        /// <param name="minWords"></param>
        /// <param name="maxWords"></param>
        /// <returns></returns>
        public string GetRandomSentence(
            int minWords = 1,
            int maxWords = int.MaxValue)
        {
            if (minWords >= maxWords || minWords < 1)
                throw new ArgumentOutOfRangeException();

            minWords = Math.Max(minWords, _minWords);
            maxWords = Math.Min(maxWords, _maxWords + 1);

            var tried = new List<int>();
            while (true)
            {
                var wordCount = _rand.Next(minWords, maxWords);
                if (tried.Contains(wordCount))
                    continue;

                tried.Add(wordCount);
                if (_sentencesByWordCount.ContainsKey(wordCount))
                    return _sentencesByWordCount[wordCount][_rand.Next(0, _sentencesByWordCount[wordCount].Length)];

                if (tried.Count >= maxWords - minWords)
                    return null;
            }
        }

        /// <summary>
        /// Constructs a paragraph by choosing random sentences from the sentence pool.
        /// </summary>
        /// <param name="minSentenceCount"></param>
        /// <param name="maxSentenceCount"></param>
        /// <param name="minSentenceWordCount"></param>
        /// <param name="maxSentenceWordCount"></param>
        /// <returns></returns>
        public string ConstructRandomParagraphFromSentences(
            int minSentenceCount = 1,
            int maxSentenceCount = 7,
            int minSentenceWordCount = 1,
            int maxSentenceWordCount = int.MaxValue)
        {
            if (minSentenceCount >= maxSentenceCount || minSentenceCount < 1)
                throw new ArgumentOutOfRangeException();

            if (minSentenceWordCount >= maxSentenceWordCount || minSentenceWordCount < 1)
                throw new ArgumentOutOfRangeException();

            minSentenceWordCount = Math.Max(minSentenceWordCount, _minWords);
            maxSentenceWordCount = Math.Min(maxSentenceWordCount, _maxWords + 1);

            var sentenceCount = _rand.Next(minSentenceCount, maxSentenceCount);
            var sentences = new List<string>();
            while (sentenceCount > 0)
            {
                var sentence = GetRandomSentence(minSentenceWordCount, maxSentenceWordCount);
                if (sentence == null)
                    break;

                sentences.Add(sentence);
                --sentenceCount;
            }

            return string.Join(" ", sentences);
        }

        /// <summary>
        /// Constructs a paragraph by constructing sentences by choosing random words from the word pool.
        /// </summary>
        /// <param name="minSentenceCount"></param>
        /// <param name="maxSentenceCount"></param>
        /// <param name="minSentenceWordCount"></param>
        /// <param name="maxSentenceWordCount"></param>
        /// <param name="indent"></param>
        /// <returns></returns>
        public string ConstructRandomParagraphFromWords(
            int minSentenceCount = 1,
            int maxSentenceCount = 7,
            int minSentenceWordCount = 1,
            int maxSentenceWordCount = 20,
            bool indent = true)
        {
            if (minSentenceCount >= maxSentenceWordCount || minSentenceCount < 1)
                throw new ArgumentOutOfRangeException();

            if (minSentenceWordCount >= maxSentenceWordCount || minSentenceWordCount < 1)
                throw new ArgumentOutOfRangeException();

            var sentenceCount = _rand.Next(minSentenceCount, maxSentenceCount);
            var sentences = new List<string>();

            while (sentenceCount > 0)
            {
                var sentence = ConstructRandomSentence(minSentenceWordCount, maxSentenceWordCount);
                if (sentence == null)
                    break;
                sentences.Add(sentence);
                --sentenceCount;
            }

            if (indent)
                return $"\t{string.Join(" ", sentences)}";

            return string.Join(" ", sentences);
        }

        /// <summary>
        /// Gets a random paragraph from the paragraph pool.
        /// </summary>
        /// <param name="minSentences"></param>
        /// <param name="maxSentences"></param>
        /// <returns></returns>
        public string GetRandomParagraph(
            int minSentences = 1,
            int maxSentences = int.MaxValue)
        {
            if (minSentences >= maxSentences || minSentences < 1)
                throw new ArgumentOutOfRangeException();

            minSentences = Math.Max(minSentences, _minSentences);
            maxSentences = Math.Min(maxSentences, _maxSentences);

            var tried = new List<int>();
            while (true)
            {
                var sentences = _rand.Next(minSentences, maxSentences);
                if (tried.Contains(sentences))
                    continue;

                tried.Add(sentences);
                if (_paragraphsBySentenceCount.ContainsKey(sentences))
                    return _paragraphsBySentenceCount[sentences][_rand.Next(0, _paragraphsBySentenceCount[sentences].Length)];

                if (tried.Count >= maxSentences - minSentences)
                    return null;
            }
        }

        /// <summary>
        /// Constructs a passage by constructing paragraphs by constructing sentences by choosing random words from the word pool.
        /// </summary>
        /// <param name="minSentenceWordCount"></param>
        /// <param name="maxSentenceWordCount"></param>
        /// <param name="minParagraphSentenceCount"></param>
        /// <param name="maxParagraphSentenceCount"></param>
        /// <param name="minParagraphCount"></param>
        /// <param name="maxParagraphCount"></param>
        /// <param name="paragraphSeparator"></param>
        /// <param name="indent"></param>
        /// <returns></returns>
        public string ConstructRandomPassageFromWords(
            int minSentenceWordCount = 1,
            int maxSentenceWordCount = 20,
            int minParagraphSentenceCount = 1,
            int maxParagraphSentenceCount = int.MaxValue,
            int minParagraphCount = 1,
            int maxParagraphCount = 7,
            string paragraphSeparator = "\r\n\r\n",
            bool indent = true)
        {
            if (minSentenceWordCount >= maxSentenceWordCount || minSentenceWordCount < 1)
                throw new ArgumentOutOfRangeException();

            if (minParagraphSentenceCount >= maxParagraphSentenceCount || minParagraphSentenceCount < 1)
                throw new ArgumentOutOfRangeException();

            if (minParagraphCount >= maxParagraphCount || minParagraphCount < 1)
                throw new ArgumentOutOfRangeException();

            var paragraphCount = _rand.Next(minParagraphCount, maxParagraphCount);
            var paragraphs = new List<string>();
            while (paragraphCount > 0)
            {
                paragraphs.Add(
                    ConstructRandomParagraphFromWords(
                        minParagraphSentenceCount, 
                        maxParagraphSentenceCount,
                        minSentenceWordCount,
                        maxSentenceWordCount,
                        indent));
                --paragraphCount;
            }

            return string.Join(paragraphSeparator, paragraphs);
        }

        static string Capitalize(string word)
        {
            if (word.Length > 1)
                return $"{word.Substring(0, 1).ToUpper()}{word.Substring(1)}";

            return word.ToUpper();
        }
    }
}
