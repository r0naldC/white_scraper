using api_auditor.Classes;
using System;
using System.IO;
using System.Windows.Forms;

namespace api_auditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_browseFolder_Click(object sender, System.EventArgs e)
        {
            var dialog = new FolderBrowserDialog
            {
                Description = "Selecciona la carpeta de controladores"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txt_controllersPath.Text = dialog.SelectedPath;
            }
        }

        private async void btn_runAuditor_Click(object sender, System.EventArgs e)
        {
            var controllersPath = txt_controllersPath.Text.Trim();
            var baseUrl = txt_baseUrl.Text.Trim().TrimEnd('/');
            var csvName = txt_csvName.Text.Trim();

            if (string.IsNullOrWhiteSpace(controllersPath) || !Directory.Exists(controllersPath))
            {
                MessageBox.Show("Selecciona una carpeta válida.");
                return;
            }

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                MessageBox.Show("Escribe una URL base válida.");
                return;
            }

            if (string.IsNullOrWhiteSpace(csvName))
                csvName = "resultados_endpoints.csv";

            lbl_status.Text = "Procesando...";
            btn_runAuditor.Enabled = false;

            try
            {
                var outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, csvName);
                var results = await EndpointTester.GenerateReportAsync(controllersPath);
                //var results = await EndpointAnalyzer.RunAsync(controllersPath, baseUrl, outputPath);
                dgv_results.DataSource = results;
                lbl_status.Text = $"Terminado. {results.Count} endpoints procesados.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                lbl_status.Text = "Error";
            }
            finally
            {
                lbl_status.Enabled = true;
            }
        }
    }
}
