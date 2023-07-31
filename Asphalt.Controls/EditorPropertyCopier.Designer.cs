namespace Asphalt.Controls
{
    partial class EditorPropertyCopier
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CopyButton = new System.Windows.Forms.Button();
            this.PasteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CopyButton
            // 
            this.CopyButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.CopyButton.Location = new System.Drawing.Point(0, 0);
            this.CopyButton.Name = "CopyButton";
            this.CopyButton.Size = new System.Drawing.Size(123, 23);
            this.CopyButton.TabIndex = 0;
            this.CopyButton.Text = "Copy to Clipboard";
            this.CopyButton.UseVisualStyleBackColor = true;
            this.CopyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // PasteButton
            // 
            this.PasteButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.PasteButton.Location = new System.Drawing.Point(0, 23);
            this.PasteButton.Name = "PasteButton";
            this.PasteButton.Size = new System.Drawing.Size(123, 23);
            this.PasteButton.TabIndex = 1;
            this.PasteButton.Text = "Paste from Clipboard";
            this.PasteButton.UseVisualStyleBackColor = true;
            this.PasteButton.Click += new System.EventHandler(this.PasteButton_Click);
            // 
            // EditorPropertyCopier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PasteButton);
            this.Controls.Add(this.CopyButton);
            this.Name = "EditorPropertyCopier";
            this.Size = new System.Drawing.Size(123, 47);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CopyButton;
        private System.Windows.Forms.Button PasteButton;
    }
}
