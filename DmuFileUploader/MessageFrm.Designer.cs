
namespace DmuFileUploader
{
    partial class MessageFrm
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
            this.MessageLbl = new System.Windows.Forms.Label();
            this.DetailtTxt = new System.Windows.Forms.TextBox();
            this.DetailsLbl = new System.Windows.Forms.Label();
            this.OkBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MessageLbl
            // 
            this.MessageLbl.AutoSize = true;
            this.MessageLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MessageLbl.Location = new System.Drawing.Point(20, 39);
            this.MessageLbl.Name = "MessageLbl";
            this.MessageLbl.Size = new System.Drawing.Size(84, 20);
            this.MessageLbl.TabIndex = 0;
            this.MessageLbl.Text = "Message";
            // 
            // DetailtTxt
            // 
            this.DetailtTxt.Location = new System.Drawing.Point(27, 111);
            this.DetailtTxt.Multiline = true;
            this.DetailtTxt.Name = "DetailtTxt";
            this.DetailtTxt.Size = new System.Drawing.Size(625, 274);
            this.DetailtTxt.TabIndex = 1;
            // 
            // DetailsLbl
            // 
            this.DetailsLbl.AutoSize = true;
            this.DetailsLbl.Location = new System.Drawing.Point(24, 86);
            this.DetailsLbl.Name = "DetailsLbl";
            this.DetailsLbl.Size = new System.Drawing.Size(55, 17);
            this.DetailsLbl.TabIndex = 2;
            this.DetailsLbl.Text = "Details:";
            // 
            // OkBtn
            // 
            this.OkBtn.Location = new System.Drawing.Point(527, 406);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(125, 38);
            this.OkBtn.TabIndex = 3;
            this.OkBtn.Text = "&Ok";
            this.OkBtn.UseVisualStyleBackColor = true;
            // 
            // MessageFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 468);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.DetailsLbl);
            this.Controls.Add(this.DetailtTxt);
            this.Controls.Add(this.MessageLbl);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageFrm";
            this.Text = "MessageFrm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MessageLbl;
        private System.Windows.Forms.TextBox DetailtTxt;
        private System.Windows.Forms.Label DetailsLbl;
        private System.Windows.Forms.Button OkBtn;
    }
}