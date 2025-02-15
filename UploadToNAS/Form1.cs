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
                form2.Show();
            }
            else
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                textBox3.Text = desktopPath;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            getFolderPath(textBox1);
        }

        private void getFolderPath(TextBox targetTextBox)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                // �����t�H���_��ݒ� (�I�v�V����)
                folderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                // �_�C�A���O��\��
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    // �I�����ꂽ�t�H���_�p�X���擾
                    string selectedFolder = folderDialog.SelectedPath;

                    // �t�H���_�p�X��\�� (�Ⴆ�΁ATextBox�ɕ\��)
                    targetTextBox.Text = selectedFolder;
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            getFolderPath(textBox2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            getFolderPath(textBox3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" &&
                textBox2.Text != "" &&
                textBox3.Text != "")
            {
                string exePath = Application.ExecutablePath;
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string shortcutPath = Path.Combine(desktopPath, "UploadToNAS.lnk");
                string arguments = $"\"{textBox1.Text}\" \"{textBox2.Text}\" \"{textBox3.Text}\"";
                WshShell wshShell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)wshShell.CreateShortcut(shortcutPath);

                // �V���[�g�J�b�g�̃^�[�Q�b�g�t�@�C���ƈ�����ݒ�
                shortcut.TargetPath = exePath;
                shortcut.Arguments = arguments;

                // �A�C�R���̐ݒ�i�I�v�V�����j
                shortcut.IconLocation = exePath;

                // �V���[�g�J�b�g��ۑ�
                shortcut.Save();
            }
            else
            {
                MessageBox.Show("�����͂̕���������܂�");
            }
        }
    }
}
