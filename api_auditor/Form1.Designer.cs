namespace api_auditor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.txt_controllersPath = new System.Windows.Forms.TextBox();
            this.txt_baseUrl = new System.Windows.Forms.TextBox();
            this.btn_browseFolder = new System.Windows.Forms.Button();
            this.lbl_apiUrl = new System.Windows.Forms.Label();
            this.btn_runAuditor = new System.Windows.Forms.Button();
            this.lbl_csvFileName = new System.Windows.Forms.Label();
            this.txt_csvName = new System.Windows.Forms.TextBox();
            this.lbl_controllerDirPath = new System.Windows.Forms.Label();
            this.pb_progress = new System.Windows.Forms.ProgressBar();
            this.lbl_status = new System.Windows.Forms.Label();
            this.dgv_results = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_results)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_controllersPath
            // 
            this.txt_controllersPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_controllersPath.Location = new System.Drawing.Point(242, 93);
            this.txt_controllersPath.Name = "txt_controllersPath";
            this.txt_controllersPath.Size = new System.Drawing.Size(405, 30);
            this.txt_controllersPath.TabIndex = 0;
            // 
            // txt_baseUrl
            // 
            this.txt_baseUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_baseUrl.Location = new System.Drawing.Point(156, 50);
            this.txt_baseUrl.Name = "txt_baseUrl";
            this.txt_baseUrl.Size = new System.Drawing.Size(618, 30);
            this.txt_baseUrl.TabIndex = 1;
            // 
            // btn_browseFolder
            // 
            this.btn_browseFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_browseFolder.Location = new System.Drawing.Point(653, 90);
            this.btn_browseFolder.Name = "btn_browseFolder";
            this.btn_browseFolder.Size = new System.Drawing.Size(121, 36);
            this.btn_browseFolder.TabIndex = 2;
            this.btn_browseFolder.Text = "buscar...";
            this.btn_browseFolder.UseVisualStyleBackColor = true;
            this.btn_browseFolder.Click += new System.EventHandler(this.btn_browseFolder_Click);
            // 
            // lbl_apiUrl
            // 
            this.lbl_apiUrl.AutoSize = true;
            this.lbl_apiUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_apiUrl.Location = new System.Drawing.Point(21, 50);
            this.lbl_apiUrl.Name = "lbl_apiUrl";
            this.lbl_apiUrl.Size = new System.Drawing.Size(129, 32);
            this.lbl_apiUrl.TabIndex = 3;
            this.lbl_apiUrl.Text = "API URl:";
            // 
            // btn_runAuditor
            // 
            this.btn_runAuditor.BackColor = System.Drawing.Color.LightGreen;
            this.btn_runAuditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_runAuditor.Location = new System.Drawing.Point(27, 191);
            this.btn_runAuditor.Name = "btn_runAuditor";
            this.btn_runAuditor.Size = new System.Drawing.Size(205, 87);
            this.btn_runAuditor.TabIndex = 4;
            this.btn_runAuditor.Text = "Ejecutar Auditoria";
            this.btn_runAuditor.UseVisualStyleBackColor = false;
            this.btn_runAuditor.Click += new System.EventHandler(this.btn_runAuditor_Click);
            // 
            // lbl_csvFileName
            // 
            this.lbl_csvFileName.AutoSize = true;
            this.lbl_csvFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_csvFileName.Location = new System.Drawing.Point(21, 142);
            this.lbl_csvFileName.Name = "lbl_csvFileName";
            this.lbl_csvFileName.Size = new System.Drawing.Size(278, 32);
            this.lbl_csvFileName.TabIndex = 6;
            this.lbl_csvFileName.Text = "Nombre de archivo:";
            // 
            // txt_csvName
            // 
            this.txt_csvName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_csvName.Location = new System.Drawing.Point(298, 142);
            this.txt_csvName.Name = "txt_csvName";
            this.txt_csvName.Size = new System.Drawing.Size(476, 30);
            this.txt_csvName.TabIndex = 5;
            // 
            // lbl_controllerDirPath
            // 
            this.lbl_controllerDirPath.AutoSize = true;
            this.lbl_controllerDirPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_controllerDirPath.Location = new System.Drawing.Point(21, 90);
            this.lbl_controllerDirPath.Name = "lbl_controllerDirPath";
            this.lbl_controllerDirPath.Size = new System.Drawing.Size(215, 32);
            this.lbl_controllerDirPath.TabIndex = 7;
            this.lbl_controllerDirPath.Text = "Controladores:";
            // 
            // pb_progress
            // 
            this.pb_progress.ForeColor = System.Drawing.Color.Chartreuse;
            this.pb_progress.Location = new System.Drawing.Point(238, 255);
            this.pb_progress.Name = "pb_progress";
            this.pb_progress.Size = new System.Drawing.Size(535, 23);
            this.pb_progress.TabIndex = 8;
            // 
            // lbl_status
            // 
            this.lbl_status.AutoSize = true;
            this.lbl_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_status.Location = new System.Drawing.Point(361, 201);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(0, 32);
            this.lbl_status.TabIndex = 10;
            // 
            // dgv_results
            // 
            this.dgv_results.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_results.Location = new System.Drawing.Point(27, 321);
            this.dgv_results.Name = "dgv_results";
            this.dgv_results.RowHeadersWidth = 51;
            this.dgv_results.RowTemplate.Height = 24;
            this.dgv_results.Size = new System.Drawing.Size(734, 104);
            this.dgv_results.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgv_results);
            this.Controls.Add(this.lbl_status);
            this.Controls.Add(this.pb_progress);
            this.Controls.Add(this.lbl_controllerDirPath);
            this.Controls.Add(this.lbl_csvFileName);
            this.Controls.Add(this.txt_csvName);
            this.Controls.Add(this.btn_runAuditor);
            this.Controls.Add(this.lbl_apiUrl);
            this.Controls.Add(this.btn_browseFolder);
            this.Controls.Add(this.txt_baseUrl);
            this.Controls.Add(this.txt_controllersPath);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_results)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txt_controllersPath;
        private System.Windows.Forms.TextBox txt_baseUrl;
        private System.Windows.Forms.Button btn_browseFolder;
        private System.Windows.Forms.Label lbl_apiUrl;
        private System.Windows.Forms.Button btn_runAuditor;
        private System.Windows.Forms.Label lbl_csvFileName;
        private System.Windows.Forms.TextBox txt_csvName;
        private System.Windows.Forms.Label lbl_controllerDirPath;
        private System.Windows.Forms.ProgressBar pb_progress;
        private System.Windows.Forms.Label lbl_status;
        private System.Windows.Forms.DataGridView dgv_results;
    }
}

