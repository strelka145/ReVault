using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UploadToNAS
{
    public partial class Form2 : Form
    {
        private string sourceDir;       // コピー元ディレクトリ
        private string destinationRoot; // 保存先ディレクトリ
        private string lastRunFile;     // 最終実行時刻を記録するファイルの保存場所

        // コンストラクタ: 引数で各種ディレクトリ情報を受け取る
        public Form2(string arg1, string arg2, string arg3)
        {
            InitializeComponent();
            sourceDir = arg1;
            destinationRoot = arg2;
            lastRunFile = arg3;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            UploadToNAS();
        }

        private void UploadToNAS()
        {
            // コピー元ディレクトリが存在しない場合、警告を表示
            if (!Directory.Exists(sourceDir))
            {
                MessageBox.Show($"エラー: コピー元フォルダが存在しません: {sourceDir}", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            // 保存先ディレクトリが存在しない場合、警告を表示
            if (!Directory.Exists(destinationRoot))
            {
                MessageBox.Show($"エラー: 保存先フォルダが存在しません: {destinationRoot}", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // 最終実行時刻を取得
            DateTime lastRunTime = GetLastRunTime(lastRunFile);
            DateTime currentRunTime = DateTime.Now;

            // 指定ディレクトリ以下のファイルのうち、最終実行時刻より新しいものを取得
            var files = Directory.EnumerateFiles(sourceDir, "*.*", SearchOption.AllDirectories)
                .Where(file => File.GetLastWriteTime(file) > lastRunTime);

            int copiedFileCount = 0;
            foreach (var file in files)
            {
                try
                {
                    string relativePath = Path.GetRelativePath(sourceDir, file);

                    string yearFolder = currentRunTime.ToString("yyyy");
                    string timestampFolder = currentRunTime.ToString("yyyyMMddHHmm");
                    string destinationDir = Path.Combine(destinationRoot, yearFolder, timestampFolder, Path.GetDirectoryName(relativePath) ?? "");

                    Directory.CreateDirectory(destinationDir);
                    string destinationFile = Path.Combine(destinationDir, Path.GetFileName(file));
                    File.Copy(file, destinationFile, true);

                    textBox1.AppendText($"Copy: {file} → {destinationFile}\r\n");
                    copiedFileCount++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"エラー: {file} のコピーに失敗: {ex.Message}", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            textBox1.AppendText($"処理完了: {copiedFileCount} 個のファイルをコピーしました。\r\n");
            if (copiedFileCount > 0)
            {
                File.WriteAllText(lastRunFile, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
                Application.Exit();
        }

        static DateTime GetLastRunTime(string lastRunFile)
        {
            if (!File.Exists(lastRunFile))
            {
                File.WriteAllText(lastRunFile+"\\lastrun.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return DateTime.Now; // 初回起動
            }

            string content = File.ReadAllText(lastRunFile);
            if (DateTime.TryParse(content, out DateTime lastRunTime))
            {
                return lastRunTime;
            }

            return DateTime.Now;
        }
    }
}
