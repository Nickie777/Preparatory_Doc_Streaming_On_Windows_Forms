using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Preparatory_Doc_Streaming_On_Windows_Forms
{
    public partial class Form1 : Form
    {
        private MLContext _mlContext;
        private ITransformer _model;
        private DocumentTypeCatalog _documentTypeCatalog;
        private string _modelPath;  // Переменная для хранения пути к модели

        public Form1()
        {
            InitializeComponent();

            // Инициализация MLContext, справочника типов документов и пути к модели
            _mlContext = new MLContext();
            _documentTypeCatalog = new DocumentTypeCatalog();
            _modelPath = "model.zip";  // Установите начальное значение для пути к модели
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

        private void button_LearnModel_Click(object sender, EventArgs e)
        {

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
