﻿using System.Collections.Generic;

namespace Decompose.Numerics
{
    public interface IBitMatrix : IMatrix<bool>
    {
        int WordLength { get; }
        void XorRows(int dst, int src, int col);
        void Clear();
        void CopySubMatrix(IBitMatrix other, int row, int col);
        IEnumerable<bool> GetRow(int row);
        IEnumerable<int> GetNonZeroCols(int row);
        IEnumerable<bool> GetCol(int col);
        IEnumerable<int> GetNonZeroRows(int col);
        int GetRowWeight(int row);
        int GetColWeight(int col);
        IEnumerable<int> GetRowWeights();
        IEnumerable<int> GetColWeights();
    }
}
