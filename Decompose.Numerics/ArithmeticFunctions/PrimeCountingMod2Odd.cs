﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;

namespace Decompose.Numerics
{
    public class PrimeCountingMod2Odd
    {
        private int threads;
        private MobiusCollection mobius;
        private DivisionFreeDivisorSummatoryFunction[] hyperbolicSum;

        public PrimeCountingMod2Odd(int threads)
        {
            this.threads = threads;
            var count = Math.Max(threads, 1);
            hyperbolicSum = new DivisionFreeDivisorSummatoryFunction[count];
            for (var i = 0; i < count; i++)
                hyperbolicSum[i] = new DivisionFreeDivisorSummatoryFunction(0, false, true);
        }

        public int Evaluate(BigInteger n)
        {
            var jmax = IntegerMath.FloorLog(n, 2);
            var dmax = IntegerMath.FloorRoot(n, 2);
            mobius = new MobiusCollection((int)(IntegerMath.Max(jmax, dmax) + 1), 0);
            return Pi2(n);
        }

        private int Pi2(BigInteger n)
        {
            var kmax = (int)IntegerMath.FloorLog(n, 2);
            var sum = 0;
            for (var k = 1; k <= kmax; k++)
            {
                if (mobius[k] != 0)
                    sum += mobius[k] * F2(IntegerMath.FloorRoot(n, k));
            }
            return (sum + (n >= 2 ? 1 : 0)) % 2;
        }

        private int F2(BigInteger n)
        {
#if true
            var xmax = ((long)IntegerMath.FloorSquareRoot(n) - 1) | 1;
#else
            var xmax = ((long)IntegerMath.FloorPower(n, 2, 7) - 1) | 1;
#endif
            var s = 0;
            var x = xmax;
            var beta = (long)(n / (x + 2));
            var eps = (long)(n % (x + 2));
            var delta = (long)(n / x - (ulong)beta);
            var gamma = 2 * beta - x * delta;
            var alpha = beta / (x + 2);
            var alphax = (alpha + 1) * (x + 2);
            var lastalpha = (long)-1;
            var count = 0;
            while (x >= 1)
            {
                eps += gamma;
                if (eps >= x)
                {
                    ++delta;
                    gamma -= x;
                    eps -= x;
                    if (eps >= x)
                    {
                        ++delta;
                        gamma -= x;
                        eps -= x;
                        if (eps >= x)
                            break;
                    }
                }
                else if (eps < 0)
                {
                    --delta;
                    gamma += x;
                    eps += x;
                }
                gamma += 4 * delta;
                beta += delta;
                alphax -= 2 * alpha + 2;
                if (alphax <= beta)
                {
                    ++alpha;
                    alphax += x;
                    if (alphax <= beta)
                    {
                        ++alpha;
                        alphax += x;
                        if (alphax <= beta)
                            break;
                    }
                }

                Debug.Assert(eps == n % x);
                Debug.Assert(beta == n / x);
                Debug.Assert(delta == beta - n / (x + 2));
                Debug.Assert(gamma == 2 * beta - (BigInteger)(x - 2) * delta);
                Debug.Assert(alpha == n / ((BigInteger)x * x));

                var mu = mobius[(int)x];
                if (mu != 0)
                {
                    if (alpha != lastalpha)
                    {
                        count &= 3;
                        if (count != 0)
                        {
                            s += count * T2(lastalpha);
                            count = 0;
                        }
                        lastalpha = alpha;
                    }
                    count += mu;
                }
                x -= 2;
            }
            {
                count &= 3;
                if (count != 0)
                    s += count * T2(lastalpha);
            }
            var xx = (ulong)x * (ulong)x;
            var dx = 4 * (ulong)x - 4;
            while (x >= 1)
            {
                var mu = mobius[(int)x];
                Debug.Assert(xx == (ulong)x * (ulong)x);
                if (mu != 0)
                {
                    var term = T2(n / xx);
                    if (mu == 1)
                        s += term;
                    else
                        s += 4 - term;
                }
                xx -= dx;
                dx -= 8;
                x -= 2;
            }
            Debug.Assert((s - 1) % 2 == 0);
            return (s - 1) / 2;
        }

        public int T2(BigInteger n)
        {
            var sqrt = (long)IntegerMath.FloorSquareRoot(n);
            var result = 2 * S1(n, 1, sqrt) + 3 * (int)(((sqrt + (sqrt & 1)) >> 1) & 1);
            Debug.Assert(result % 4 == new DivisionFreeDivisorSummatoryFunction(0, false, true).Evaluate(n) % 4);
            return result;
        }

        private int S1(BigInteger n, long x1, long x2)
        {
            if (n <= long.MaxValue)
                return S1((long)n, (int)x1, (int)x2);

            var s = (long)0;
            var x = (x2 - 1) | 1;
            var beta = (long)(n / (x + 2));
            var eps = (long)(n % (x + 2));
            var delta = (long)(n / x - beta);
            var gamma = 2 * beta - x * delta;
            while (x >= x1)
            {
                eps += gamma;
                if (eps >= x)
                {
                    ++delta;
                    gamma -= x;
                    eps -= x;
                    if (eps >= x)
                    {
                        ++delta;
                        gamma -= x;
                        eps -= x;
                        if (eps >= x)
                            break;
                    }
                }
                else if (eps < 0)
                {
                    --delta;
                    gamma += x;
                    eps += x;
                }
                gamma += 4 * delta;
                beta += delta;
                beta &= 3;

                Debug.Assert(eps == n % x);
                Debug.Assert(beta == ((n / x) & 3));
                Debug.Assert(delta == (n / x) - n / (x + 2));
                Debug.Assert(gamma == 2 * (n / x) - (BigInteger)(x - 2) * delta);

                s += beta + (beta & 1);
                x -= 2;
            }
            var nRep = (UInt128)n;
            while (x >= x1)
            {
                beta = (long)((nRep / (ulong)x) & 3);
                s += beta + (beta & 1);
                x -= 2;
            }
            return (int)((s >> 1) & 1);
        }

        private int S1(long n, int x1, int x2)
        {
            var s = (int)0;
            var x = (x2 - 1) | 1;
            var beta = (int)(n / (x + 2));
            var eps = (int)(n % (x + 2));
            var delta = (int)(n / x - beta);
            var gamma = 2 * beta - x * delta;
            while (x >= x1)
            {
                eps += gamma;
                if (eps >= x)
                {
                    ++delta;
                    gamma -= x;
                    eps -= x;
                    if (eps >= x)
                    {
                        ++delta;
                        gamma -= x;
                        eps -= x;
                        if (eps >= x)
                            break;
                    }
                }
                else if (eps < 0)
                {
                    --delta;
                    gamma += x;
                    eps += x;
                }
                gamma += 4 * delta;
                beta += delta;
                beta &= 3;

                Debug.Assert(eps == n % x);
                Debug.Assert(beta == ((n / x) & 3));
                Debug.Assert(delta == (n / x) - n / (x + 2));
                Debug.Assert(gamma == 2 * (n / x) - (BigInteger)(x - 2) * delta);

                s += beta + (beta & 1);
                x -= 2;
            }
            while (x >= x1)
            {
                beta = (int)((n / x) & 3);
                s += beta + (beta & 1);
                x -= 2;
            }
            return (int)((s >> 1) & 1);
        }
    }
}