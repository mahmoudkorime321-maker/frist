using System;

namespace WpfApp1
{
    public class MatrixStep
    {
       
        public double[,] State { get; set; }
        public string Description { get; set; }
        public int? HighlightRow { get; set; }
        public MatrixStep(double[,] state, string description, int? highlightRow = null)
        {
            int rows = state.GetLength(0);
            int cols = state.GetLength(1);
            State = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    State[i, j] = state[i, j];
                }
            }
            Description = description;
            HighlightRow = highlightRow;
        }
    }
}