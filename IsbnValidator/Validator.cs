using System;

namespace IsbnValidator
{
    public static class Validator
    {
        /// <summary>
        /// Returns true if the specified <paramref name="isbn"/> is valid; returns false otherwise.
        /// </summary>
        /// <param name="isbn">The string representation of 10-digit ISBN.</param>
        /// <returns>true if the specified <paramref name="isbn"/> is valid; false otherwise.</returns>
        /// <exception cref="ArgumentException"><paramref name="isbn"/> is empty or has only white-space characters.</exception>
        public static bool IsIsbnValid(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentException("The ISBN cannot be empty or contain only white-space characters.", nameof(isbn));
            }

            if (!IsIsbnValidation(isbn))
            {
                return false;
            }

            isbn = isbn.Replace("-", string.Empty, StringComparison.InvariantCulture).Replace(" ", string.Empty, StringComparison.InvariantCulture);

            if (isbn.Length != 10)
            {
                return false;
            }

            int checksum = 0;
            for (int i = 0; i < 10; i++)
            {
                if (!int.TryParse(isbn[i].ToString(), out int digit))
                {
                    if (i == 9 && isbn[i] == 'X')
                    {
                        digit = 10;
                    }
                    else
                    {
                        return false;
                    }
                }

                checksum += digit * (10 - i);
            }

            return checksum % 11 == 0;
        }

        private static bool IsIsbnValidation(string isbn)
        {
            bool checkPrevious = true;
            int countMinus = 0;
            foreach (var l in isbn)
            {
                bool checkCurrent;
                if (l == '-')
                {
                    countMinus++;
                    checkCurrent = false;
                }
                else
                {
                    checkCurrent = true;
                }

                if (countMinus > 3)
                {
                    return false;
                }

                if (!checkCurrent && !checkPrevious)
                {
                    return false;
                }

                checkPrevious = checkCurrent;
            }

            int count = isbn.Count(u => char.IsDigit(u));

            if (count < 9 || count > 10)
            {
                return false;
            }

            int checkLetter = isbn.Count(h => char.IsLetter(h));

            if ((count == 9 && checkLetter == 0) || checkLetter > 1)
            {
                return false;
            }

            return true;
        }
    }
}
