using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using Lachesis.QuantumComputing;

namespace grover_alg
{
    class Program
    {
        static void Main(string[] args)
        {

            Grover grover = new Grover(new Random());
            grover.Run();

        }
    }
    class Grover
    {
        protected Random Random;

        public Grover(Random random)
        {
            this.Random = random;
        }

        public void Run()
        {
            int registerLength=3;//кол-во кубитов.
            //registerLength = Int32.Parse(Console.ReadLine());

            QuantumRegister fullRegister = new QuantumRegister(0, registerLength); // Полный регистр
            fullRegister = QuantumGate.HadamardGateOfLength(registerLength) * fullRegister; // преобразование адамара

            Matrix<Complex> matrixOracle = MatrixOracle(registerLength); //Проектирование оракула
            QuantumGate GateOracle = new QuantumGate(matrixOracle);//Получаем гейт (на основе matrixOracle)


            Matrix<Complex> matrixPhaseInverse = PhaseInverse(registerLength);
            QuantumGate GatePhaseInverse = new QuantumGate(matrixPhaseInverse);
            ///////////////
            for (int count = 2; count > 0; count--)
            {
                // Console.WriteLine(1);
                fullRegister = GateOracle * fullRegister;
                //Console.WriteLine(fullRegister);
                //fullRegister = QuantumGate.HadamardGateOfLength(registerLength) * fullRegister;
                //Console.WriteLine(fullRegister);
                fullRegister = GatePhaseInverse * fullRegister;
                //Console.WriteLine(GatePhaseInverse);
                //fullRegister = QuantumGate.HadamardGateOfLength(registerLength) * fullRegister;
            }
            //fullRegister = QuantumGate.HadamardGateOfLength(registerLength) * fullRegister;
            
            
            //Console.WriteLine(fullRegister);
            fullRegister.Collapse(this.Random);//измерение (наблюление)
            int RegisterValue = fullRegister.GetValue(0, registerLength);
            string BinaryRegisterValue = Convert.ToString(RegisterValue, 2);
            Console.WriteLine(BinaryRegisterValue);
            Console.ReadKey();




        }

        protected static Matrix<Complex> MatrixOracle(int registerLegth)
        {
            int matrixSize = 1 << registerLegth;
            Matrix<Complex> matrixOracle = Matrix<Complex>.Build.Sparse(matrixSize, matrixSize);  // создание разреженной матрицы

            // заполнение матрицы. x1^x2^x3
            //1 0 0 0 0 0 0 0 
            //0 1 0 0 0 0 0 0
            //0 0 1 0 0 0 0 0
            //0 0 0 1 0 0 0 0
            //0 0 0 0 1 0 0 0
            //0 0 0 0 0 1 0 0
            //0 0 0 0 0 0 1 0 
            //0 0 0 0 0 0 0 -1


            for (int RowColumn = 0; RowColumn < matrixSize; RowColumn++)
            {
                matrixOracle.At(RowColumn, RowColumn, 1);
            }
            matrixOracle.At(matrixSize-1, matrixSize-1, -1);

            //Console.WriteLine(matrixOracle);
            //Console.ReadKey();

            return matrixOracle;
        }
        protected static Matrix<Complex> PhaseInverse(int registerLegth)
        {
            int matrixSize = 1 << registerLegth;
            Complex n = 2 *1/ Math.Pow(2, registerLegth);
            Matrix<Complex> matrixPhaseInverse = Matrix<Complex>.Build.Dense(matrixSize, matrixSize, n);  // создание разреженной матрицы

            n = n - 1;
            //matrixPhaseInverse=matrixPhaseInverse

            for (int RowColumn = 0; RowColumn < matrixSize; RowColumn++)
            {
                matrixPhaseInverse.At(RowColumn, RowColumn, n);
            }

            //Console.WriteLine(matrixPhaseInverse);
            //Console.WriteLine(n);

            return matrixPhaseInverse;
        }
    }

}
