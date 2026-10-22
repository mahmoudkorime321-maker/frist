using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void GenerateMatrix_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtRows.Text, out int m) && int.TryParse(txtCols.Text, out int n) && m > 0 && n > 0)
                {
                   MatrixGrid.Rows = m;
                   MatrixGrid.Columns = n;
                   MatrixGrid.Children.Clear();
                   for (int i = 0; i < m * n; i++)
                   {
                       TextBox cellInput = new TextBox
                      {
                        Width = 60,
                        Height = 40,
                        Margin = new Thickness(5),
                        TextAlignment = TextAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        FontSize = 16,
                        BorderThickness = new Thickness(2)
                      };
                    
                    cellInput.TextChanged += CellInput_TextChanged;

                    MatrixGrid.Children.Add(cellInput);
                   }
            }
                else
                {
                    MessageBox.Show("Please enter valid positive numbers for dimensions.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
        }
        private void CellInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text) || textBox.Text == "-")
                {
                    textBox.BorderBrush = System.Windows.Media.Brushes.Gray;
                    textBox.Background = System.Windows.Media.Brushes.White;
                    return;
                }

                if (double.TryParse(textBox.Text, out _))
                {
                    textBox.BorderBrush = System.Windows.Media.Brushes.Green;
                    textBox.Background = System.Windows.Media.Brushes.White;
                }
                else
                {
                    textBox.BorderBrush = System.Windows.Media.Brushes.Red;
                    textBox.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 200, 200));
                }
            }
        }
        private System.Collections.Generic.List<MatrixStep> _steps;
        private int _currentStepIndex = 0;
        private void BtnSolve_Click(object sender, RoutedEventArgs e)
        {
            if (MatrixGrid.Children.Count == 0) return;

            int m = MatrixGrid.Rows;
            int n = MatrixGrid.Columns;
            double[,] matrix = new double[m, n];
            int childIndex = 0;

            try
            {
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        TextBox tb = (TextBox)MatrixGrid.Children[childIndex];
                        matrix[i, j] = double.Parse(tb.Text);
                        childIndex++;
                    }
                }
                _steps = GaussianSolver.SolveToRREF(matrix);
                _currentStepIndex = 0;
                DisplayCurrentStep();
            }
            catch (FormatException)
            {
                MessageBox.Show("Please make sure to enter valid numbers in all fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DisplayCurrentStep()
        {
            if (_steps == null || _steps.Count == 0) return;

            var currentStep = _steps[_currentStepIndex];
            txtStepDescription.Text = $"Step {_currentStepIndex + 1} of {_steps.Count}: {currentStep.Description}";

            int m = MatrixGrid.Rows;
            int n = MatrixGrid.Columns;
            int childIndex = 0;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    TextBox tb = (TextBox)MatrixGrid.Children[childIndex];

                    
                    tb.Text = currentStep.State[i, j].ToString("0.##");

                   
                    if (currentStep.HighlightRow.HasValue && currentStep.HighlightRow.Value == i)
                    {
                        tb.Background = System.Windows.Media.Brushes.LightYellow; 
                    }
                    else
                    {
                        tb.Background = System.Windows.Media.Brushes.White;
                    }

                    childIndex++;
                }
            }
        }

       
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_steps != null && _currentStepIndex < _steps.Count - 1)
            {
                _currentStepIndex++;
                DisplayCurrentStep();
            }
        }

       
        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (_steps != null && _currentStepIndex > 0)
            {
                _currentStepIndex--;
                DisplayCurrentStep();
            }
        }
    }
    
}
