using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinearAlgebra.Matricies
{
    public class RotationMatrices
    {
        delegate DoubleMatrix CS2Matrix(double c1, double s1, double c2, double s2, double c3, double s3);

        static CS2Matrix[,,] BASIC_EQUATIONS;
        static int MATRIX_ITER_LENGTH = 0;
        static int[][] ITER_BUFFER;
        static DoubleMatrix CONVERSION = new DoubleMatrix(new Double[,]{{ -5,  12,  -6},{-12,  25, -12}, { 20, -40,  19}});
        public DoubleMatrix SUB_CONVERSION = new DoubleMatrix(new Double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } });
        public static bool IS_RIGHT_HAND = true;
        public static DoubleMatrix GetRotationMatrixWithHandness(int axis_id1, int axis_id2, int axis_id3, double angle1, double angle2, double angle3)
        {
            DoubleMatrix result;
            if (IS_RIGHT_HAND)
            {
                result = GetStandardRotationMatrix(axis_id1, axis_id2, axis_id3, angle1, angle2, angle3);
            }
            else
            {
                result = GetLeftHandMatrix(axis_id1, axis_id2, axis_id3, angle1, angle2, angle3);
            }

            return result;
        }

        static void TryInit()
        {
            if (BASIC_EQUATIONS == null)
            {
                BASIC_EQUATIONS = new CS2Matrix[3, 3, 3];
                BASIC_EQUATIONS[0, 2, 0] = (c1, s1, c2, s2, c3, s3) => Zip(c2, -c3 * s2, s2 * s3, c1 * s2, c1 * c2 * c3 - s1 * s3, -c3 * s1 - c1 * c2 * s3, s1 * s2, c1 * s3 + c2 * c3 * s1, c1 * c3 - c2 * s1 * s3);
                BASIC_EQUATIONS[0, 2, 0] = (c1, s1, c2, s2, c3, s3) => Zip(c2, -c3 * s2, s2 * s3, c1 * s2, c1 * c2 * c3 - s1 * s3, -c3 * s1 - c1 * c2 * s3, s1 * s2, c1 * s3 + c2 * c3 * s1, c1 * c3 - c2 * s1 * s3);
                BASIC_EQUATIONS[0, 1, 0] = (c1, s1, c2, s2, c3, s3) => Zip(c2, s2 * s3, c3 * s2, s1 * s2, c1 * c3 - c2 * s1 * s3, -c1 * s3 - c2 * c3 * s1, -c1 * s2, c3 * s1 + c1 * c2 * s3, c1 * c2 * c3 - s1 * s3);
                BASIC_EQUATIONS[1, 0, 1] = (c1, s1, c2, s2, c3, s3) => Zip(c1 * c3 - c2 * s1 * s3, s1 * s2, c1 * s3 + c2 * c3 * s1, s2 * s3, c2, -c3 * s2, -c3 * s1 - c1 * c2 * s3, c1 * s2, c1 * c2 * c3 - s1 * s3);
                BASIC_EQUATIONS[1, 2, 1] = (c1, s1, c2, s2, c3, s3) => Zip(c1 * c2 * c3 - s1 * s3, -c1 * s2, c3 * s1 + c1 * c2 * s3, c3 * s2, c2, s2 * s3, -c1 * s3 - c2 * c3 * s1, s1 * s2, c1 * c3 - c2 * s1 * s3);
                BASIC_EQUATIONS[2, 1, 2] = (c1, s1, c2, s2, c3, s3) => Zip(c1 * c2 * c3 - s1 * s3, -c3 * s1 - c1 * c2 * s3, c1 * s2, c1 * s3 + c2 * c3 * s1, c1 * c3 - c2 * s1 * s3, s1 * s2, -c3 * s2, s2 * s3, c2);
                BASIC_EQUATIONS[2, 0, 2] = (c1, s1, c2, s2, c3, s3) => Zip(c1 * c3 - c2 * s1 * s3, -c1 * s3 - c2 * c3 * s1, s1 * s2, c3 * s1 + c1 * c2 * s3, c1 * c2 * c3 - s1 * s3, -c1 * s2, s2 * s3, c3 * s2, c2);
                BASIC_EQUATIONS[0, 2, 1] = (c1, s1, c2, s2, c3, s3) => Zip(c2 * c3, -s2, c2 * s3, s1 * s3 + c1 * c3 * s2, c1 * c2, c1 * s2 * s3 - c3 * s1, c3 * s1 * s2 - c1 * s3, c2 * s1, c1 * c3 + s1 * s2 * s3);
                BASIC_EQUATIONS[0, 1, 2] = (c1, s1, c2, s2, c3, s3) => Zip(c2 * c3, -c2 * s3, s2, c1 * s3 + c3 * s1 * s2, c1 * c3 - s1 * s2 * s3, -c2 * s1, s1 * s3 - c1 * c3 * s2, c3 * s1 + c1 * s2 * s3, c1 * c2);
                BASIC_EQUATIONS[1, 0, 2] = (c1, s1, c2, s2, c3, s3) => Zip(c1 * c3 + s1 * s2 * s3, c3 * s1 * s2 - c1 * s3, c2 * s1, c2 * s3, c2 * c3, -s2, c1 * s2 * s3 - c3 * s1, c1 * c3 * s2 + s1 * s3, c1 * c2);
                BASIC_EQUATIONS[1, 2, 0] = (c1, s1, c2, s2, c3, s3) => Zip(c1 * c2, s1 * s3 - c1 * c3 * s2, c3 * s1 + c1 * s2 * s3, s2, c2 * c3, -c2 * s3, -c2 * s1, c1 * s3 + c3 * s1 * s2, c1 * c3 - s1 * s2 * s3);
                BASIC_EQUATIONS[2, 1, 0] = (c1, s1, c2, s2, c3, s3) => Zip(c1 * c2, c1 * s2 * s3 - c3 * s1, s1 * s3 + c1 * c3 * s2, c2 * s1, c1 * c3 + s1 * s2 * s3, c3 * s1 * s2 - c1 * s3, -s2, c2 * s3, c2 * c3);
                BASIC_EQUATIONS[2, 0, 1] = (c1, s1, c2, s2, c3, s3) => Zip(c1 * c3 - s1 * s2 * s3, -c2 * s1, c1 * s3 + c3 * s1 * s2, c3 * s1 + c1 * s2 * s3, c1 * c2, s1 * s3 - c1 * c3 * s2, -c2 * s3, s2, c2 * c3);
            }
        }

        static void CalculateSinCos(double a, double b, double c, out double c1, out double s1, out double c2, out double s2, out double c3, out double s3) 
        {
            c1 = Math.Cos(a);
            s1 = Math.Sin(a);
            c2 = Math.Cos(b);
            s2 = Math.Sin(b);
            c3 = Math.Cos(c);
            s3 = Math.Sin(c);
        }

        static DoubleMatrix Zip(params double []flattened)
        {
            var f = flattened;
            DoubleMatrix result = new DoubleMatrix(new double[,]{{f[0],f[1],f[2]},{f[3],f[4],f[5]},{f[6],f[7],f[8]}});
            return result;
        }

        public static DoubleMatrix GetStandardRotationMatrix(int axis_id1, int axis_id2, int axis_id3, double angle1, double angle2, double angle3)
        {
            TryInit();
            if (BASIC_EQUATIONS[axis_id1, axis_id2, axis_id3] == null)
                return null;
            double a = angle1, b = angle2, c = angle3;
            double c1, s1, c2, s2, c3, s3;
            CalculateSinCos(a, b, c, out c1, out s1, out c2, out s2, out c3, out s3);
            DoubleMatrix result = BASIC_EQUATIONS[axis_id1, axis_id2, axis_id3](c1, s1, c2, s2, c3, s3);
            return result;
        }

        public static DoubleMatrix GetLeftHandMatrix(int axis_id1, int axis_id2, int axis_id3, double angle1, double angle2, double angle3)
        {
            if (axis_id1 != 2)
                angle1 = -angle1;
            if (axis_id2 != 2)
                angle2 = -angle2;
            if (axis_id3 != 2)
                angle3 = -angle3;
            var result = GetStandardRotationMatrix(axis_id1, axis_id2, axis_id3, angle1, angle2, angle3);
            //result = SUB_* result;
            return result;
        }

        public static IEnumerable<int[]> GetAllMatrices()
        {
            TryInit();
            if (ITER_BUFFER == null)
                ITER_BUFFER = new int[3*3*3][];
            if (MATRIX_ITER_LENGTH == 0)
            {
                for (int i = 0; i < 3; i ++)
                    for (int j = 0; j < 3; j ++)
                        for (int k = 0; k < 3; k ++)
                            if (BASIC_EQUATIONS[i,j,k] != null)
                            {
                                ITER_BUFFER[MATRIX_ITER_LENGTH++] = new int[]{i,j,k};
                            }
            }

            for (int i = 0; i < MATRIX_ITER_LENGTH; i ++)
                yield return ITER_BUFFER[i];
        }
    }
}
