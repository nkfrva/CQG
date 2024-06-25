/// <summary>
/// Counting the number of misprints
/// </summary>
/// <param name="pattern">Pattern word (from dictionary) </param>
/// <param name="word">Check word</param>
/// <param name="maxDiff">Max number of errors</param>
/// <returns>Count of misprintsи</returns>
int GetCountOfMisprints(string pattern, string word, int maxDiff = 2)
{
	if (Math.Abs(pattern.Length - word.Length) > maxDiff)
		return 0;

	var diff = 0;
	int i = 0, j = 0;

	while(i < pattern.Length && j < word.Length)
	{
		if (pattern[i] == word[j])
		{
			i++;
			j++;
		}
		// inserting
		else if (j + 1 < word.Length && pattern[i] == word[j + 1])
		{
			diff++;
			i++;
			j += 2;
		}
		// deleting
		else if (i + 1 < pattern.Length && pattern[i + 1] == word[j])
		{
			diff++;
			i += 2;
			j++;
		}
		// inserting + deleting
		else if (i + 1 < pattern.Length && j + 1 < word.Length &&
			pattern[i + 1] == word[j + 1])
		{
			diff += 2;
			i += 2;
			j += 2;
		}
		// lasts elements
		else if (i + 1 == pattern.Length && j + 1 == word.Length &&
			pattern[i] != word[j])
			return diff + 1;
		else
			return 0;

		if (diff > 2) 
			return 0;
	}

	// case, when the last two characters in a row are deleted.
	// example: [t][h][e] -> [][h][]. Or [h][i][s]. 
	if (Math.Abs(pattern.Length - word.Length) == 2 && diff == 0)
		return 0;

	if (pattern.Length != word.Length && 
		pattern.Last() != word.Last())
		diff++;

	return diff;
}

/// <summary>
/// Output formatting. In case there are more than one word, 
/// the output is formatted as { ..., ..., }
/// </summary>
string OutputFormatting(List<string> words) =>
	words.Count == 1 ? words.First() : $"{{{String.Join(' ', words)}}}";


var set = new HashSet<string>();
var line = Console.ReadLine();

while (line != "===")
{
	set.UnionWith(line!
		.Split()
		.ToHashSet());
	line = Console.ReadLine();
}

set.Remove("");
line = Console.ReadLine();

while (line != "===")
{
	var text = line!.Split();
	var result = new List<string>();

	for (var i = 0; i < text.Length; i++)
	{
		var currentWord = text[i];

		if (set.Contains(currentWord))
		{
			result.Add(currentWord);
			continue;
		}

		var minimalErrorWords = new List<string>();
		var minimalError = 2;

		foreach (var pattern in set)
		{
			var diff = GetCountOfMisprints(pattern, currentWord.ToLower(), minimalError);
			if (diff == 1 && minimalError == 2)
				minimalErrorWords.Clear();

			if (diff == 1 || (diff == 2 && minimalError == 2))
				minimalErrorWords.Add(pattern);

			if (diff < minimalError && diff != 0)
				minimalError = diff;
		}

		if (minimalErrorWords.Count == 0)
			result.Add($"{{{currentWord}?}}");
		else
			result.Add(OutputFormatting(minimalErrorWords));
	}

	Console.WriteLine(String.Join(' ', result));
	line = Console.ReadLine();
}
