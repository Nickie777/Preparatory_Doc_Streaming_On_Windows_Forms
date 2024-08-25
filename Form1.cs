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
        private string _modelPath;  // ���������� ��� �������� ���� � ������

        public Form1()
        {
            InitializeComponent();

            // ������������� MLContext, ����������� ����� ���������� � ���� � ������
            _mlContext = new MLContext();
            _documentTypeCatalog = new DocumentTypeCatalog();
            _modelPath = "model.zip";  // ���������� ��������� �������� ��� ���� � ������
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // ������� ���� � �����, ���������� �����������
            string imageFolder = @"D:\test\Images";

            // �������� ������ ������
            var emptyData = _mlContext.Data.LoadFromEnumerable<DocumentData>(new List<DocumentData>());

            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("Label")
                .Append(_mlContext.Transforms.LoadImages("ImagePath", imageFolder, nameof(DocumentData.ImagePath)))
                .Append(_mlContext.Transforms.ResizeImages("ImagePath", 224, 224))
                .Append(_mlContext.Transforms.ExtractPixels("ImagePath"));

            // ������� ������ �� ������ ������ (�� ����, ��� "�������" ������)
            _model = pipeline.Fit(emptyData);

            // ���������� ������ � ����
            SaveModel(_modelPath);

            MessageBox.Show($"������ ������ ������� � ��������� � ���� {_modelPath}!");
        }

        // ����� ��� ���������� ������
        private void SaveModel(string modelPath)
        {
            _mlContext.Model.Save(_model, null, modelPath);
        }

        // ����� ��� �������� ������
        private void LoadModel(string modelPath)
        {
            _model = _mlContext.Model.Load(modelPath, out var _);
        }

        // ����� ��� ������������� ������ ���������
        public class DocumentData
        {
            public string ImagePath { get; set; }
            public string Label { get; set; }
        }

        private void button_LearnModel_Click(object sender, EventArgs e)
        {

        }
    }

    // ����� ��� ����������� ����� ����������
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
                new DocumentType { Name = "���" },
                new DocumentType { Name = "����-�������" },
                new DocumentType { Name = "�������� ���������" },
                new DocumentType { Name = "���" }
            };
        }

        public void AddDocumentType(string documentTypeName)
        {
            DocumentTypes.Add(new DocumentType { Name = documentTypeName });
        }
    }
}
