using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DM_Task_2
{
    public partial class Form1 : Form
    {
        int[,] matrix;
        public int[,] Matrix { get => matrix; set => matrix = value; }
        public Form1()
        {
            InitializeComponent();
            dataGridView1.AllowUserToAddRows = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int n) || n < 5)
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                for (int i = 0; i < n; i++)
                {
                    dataGridView1.Columns.Add((i + 1).ToString(), (i + 1).ToString());
                    dataGridView1.Rows.Add();
                }
                Matrix = new int[n, n];
            }
            else
            {
                MessageBox.Show("Введите корректное значение для N.");
            }
        }
        public string CheckIntroversion()
        {
            int n = matrix.GetLength(0); 

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (matrix[i, j] != 0)
                    {
                        
                        bool[] visited = new bool[n];
                        if (!DFS(i, j, -1, visited))
                        {
                            return "не замкнута"; 
                        }
                    }
                }
            }

            return "Замкнута"; 
        }

        private bool DFS(int current, int target, int parent, bool[] visited)
        {
            visited[current] = true;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (matrix[current, i] != 0)
                {
                    if (i == target)
                    {
                        return true; 
                    }
                    if (!visited[i] && i != parent && DFS(i, target, current, visited))
                    {
                        return true; 
                    }
                }
            }

            return false;
        }
    
        void _readsDataGridView()
        {
            int n = dataGridView1.Columns.Count;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (int.TryParse(dataGridView1.Rows[i].Cells[j].Value?.ToString(), out int cellValue))
                    {
                        Matrix[i, j] = cellValue;
                    }
                    else
                    {
                        MessageBox.Show("Введите корректные значения для ячеек.");
                        return;
                    }
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            _readsDataGridView();
            label1.Text = CheckIntroversion();
            label2.Text = NeutralElement();
            label3.Text = CheckAssociativity();
            label4.Text = IsOperationCommutative();
            label5.Text = ClassifyGroupType();
        }
        public string ClassifyGroupType()
        {
            bool isIntroverted = !CheckIntroversion().Contains("не");//замкнута
            bool isAssociative = !CheckAssociativity().Contains("не");//ассоциативность
            bool hasIdentity = !NeutralElement().Contains("не");//Нейтральный элемент
            bool isCommutative = !IsOperationCommutative().Contains("не");//коммутативная

            if (isAssociative && hasIdentity && isIntroverted && isCommutative)
            {
                return "Абелева группа";
            }
            else if (isAssociative && hasIdentity && isIntroverted)
            {
                return "Группа";
            }
            else if (isAssociative && hasIdentity)
            {
                return "Петля";
            }
            else if (isAssociative && isCommutative)
            {
                return "Квазигруппа";
            }
            else if (isAssociative)
            {
                return "Моноид";
            }
            else if (isCommutative)
            {
                return "Полугруппа";
            }
            else if (!isAssociative)
            {
                return "Группоид";
            }
            else
            {
                return "Неизвестный тип";
            }
        }
        public string IsOperationCommutative()
        {
            int n = matrix.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (matrix[i, j] != matrix[j, i])
                    {
                        return "не коммутативная операция";
                    }
                }
            }
            return "Коммутативная операция";
        }
        public string IsMultiplicationIdentity()
        {
            int n = matrix.GetLength(0);

            
            int[,] identityMatrix = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    identityMatrix[i, j] = (i == j) ? 1 : 0;
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (matrix[i, j] != identityMatrix[i, j])
                    {
                        return "Нейтрального элемента нет"; 
                    }
                }
            }
            return "Нейтрального элемент есть"; 
        }
        public string CheckAssociativity()
        {
            try
            {
                int n = matrix.GetLength(1);
                string st = "";
                for (int a = 0; a < n; a++)
                {
                    for (int b = 0; b < n; b++)
                    {
                        for (int c = 0; c < n; c++)
                        {
                            if (matrix[a, matrix[b, c]] == matrix[matrix[a, b], c])
                            {
                                st = "ассоциативна";
                                return "ассоциативна";
                            }
                            else
                            {
                                st = "не ассоциативна";
                                return "не ассоциативна";
                            }
                        }
                    }
                }
                return st;
            }
            catch {
                MessageBox.Show("Ошибка: элемент превысел допустимые значения");
                return "не ассоциативна";
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private static bool IsElementInSet(int[,] matrix, int element)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (matrix[i, j] == element)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        private string NeutralElement()
        {
            List<Func<int, int, int>> operations = new List<Func<int, int, int>>()
            { 
            (x, y) => x * y,
            };

            foreach (var operation in operations)
            {
                int neutralElement = FindNeutralElement(matrix, operation);
                bool isNeutralElementInSet = IsElementInSet(matrix, neutralElement);
                if (isNeutralElementInSet)
                    return ("Нейтральный элемент для данной операции: " + neutralElement);
            }
            return "Нейтральный элемент не найден";
        }
        private static int FindNeutralElement(int[,] matrix, Func<int, int, int> operation)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);

            int neutralElement = matrix[0, 0];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    neutralElement = operation(neutralElement, matrix[i, j]);
                }
            }

            return neutralElement;
        }
    }
}
