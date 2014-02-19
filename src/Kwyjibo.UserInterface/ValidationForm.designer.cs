namespace Kwyjibo.UserInterface.Forms
{
    partial class ValidationForm
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
            this.logText = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // logText
            // 
            this.logText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logText.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logText.ForeColor = System.Drawing.Color.DimGray;
            this.logText.Location = new System.Drawing.Point(0, 0);
            this.logText.Margin = new System.Windows.Forms.Padding(30);
            this.logText.Name = "logText";
            this.logText.Size = new System.Drawing.Size(988, 570);
            this.logText.TabIndex = 1;
            this.logText.Text = "";
            this.logText.WordWrap = false;
            // 
            // ValidationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(988, 570);
            this.Controls.Add(this.logText);
            this.Name = "ValidationForm";
            this.Text = "Validation";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox logText;

    }
}