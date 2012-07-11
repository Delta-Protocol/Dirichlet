﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Decompose.Numerics
{
    public static partial class IntegerMath
    {
        private static IFactorizationAlgorithm<int> factorerInt = new TrialDivisionFactorization();

        public static bool IsSquareFree(int n)
        {
            return Abs(Mobius(n)) == 1;
        }

        public static bool IsSquareFree(BigInteger n)
        {
            return Abs(Mobius(n)) == 1;
        }

        public static int LittleOmega(int n)
        {
            var factors = factorerInt.Factor(n).ToArray();
            return factors
                .OrderBy(factor => factor)
                .GroupBy(factor => factor)
                .Count();
        }

        public static BigInteger LittleOmega(BigInteger n)
        {
            if (n < int.MaxValue)
                return LittleOmega((int)n);
            throw new NotImplementedException();
        }

        public static int BigOmega(int n)
        {
            return factorerInt.Factor(n).Count();
        }

        public static BigInteger BigOmega(BigInteger n)
        {
            if (n < int.MaxValue)
                return BigOmega((int)n);
            throw new NotImplementedException();
        }

        public static int Liouville(int n)
        {
            return BigOmega(n) % 2 == 0 ? 1 : -1;
        }

        public static BigInteger Liouville(BigInteger n)
        {
            if (n < int.MaxValue)
                return Liouville((int)n);
            throw new NotImplementedException();
        }

        public static int Mobius(int n)
        {
            var factors = factorerInt.Factor(n).ToArray();
            bool squareFree = factors
                .OrderBy(factor => factor)
                .GroupBy(factor => factor)
                .All(grouping => grouping.Count() < 2);
            if (!squareFree)
                return 0;
            return factors.Length % 2 == 0 ? 1 : -1;
        }

        public static int Mobius(BigInteger n)
        {
            if (n < int.MaxValue)
                return Mobius((int)n);
            throw new NotImplementedException();
        }

        public static int Mertens(int n)
        {
            var sum = 0;
            for (var i = 1; i <= n; i++)
                sum += Mobius(i);
            return sum;
        }

        public static int MertensOdd(int n)
        {
            var sum = 0;
            for (var i = 1; i <= n; i += 2)
                sum += Mobius(i);
            return sum;
        }

        public static BigInteger Mertens(BigInteger n)
        {
            if (n < int.MaxValue)
                return Mertens((int)n);
            throw new NotImplementedException();
        }

        public static int NumberOfDivisors(int n)
        {
            return factorerInt.Factor(n)
                .GroupBy(p => p)
                .Select(grouping => grouping.Count() + 1)
                .Product();
        }

        public static int NumberOfDivisors(BigInteger n)
        {
            if (n < int.MaxValue)
                return NumberOfDivisors((int)n);
            throw new NotImplementedException();
        }

        public static int NumberOfDivisors(int n, int i)
        {
            if (i == 0)
                return 0;
            if (i == 1)
                return 1;
            return factorerInt.Factor(n)
                .GroupBy(p => p)
                .Select(grouping => grouping.Count())
                .Select(k => Binomial(k + i - 1, k))
                .Product();
        }

        public static BigInteger NumberOfDivisors(BigInteger n, int i)
        {
            if (n < int.MaxValue)
                return NumberOfDivisors((int)n, i);
            throw new NotImplementedException();
        }

        public static int SumOfNumberOfDivisors(int n)
        {
            return SumOfNumberOfDivisors(n, 2);
        }

        public static BigInteger SumOfNumberOfDivisors(BigInteger n)
        {
            return SumOfNumberOfDivisors(n, 2);
        }

        public static int SumOfNumberOfDivisors(int n, int i)
        {
            var sum = (int)0;
            for (var j = (int)1; j <= n; j++)
                sum += NumberOfDivisors(j, i);
            return sum;
        }

        public static BigInteger SumOfNumberOfDivisors(BigInteger n, int i)
        {
            var sum = (BigInteger)0;
            for (var j = (BigInteger)1; j <= n; j++)
                sum += NumberOfDivisors(j, i);
            return sum;
        }

        public static int PrimeCountingFunction(int n)
        {
            return primes.TakeWhile(p => p <= n).Count();
        }

        public static BigInteger PrimeCountingFunction(BigInteger n)
        {
            if (n < int.MaxValue)
                return PrimeCountingFunction((int)n);
            throw new NotImplementedException();
        }
    }
}
