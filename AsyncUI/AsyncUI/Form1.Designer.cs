namespace AsyncUI
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
            btnAsync = new Button();
            lbLog = new ListBox();
            SuspendLayout();
            // 
            // btnAsync
            // 
            btnAsync.Location = new Point(24, 22);
            btnAsync.Name = "btnAsync";
            btnAsync.Size = new Size(131, 63);
            btnAsync.TabIndex = 0;
            btnAsync.Text = "비동기 테스트";
            btnAsync.UseVisualStyleBackColor = true;
            btnAsync.Click += btnAsync_Click;
            // 
            // lbLog
            // 
            lbLog.FormattingEnabled = true;
            lbLog.ItemHeight = 15;
            lbLog.Location = new Point(161, 22);
            lbLog.Name = "lbLog";
            lbLog.Size = new Size(291, 334);
            lbLog.TabIndex = 1;
            lbLog.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lbLog);
            Controls.Add(btnAsync);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button btnAsync;
        private ListBox lbLog;
    }
}
