using System;

public static class StringSimilarity
{
    // Calculate the Levenshtein Distance between two strings (a and b)
    private static int LevenshteinDistance(string a, string b)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        if (b == null) throw new ArgumentNullException(nameof(b));

        int[,] matrix = new int[a.Length + 1, b.Length + 1];

        // Initialize the first row and column
        for (int i = 0; i <= a.Length; i++) matrix[i, 0] = i;
        for (int j = 0; j <= b.Length; j++) matrix[0, j] = j;

        // Calculate distances
        for (int i = 1; i <= a.Length; i++)
        {
            for (int j = 1; j <= b.Length; j++)
            {
                int cost = (b[j - 1] == a[i - 1]) ? 0 : 1;
                matrix[i, j] = Math.Min(Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1), matrix[i - 1, j - 1] + cost);
            }
        }

        return matrix[a.Length, b.Length];
    }

    // Calculate similarity percentage based on Levenshtein Distance
    public static double CalculateSimilarity(string a, string b)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));
        if (b == null) throw new ArgumentNullException(nameof(b));
        if (a.Length == 0 || b.Length == 0) return 0.0;

        int distance = LevenshteinDistance(a, b);
        int maxLength = Math.Max(a.Length, b.Length);

        return (1.0 - (double)distance / maxLength) * 100.0;
    }
}
