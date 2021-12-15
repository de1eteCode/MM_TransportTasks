using MM_TransportTasks.Models;
using MM_TransportTasks.Models.Abstracts;
using MM_TransportTasks.Models.Poco;
using MM_TransportTasks.Models.Trasports;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MM_TransportTasks.ViewModels
{
    internal class TransportTaskVM : BaseVM
    {
        private int _selectedSellers;
        private int _selectedBuyers;
        private TaskMethodSelector? _selectedTaskMethod;
        private IFuncZ? _selectedFuncZ;

        public TransportTaskVM()
        {
            MathResultCommand = new RelayCommand(MathResult);
            ResizeCollections();
        }

        #region Properties

        /// <summary>
        /// Продавцы
        /// </summary>
        public IEnumerable<int> Sellers => Enumerable.Range(2, 9);

        /// <summary>
        /// Покупатели
        /// </summary>
        public IEnumerable<int> Buyers => Enumerable.Range(2, 9);

        /// <summary>
        /// Возможные Z(x)
        /// </summary>
        public IEnumerable<IFuncZ> FuncsZ { get; } = new List<IFuncZ>()
        {
            new MinZ(),
            new MaxZ()
        };

        /// <summary>
        /// Возможные методы решения
        /// </summary>
        public IEnumerable<TaskMethodSelector> TaskMethods { get; } = new List<TaskMethodSelector>()
        {
            TaskMethodSelector.Ctor<MinimalPrice>("Мин. цена"),
            TaskMethodSelector.Ctor<NorthWest>("Северо-западный")
        };

        /// <summary>
        /// Количество продавцов (столбцы (Columns))
        /// </summary>
        public int SelectedSellers
        {
            get
            {
                if (_selectedSellers == 0)
                {
                    _selectedSellers = Sellers.First();
                }
                return _selectedSellers;
            }
            set
            {
                _selectedSellers = value;
                OnPropertyChanged();
                ResizeCollections();
            }
        }

        /// <summary>
        /// Количество покупателей (строки (Rows))
        /// </summary>
        public int SelectedBuyers
        {
            get
            {
                if (_selectedBuyers == 0)
                {
                    _selectedBuyers = Buyers.First();
                }
                return _selectedBuyers;
            }
            set
            {
                _selectedBuyers = value;
                OnPropertyChanged();
                ResizeCollections();
            }
        }

        /// <summary>
        /// Выбранный метод решения
        /// </summary>
        public TaskMethodSelector SelectedTaskMethod
        {
            get
            {
                if (_selectedTaskMethod is null)
                {
                    _selectedTaskMethod = TaskMethods.First();
                }
                return _selectedTaskMethod;
            }
            set
            {
                _selectedTaskMethod = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Выбранная Z(x)
        /// </summary>
        public IFuncZ SelectedFuncZ
        {
            get
            {
                if (_selectedFuncZ is null)
                {
                    _selectedFuncZ = FuncsZ.First();
                }
                return _selectedFuncZ;
            }
            set
            {
                _selectedFuncZ = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Матрица данных
        /// </summary>
        public ObservableCollection<MatrixValue> Variables { get; set; } = new();

        /// <summary>
        /// Матрица потребностей
        /// </summary>
        public ObservableCollection<MatrixValue> BuyersNeed { get; set; } = new();

        /// <summary>
        /// Матрица предложений
        /// </summary>
        public ObservableCollection<MatrixValue> SellersOffer { get; set; } = new();

        /// <summary>
        /// Команда для кнопки
        /// </summary>
        public ICommand MathResultCommand { get; }

        #endregion

        /// <summary>
        /// Вызов решения и вызов отображения
        /// </summary>
        private void MathResult()
        {
            var result = SelectedTaskMethod.Calculation(
                            SelectedBuyers,
                            SelectedSellers,
                            BuyersNeed.Select(item => new MatrixValue(item.Value)).ToList(),
                            SellersOffer.Select(item => new MatrixValue(item.Value)).ToList(),
                            Variables.Select(item => new MatrixValue(item.Value)).ToList(),
                            SelectedFuncZ);

            ShowResults(result.Item1, result.Item2);
        }

        /// <summary>
        /// Отображение ответа в виде матрицы
        /// </summary>
        private void ShowResults(int[,] matrix, int z)
        {
            string result = $"Z(x) = {z}\r\n";

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result += matrix[i, j].ToString() + " ";
                }
                result += "\r\n";
            }

            System.Windows.MessageBox.Show(
                "Задача решена.\r\n" + result,
                "Успех",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }

        /// <summary>
        /// Изменение размера матрицы на форме, в зависимости от выбранных параметров
        /// </summary>
        private void ResizeCollections()
        {
            Variables.Clear();

            BuyersNeed.Clear();
            SellersOffer.Clear();

            var cols = SelectedBuyers;
            var rows = SelectedSellers;

            for (int i = 0; i < cols * rows; i++)
            {
                Variables.Add(new MatrixValue(0));
            }

            for (int i = 0; i < cols; i++)
            {
                BuyersNeed.Add(new MatrixValue(0));
            }

            for (int i = 0; i < rows; i++)
            {
                SellersOffer.Add(new MatrixValue(0));
            }

            OnPropertyChanged(nameof(Variables));
        }
    }
}
