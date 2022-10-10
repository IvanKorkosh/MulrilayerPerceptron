using System.Drawing;
using System.Windows.Forms;
using MultilayerPerseprotron;
using MultilayerPerseprotron.DataStructure;
using MultilayerPerseprotron.Activators;
using System.Collections.Generic;
using System;

namespace СlassicalNeuralNetworkGui
{
    public partial class Form1 : Form
    {
        private readonly INetwork network;

        private readonly List<Vector> trainingSet;
        private readonly List<Vector> trainingSetResult;

        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 6; ++i)
            { 
                dataGridView1.Rows.Add();
            }

            trainingSet = GetSet(Properties.Resources.TrainingSet);
            trainingSetResult = GetSet(Properties.Resources.TrainingSetResult);
            network = GetLearnedNetwork();
        }

        private INetwork GetLearnedNetwork()
        {
            double learningRate = 0.5;
            double moment = 0.3;
            var activator = new Sigmoid();
            var result = new Network(activator, trainingSet[0].Length, trainingSet[0].Length, trainingSetResult.Count);

            int epochsNumber = 0;
            while (true)
            {
                ++epochsNumber;
                double maxDeviation = 0;
                for (int j = 0; j < trainingSet.Count; ++j)
                {
                    var idealResult = trainingSetResult[j];
                    var realResult = result.Recognize(trainingSet[j]);
                    result.Learn(idealResult, learningRate, moment);

                    var deviation = GetMaximumDeviation(realResult, idealResult);
                    maxDeviation = deviation > maxDeviation ? deviation : maxDeviation;
                }

                if (maxDeviation < 0.01)
                {
                    MessageBox.Show(epochsNumber.ToString());
                    return result;
                }
            }
        }

        private double GetMaximumDeviation(Vector real, Vector ideal)
        {
            double maxDeviation = 0;
            for (int i = 0; i < real.Length; ++i)
            {
                double deviation = Math.Abs(ideal[i] - real[i]);
                maxDeviation = deviation > maxDeviation ? deviation : maxDeviation;
            }

            return maxDeviation;
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor == Color.Black)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;
                dataGridView1[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor = Color.White;
            }
            else
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Black;
                dataGridView1[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor = Color.Black;
            }

            GetSimilaritis();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            SetVectorToGrid(trainingSet[0]);
            GetSimilaritis();
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            SetVectorToGrid(trainingSet[1]);
            GetSimilaritis();
        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {
            SetVectorToGrid(trainingSet[2]);
            GetSimilaritis();
        }

        private void PictureBox4_Click(object sender, EventArgs e)
        {
            SetVectorToGrid(trainingSet[3]);
            GetSimilaritis();
        }

        private void PictureBox5_Click(object sender, EventArgs e)
        {
            SetVectorToGrid(trainingSet[4]);
            GetSimilaritis();
        }

        private void GetSimilaritis()
        {
            var results = network.Recognize(GetVectorFromGrid());
            textBox1.Text = ((int)(results[0] * 100)).ToString();
            textBox2.Text = ((int)(results[1] * 100)).ToString();
            textBox3.Text = ((int)(results[2] * 100)).ToString();
            textBox4.Text = ((int)(results[3] * 100)).ToString();
            textBox5.Text = ((int)(results[4] * 100)).ToString();
        }

        private Vector GetVectorFromGrid()
        {
            var vector = new Vector(trainingSet[0].Length);
            for (int row = 0; row < dataGridView1.RowCount; ++row)
            {
                for (int column = 0; column < dataGridView1.ColumnCount; ++column)
                {
                    vector[row * dataGridView1.RowCount + column] = dataGridView1[column, row].Style.BackColor == Color.Black ? 1 : 0;
                }
            }

            return vector;
        }

        private void SetVectorToGrid(Vector vector)
        {
            for (int row = 0; row < dataGridView1.RowCount; ++row)
            {
                for (int column = 0; column < dataGridView1.ColumnCount; ++column)
                {
                    dataGridView1[column, row].Style.BackColor = vector[row * dataGridView1.ColumnCount + column] == 1 ? Color.Black : Color.White;
                    dataGridView1[column, row].Style.SelectionBackColor = vector[row * dataGridView1.ColumnCount + column] == 1 ? Color.Black : Color.White;
                }
            }
        }

        private List<Vector> GetSet(string source)
        {
            var result = new List<Vector>();
            foreach (var item in source.Split(Environment.NewLine))
            {
                result.Add(ToVector(item));
            }

            return result;
        }

        private Vector ToVector(string source)
        {
            var vector = new Vector(source.Length);
            for (int i = 0; i < source.Length; ++i)
            {
                vector[i] = source[i] == '0' ? 0 : 1;
            }

            return vector;
        }
    }
}