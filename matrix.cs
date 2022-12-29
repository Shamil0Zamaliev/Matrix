using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    internal class matrix
    {
        private int m;
        private int n;
        private double[,] mat;
        Random rnd = new Random();


        public int M { get => this.m; }
        public int N { get => this.n; }

        // Функция высшего порядка
        public void MainFunc(Action<int, int> func)
        {
            for (var i = 0; i < this.M; i++)
            {
                for (var j = 0; j < this.N; j++)
                {
                    func(i, j);
                }
            }
        }



        public matrix(int m, int n)
        {
            this.m = m;
            this.n = n;
            this.mat = new double[m, n];
            this.MainFunc((i, j) => this.mat[i, j] = 0);
        }

        public matrix(int n)
        {
            this.m = n;
            this.n = n;
            this.mat = new double[m, n];
            this.MainFunc((i, j) => this.mat[i, j] = 0);
        }

        // можем работать с экземпляром как с массивом
        public double this[int x, int y]
        {
            get
            {
                return this.mat[x, y];
            }
            set
            {
                this.mat[x, y] = value;
            }
        }
        public void AutoFillMatrix()
        {
            this.MainFunc((i, j) => this[i, j] = rnd.Next(100));
        }

        public void FillMatrix()
        {
            for (var i = 0; i < this.M; i++)
            {
                for (var j = 0; j < this.N; j++)
                {
                    Console.Write("a" + (i+1) + (j + 1) + " : ");
                    this[i, j] = double.Parse(Console.ReadLine());
                }
            }
        }
        public matrix Transpose()
        {
            var result = new matrix(this.N, this.M);
            result.MainFunc((i, j) => result[i, j] = this[j, i]);
            return result;
        }

        public void Show()
        {
            for (var i = 0; i < this.M; i++)
            {
                for (var j = 0; j < this.N; j++)
                {
                    Console.Write(String.Format("{0,3}", mat[i, j]));
                }
                Console.WriteLine();
            }
        }

        public void Round()
        {
            this.MainFunc((i, j) => this[i, j] = Math.Round(this[i, j]));
        }
        //перегрузки
        public static matrix operator *(matrix m1, double value)
        {
            var result = new matrix(m1.M, m1.N);
            result.MainFunc((i, j) => result[i, j] = m1[i, j] * value);

            return result;
        }

        public static matrix operator *(matrix m1, matrix m2)
        {
            if (m1.N != m2.M)
            {
                throw new ArgumentException("can not be multiplied");
            }
            var result = new matrix(m1.M, m2.N);
            result.MainFunc((i, j) =>
            {
                for (var k = 0; k < m1.N; k++)
                {
                    result[i, j] += m1[i, k] * m2[k, j];
                }
            });

            return result;
        }

        public static matrix operator /(matrix m1, double value)
        {
            return m1 * Math.Pow(value, -1);
        }

        public static matrix operator +(matrix m1, matrix m2)
        {
            if (m1.M != m2.M || m1.N != m2.N)
            {
                throw new ArgumentException("different dimensions");
            }
            var result = new matrix(m1.M, m1.N);
            result.MainFunc((i, j) => result[i, j] = m1[i, j] + m2[i, j]);

            return result;
        }

        public static matrix operator -(matrix m1, matrix m2)
        {
            return m1 + (m2 * -1);
        }
    }

    internal class squarematrix : matrix
    {

        public squarematrix(int m, int n) : base(m, n) { }
        public squarematrix(int n) : base(n) { }

        public squarematrix Transpose()
        {
            var result = new squarematrix(this.N, this.M);
            result.MainFunc((i, j) => result[i, j] = this[j, i]);
            return result;
        }



        public squarematrix MatrixWithoutRow(int row)
        {
            var result = new squarematrix(this.M - 1, this.N);
            result.MainFunc((i, j) => result[i, j] = i < row ? this[i, j] : this[i + 1, j]);
            return result;
        }

        public squarematrix MatrixWithoutColumn(int column)
        {
            var result = new squarematrix(this.M, this.N - 1);
            result.MainFunc((i, j) => result[i, j] = j < column ? this[i, j] : this[i, j + 1]);
            return result;
        }
        public double Determine()
        {
            if (this.N == 2)
            {
                return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
            }

            double result = 0;
            for (var j = 0; j < this.N; j++)
            {
                result += (j % 2 == 1 ? 1 : -1) * this[1, j] * this.MatrixWithoutColumn(j).MatrixWithoutRow(1).Determine();
            }

            return result;
        }
        public double Minor(int i, int j)
        {
            return MatrixWithoutColumn(j).MatrixWithoutRow(i).Determine();
        }
        public squarematrix Inverse()
        {
            var det = this.Determine();
            if (Math.Abs(det) < Double.Epsilon) return null;
            var result = new squarematrix(M, N);
            MainFunc((i, j) => { result[i, j] = ((i + j) % 2 == 1 ? -1 : 1) * Minor(i, j) / det; });
            result = result.Transpose();
            return result;

        }
        /*
        public squarematrix RemoveLinearDependence()
        {
            var result = new squarematrix(this.M, this.N);
        }
        */

        internal class inversematrix : squarematrix
        {
            public inversematrix(int m, int n) : base(m, n) { }
            public inversematrix(int n) : base(n) { }
        }

    }
}


