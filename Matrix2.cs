using Microsoft.Xna.Framework;
using System;

namespace MonoPlayground
{
    internal struct Matrix2
    {
        public const int Width = 2;
        public const int Height = 2;
        public float[,] Values;
        public static Matrix2 Zero = new Matrix2(0, 0, 0, 0);
        public Matrix2(float r0c0, float r0c1, float r1c0, float r1c1)
        {
            Values = new float[Width, Height];
            Values[0, 0] = r0c0;
            Values[0, 1] = r0c1;
            Values[1, 0] = r1c0;
            Values[1, 1] = r1c1;
        }
        public Matrix2(Vector2 row0, Vector2 row1)
        {
            Values = new float[Width, Height];
            Values[0, 0] = row0.X;
            Values[0, 1] = row0.Y;
            Values[1, 0] = row1.X;
            Values[1, 1] = row1.Y;
        }
        public float this[int row, int col] { get => Values[row, col]; set => Values[row, col] = value; }
        public float Determinant() => Values[0, 0] * Values[1, 1] - Values[0, 1] * Values[1, 0];
        public static Matrix2 Inverse(Matrix2 matrix)
        {
            float determinant = matrix.Determinant();
            if (determinant == 0)
                throw new DivideByZeroException();
            Matrix2 inverse = new Matrix2(
                r0c0: +matrix[1, 1],
                r0c1: -matrix[0, 1],
                r1c0: -matrix[1, 0],
                r1c1: +matrix[0, 0]) * (1 / determinant);
            return inverse;
        }
        public static Matrix2 operator +(Matrix2 matrix) => matrix;
        public static Matrix2 operator -(Matrix2 matrix) => new Matrix2(
            r0c0: -matrix[0, 0],
            r0c1: -matrix[0, 1],
            r1c0: -matrix[1, 0],
            r1c1: -matrix[1, 1]);
        public static Matrix2 operator +(Matrix2 matrix0, Matrix2 matrix1) => new Matrix2(
            r0c0: matrix0[0, 0] + matrix1[0, 0],
            r0c1: matrix0[0, 1] + matrix1[0, 1],
            r1c0: matrix0[1, 0] + matrix1[1, 0],
            r1c1: matrix0[1, 1] + matrix1[1, 1]);
        public static Matrix2 operator -(Matrix2 matrix0, Matrix2 matrix1) => matrix0 + (-matrix1);
        public static Matrix2 operator *(Matrix2 matrix0, Matrix2 matrix1) => new Matrix2(
            r0c0: matrix0[0, 0] * matrix1[0, 0] + matrix0[0, 1] * matrix1[1, 0],
            r0c1: matrix0[0, 0] * matrix1[0, 1] + matrix0[0, 1] * matrix1[1, 1],
            r1c0: matrix0[1, 0] * matrix1[0, 0] + matrix0[1, 1] * matrix1[1, 0],
            r1c1: matrix0[1, 0] * matrix1[0, 1] + matrix0[1, 1] * matrix1[1, 1]);
        public static Matrix2 operator *(float scalar, Matrix2 matrix) => new Matrix2(
            r0c0: scalar * matrix[0, 0],
            r0c1: scalar * matrix[0, 1],
            r1c0: scalar * matrix[1, 0],
            r1c1: scalar * matrix[1, 1]);
        public static Matrix2 operator *(Matrix2 matrix, float scalar) => scalar * matrix;
        public static Vector2 operator *(Matrix2 matrix, Vector2 column) => new Vector2(
            x: matrix[0, 0] * column.X + matrix[0, 1] * column.Y,
            y: matrix[1, 0] * column.X + matrix[1, 1] * column.Y);
        public static Vector2 operator *(Vector2 row, Matrix2 matrix) => new Vector2(
            x: row.X * matrix[0, 0] + row.Y * matrix[1, 0],
            y: row.X * matrix[0, 1] + row.Y * matrix[1, 1]);
    }
}
