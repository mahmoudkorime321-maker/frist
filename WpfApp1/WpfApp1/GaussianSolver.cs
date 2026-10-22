using System;
using System.Collections.Generic;

namespace WpfApp1
{
    public class GaussianSolver
    {
        public static List<MatrixStep> SolveToRREF(double[,] matrix)
        {
            var steps = new List<MatrixStep>();
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            steps.Add(new MatrixStep(matrix, "Initial matrix as entered"));

            int lead = 0;
            for (int r = 0; r < rows; r++)
            {
                if (cols <= lead) break;

                int i = r;
                while (Math.Abs(matrix[i, lead]) < 1e-10)
                {
                    i++;
                    if (rows == i)
                    {
                        i = r;
                        lead++;
                        if (cols == lead) return steps;
                    }
                }

                
                if (i != r)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        double temp = matrix[r, j];
                        matrix[r, j] = matrix[i, j];
                        matrix[i, j] = temp;
                    }
                    steps.Add(new MatrixStep(matrix, $"Swapped Row {r + 1} with Row {i + 1}", r));
                }

                
                double div = matrix[r, lead];
                if (Math.Abs(div - 1.0) > 1e-10 && Math.Abs(div) > 1e-10)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        matrix[r, j] /= div;
                    }
                    steps.Add(new MatrixStep(matrix, $"Divided Row {r + 1} by {div:0.##} to make pivot = 1", r));
                }

                
                for (int j = 0; j < rows; j++)
                {
                    if (j != r)
                    {
                        double mult = matrix[j, lead];
                        if (Math.Abs(mult) > 1e-10)
                        {
                            for (int k = 0; k < cols; k++)
                            {
                                matrix[j, k] -= mult * matrix[r, k];
                            }
                            steps.Add(new MatrixStep(matrix, $"Subtracted ({mult:0.##}) × Row {r + 1} from Row {j + 1}", j));
                        }
                    }
                }
                lead++;
            }

            steps.Add(new MatrixStep(matrix, "Successfully reached RREF (Reduced Row Echelon Form)!"));
            return steps;
        }
    }
}