namespace Preparatory_Doc_Streaming_On_Windows_Forms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button_CreateAndSaveModel = new Button();
            button_LearnModel = new Button();
            textBox_Logs = new TextBox();
            comboBox_DocumentType = new ComboBox();
            SuspendLayout();
            // 
            // button_CreateAndSaveModel
            // 
            button_CreateAndSaveModel.Location = new Point(12, 57);
            button_CreateAndSaveModel.Name = "button_CreateAndSaveModel";
            button_CreateAndSaveModel.Size = new Size(126, 23);
            button_CreateAndSaveModel.TabIndex = 0;
            button_CreateAndSaveModel.Text = "Create model";
            button_CreateAndSaveModel.UseVisualStyleBackColor = true;
            button_CreateAndSaveModel.Click += button1_Click;
            // 
            // button_LearnModel
            // 
            button_LearnModel.Location = new Point(144, 57);
            button_LearnModel.Name = "button_LearnModel";
            button_LearnModel.Size = new Size(121, 23);
            button_LearnModel.TabIndex = 1;
            button_LearnModel.Text = "Learn model";
            button_LearnModel.UseVisualStyleBackColor = true;
            button_LearnModel.Click += button_LearnModel_Click;
            // 
            // textBox_Logs
            // 
            textBox_Logs.Location = new Point(7, 96);
            textBox_Logs.Multiline = true;
            textBox_Logs.Name = "textBox_Logs";
            textBox_Logs.Size = new Size(781, 144);
            textBox_Logs.TabIndex = 2;
            // 
            // comboBox_DocumentType
            // 
            comboBox_DocumentType.FormattingEnabled = true;
            comboBox_DocumentType.Location = new Point(271, 57);
            comboBox_DocumentType.Name = "comboBox_DocumentType";
            comboBox_DocumentType.Size = new Size(242, 23);
            comboBox_DocumentType.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(comboBox_DocumentType);
            Controls.Add(textBox_Logs);
            Controls.Add(button_LearnModel);
            Controls.Add(button_CreateAndSaveModel);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_CreateAndSaveModel;
        private Button button_LearnModel;
        private TextBox textBox_Logs;
        private ComboBox comboBox_DocumentType;
    }
}
