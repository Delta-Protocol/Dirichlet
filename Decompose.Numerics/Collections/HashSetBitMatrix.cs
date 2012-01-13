﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Decompose.Numerics
{
    public class HashSetBitMatrix : IBitMatrix
    {
        private int rows;
        private int cols;

        private HashSet<int>[] rowSets;

        public int WordLength
        {
            get { return 1; }
        }

        public int Rows
        {
            get { return rows; }
        }

        public int Cols
        {
            get { return cols; }
        }

        public HashSetBitMatrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            rowSets = new HashSet<int>[rows];
            for (int i = 0; i < rows; i++)
                rowSets[i] = new HashSet<int>();
        }

        public bool this[int row, int col]
        {
            get { return rowSets[row].Contains(col); }
            set
            {
                if (value)
                    rowSets[row].Add(col);
                else
                    rowSets[row].Remove(col);
            }
        }

        public void XorRows(int dst, int src, int col)
        {
            rowSets[dst].SymmetricExceptWith(rowSets[src]);
        }

        public void Clear()
        {
            for (int i = 0; i < rows; i++)
                rowSets[i].Clear();
        }

        public void CopySubMatrix(IBitMatrix other, int row, int col)
        {
            BitMatrixHelper.CopySubMatrix(this, other, row, col);
        }

        public IEnumerable<bool> GetRow(int row)
        {
            return BitMatrixHelper.GetRow(this, row);
        }

        public IEnumerable<int> GetNonZeroCols(int row)
        {
            return rowSets[row];
        }

        public IEnumerable<bool> GetCol(int col)
        {
            return BitMatrixHelper.GetCol(this, col);
        }

        public IEnumerable<int> GetNonZeroRows(int col)
        {
            return BitMatrixHelper.GetNonZeroRows(this, col);
        }

        public int GetRowWeight(int row)
        {
            return rowSets[row].Count;
        }

        public int GetColWeight(int col)
        {
            return BitMatrixHelper.GetColWeight(this, col);
        }

        public IEnumerable<int> GetRowWeights()
        {
            return BitMatrixHelper.GetRowWeights(this);
        }

        public IEnumerable<int> GetColWeights()
        {
            var weights = new int[cols];
            for (int row = 0; row < rows; row++)
            {
                foreach (var col in rowSets[row])
                    ++weights[col];
            }
            return weights;
        }
    }
}
