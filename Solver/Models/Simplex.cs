﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Solver.Models
{
    public class Simplex
    {
        //source - симплекс таблица без базисных переменных
        double[,] table; //симплекс таблица

        int m, n;
        public String str;
        List<int> basis; //список базисных переменных

        public Simplex(double[] arr)
        {
            
            double[,] source = shuffle(arr);
            m = source.GetLength(0);
            n = source.GetLength(1);
            table = new double[m, n + m - 1];
            basis = new List<int>();

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if (j < n)
                        table[i, j] = source[i, j];
                    else
                        table[i, j] = 0;
                }
                //выставляем коэффициент 1 перед базисной переменной в строке
                if ((n + i) < table.GetLength(1))
                {
                    table[i, n + i] = 1;
                    basis.Add(n + i);
                }
            }

            n = table.GetLength(1);
        }
        private double [,]shuffle(double[] arr)
        {

            double[,] a = new double[5, 3];
            int iteration = 0;
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 3; j++)
                {
                    a[i, j] = arr[iteration]; iteration++;

                }
            double temp;
            for (int i = 0; i < 3; i++)
            {
                temp = a[0, i];
                a[0, i] = a[4, i];
                a[4, i] = temp;

            }

            for (int i = 0; i < 5; i++)
            {

                temp = a[i, 0];
                a[i, 0] = a[i, 2];
                a[i, 2] = temp;

                temp = a[i, 1];
                a[i, 1] = a[i, 2];
                a[i, 2] = temp;

            }
            for (int i = 0; i < 3; i++)
                a[4, i] *= -1;

            return a;
        }
        //result - в этот массив будут записаны полученные значения X
        public double[,] Calculate(double[] result)
        {
            int mainCol, mainRow; //ведущие столбец и строка
            int count = 1;
            str += "Начальная таблица: \n";
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                    str += (table[i, j] + " ");
                str += "\n";
            }
            str += "\n";

            while (!IsItEnd())
            {
                mainCol = findMainCol();
                mainRow = findMainRow(mainCol);
                basis[mainRow] = mainCol;

                double[,] new_table = new double[m, n];

                for (int j = 0; j < n; j++)
                    new_table[mainRow, j] = table[mainRow, j] / table[mainRow, mainCol];

                for (int i = 0; i < m; i++)
                {
                    if (i == mainRow)
                        continue;

                    for (int j = 0; j < n; j++)
                        new_table[i, j] = table[i, j] - table[i, mainCol] * new_table[mainRow, j];
                }
                str += "Этап решения симплекс методом:" + count + "\n";

                for (int i = 0; i < table.GetLength(0); i++)
                {
                    for (int j = 0; j < table.GetLength(1); j++)
                        str += (table[i, j] + " ");
                    str += "\n";
                }
                str += "\n";
                table = new_table; count++;
            }

            //заносим в result найденные значения X
            for (int i = 0; i < result.Length; i++)
            {
                int k = basis.IndexOf(i + 1);
                if (k != -1)
                    result[i] = table[k, 0];
                else
                    result[i] = 0;
            }

            str += "Конечная таблица: \n";
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                    str += (table[i, j] + " ");
                str += "\n";
            }
            str += "\n";
            return table;
        }

        private bool IsItEnd()
        {
            bool flag = true;

            for (int j = 1; j < n; j++)
            {
                if (table[m - 1, j] < 0)
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }

        private int findMainCol()
        {
            int mainCol = 1;

            for (int j = 2; j < n; j++)
                if (table[m - 1, j] < table[m - 1, mainCol])
                    mainCol = j;

            return mainCol;
        }

        private int findMainRow(int mainCol)
        {
            int mainRow = 0;

            for (int i = 0; i < m - 1; i++)
                if (table[i, mainCol] > 0)
                {
                    mainRow = i;
                    break;
                }

            for (int i = mainRow + 1; i < m - 1; i++)
                if ((table[i, mainCol] > 0) && ((table[i, 0] / table[i, mainCol]) < (table[mainRow, 0] / table[mainRow, mainCol])))
                    mainRow = i;

            return mainRow;
        }


    }
}