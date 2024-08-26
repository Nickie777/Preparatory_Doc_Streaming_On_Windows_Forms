using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Preparatory_Doc_Streaming_On_Windows_Forms
{
    public partial class Form1 : Form
    {
        private MLContext _mlContext;
        private ITransformer _model;
        private DocumentTypeCatalog _documentTypeCatalog;
        private string _modelPath;

        public Form1()
        {
            InitializeComponent();

            // Инициализация MLContext, справочника типов документов и пути к модели
            _mlContext = new MLContext();
            _documentTypeCatalog = new DocumentTypeCatalog();
            _modelPath = "model.zip";

            // Инициализация ComboBox
            comboBox_DocumentType.DataSource = _documentTypeCatalog.DocumentTypes;
            comboBox_DocumentType.DisplayMember = "Name";

            // Загрузка существующей модели, если она есть
            if (File.Exists(_modelPath))
            {
                LoadModel(_modelPath);
            }
            else
            {
                _model = null; // Убедимся, что _model изначально имеет значение null
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Укажите путь к папке, содержащей изображения
            string imageFolder = @"D:\test\Images";

            // Создание пустой модели
            var emptyData = _mlContext.Data.LoadFromEnumerable<DocumentData>(new List<DocumentData>());

            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("Label")
                .Append(_mlContext.Transforms.LoadImages("ImagePath", imageFolder, nameof(DocumentData.ImagePath)))
                .Append(_mlContext.Transforms.ResizeImages("ImagePath", 224, 224))
                .Append(_mlContext.Transforms.ExtractPixels("ImagePath"));

            // Обучаем модель на пустых данных (по сути, это "нулевая" модель)
            _model = pipeline.Fit(emptyData);

            // Сохранение модели в файл
            SaveModel(_modelPath);

            MessageBox.Show($"Пустая модель создана и сохранена в файл {_modelPath}!");
        }

        private void button_LearnModel_Click(object sender, EventArgs e)
        {
            // 1. Проверка, выбрано ли значение в ComboBox
            if (comboBox_DocumentType.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите тип документа из списка.");
                return;
            }

            // 2. Открытие диалога для выбора файла изображения
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedImagePath = openFileDialog.FileName;
                string selectedDocumentType = (comboBox_DocumentType.SelectedItem as DocumentType).Name;

                // Логика дообучения модели
                List<DocumentData> trainingData = new List<DocumentData>
                {
                    new DocumentData { ImagePath = selectedImagePath, Label = selectedDocumentType }
                };

                // Подготовка данных для дообучения
                IDataView dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

                // Подготовка пайплайна для дообучения
                var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("Label")
                    .Append(_mlContext.Transforms.LoadImages("ImagePath", Path.GetDirectoryName(selectedImagePath), nameof(DocumentData.ImagePath)))
                    .Append(_mlContext.Transforms.ResizeImages("ImagePath", 224, 224))
                    .Append(_mlContext.Transforms.ExtractPixels("ImagePath"));

                // Дообучение модели
                var newModel = pipeline.Fit(dataView);

                // Применение нового пайплайна к новым данным для создания модели
                IDataView transformedData = newModel.Transform(dataView);

                // Проверка, что _model не равна null перед объединением
                if (_model != null)
                {
                    textBox_Logs.Text = " Объединение с текущей моделью";
                    // Объединение с текущей моделью
                    var finalModel = _model.Append(newModel);
                    _model = finalModel;
                }
                else
                {
                    textBox_Logs.Text = " Используем новую модель";
                    // Если _model была null, просто используем новую модель
                    _model = newModel;
                }

                // Сохранение модели
                SaveModel(_modelPath);
                MessageBox.Show($"Модель успешно дообучена и сохранена в файл {_modelPath}.");
            }
            else
            {
                MessageBox.Show("Файл изображения не выбран.");
            }
        }

        // Метод для сохранения модели
        private void SaveModel(string modelPath)
        {
            _mlContext.Model.Save(_model, null, modelPath);
        }

        // Метод для загрузки модели
        private void LoadModel(string modelPath)
        {
            _model = _mlContext.Model.Load(modelPath, out var _);
        }

        // Класс для представления данных документа
        public class DocumentData
        {
            public string ImagePath { get; set; }
            public string Label { get; set; }
        }
    }

    // Класс для справочника типов документов
    public class DocumentType
    {
        public string Name { get; set; }
    }

    public class DocumentTypeCatalog
    {
        public List<DocumentType> DocumentTypes { get; set; }

        public DocumentTypeCatalog()
        {
            DocumentTypes = new List<DocumentType>
            {
                new DocumentType { Name = "УПД" },
                new DocumentType { Name = "Счет-фактура" },
                new DocumentType { Name = "Товарная накладная" },
                new DocumentType { Name = "Акт" }
            };
        }

        public void AddDocumentType(string documentTypeName)
        {
            DocumentTypes.Add(new DocumentType { Name = documentTypeName });
        }
    }
}
