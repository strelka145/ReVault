using IWshRuntimeLibrary;
namespace UploadToNAS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length - 1 == 3)
            {
                string arg1 = args[1];
                string arg2 = args[2];
                string arg3 = args[3];
                Form2 form2 = new Form2(arg1, arg2, arg3);
                form2.ShowDialog();
            }
            else
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                lastrunTXT_path.Text = desktopPath;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            getFolderPath(original_data_path);
        }

        private void getFolderPath(TextBox targetTextBox)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                // 初期フォルダを設定 (オプション)
                folderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                // ダイアログを表示
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    // 選択されたフォルダパスを取得
                    string selectedFolder = folderDialog.SelectedPath;

                    // フォルダパスを表示 (例えば、TextBoxに表示)
                    targetTextBox.Text = selectedFolder;
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            getFolderPath(save_path);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            getFolderPath(lastrunTXT_path);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (original_data_path.Text != "" &&
                save_path.Text != "" &&
                lastrunTXT_path.Text != "")
            {
                string exePath = Application.ExecutablePath;
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string shortcutPath = Path.Combine(desktopPath, "UploadToNAS.lnk");
                string arguments = $"\"{original_data_path.Text}\" \"{save_path.Text}\" \"{lastrunTXT_path.Text}\"";
                WshShell wshShell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)wshShell.CreateShortcut(shortcutPath);

                // ショートカットのターゲットファイルと引数を設定
                shortcut.TargetPath = exePath;
                shortcut.Arguments = arguments;

                // アイコンの設定（オプション）
                shortcut.IconLocation = exePath;

                // ショートカットを保存
                shortcut.Save();
                System.IO.File.WriteAllText(lastrunTXT_path.Text + "\\lastrun.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                MessageBox.Show("未入力の部分があります");
            }
        }
    }
}
