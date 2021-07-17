
namespace DmuFileUploader
{
    partial class LoginFrm
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
            this.UrlLbl = new System.Windows.Forms.Label();
            this.UserNameLbl = new System.Windows.Forms.Label();
            this.PasswordLbl = new System.Windows.Forms.Label();
            this.UrlTxt = new System.Windows.Forms.TextBox();
            this.UsernameTxt = new System.Windows.Forms.TextBox();
            this.PasswordTxt = new System.Windows.Forms.TextBox();
            this.LoginBtn = new System.Windows.Forms.Button();
            this.CredentialsGrb = new System.Windows.Forms.GroupBox();
            this.DynamicsConnectionLbl = new System.Windows.Forms.Label();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.CredentialsGrb.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // UrlLbl
            // 
            this.UrlLbl.AutoSize = true;
            this.UrlLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UrlLbl.Location = new System.Drawing.Point(24, 48);
            this.UrlLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UrlLbl.Name = "UrlLbl";
            this.UrlLbl.Size = new System.Drawing.Size(48, 20);
            this.UrlLbl.TabIndex = 0;
            this.UrlLbl.Text = "URL:";
            // 
            // UserNameLbl
            // 
            this.UserNameLbl.AutoSize = true;
            this.UserNameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserNameLbl.Location = new System.Drawing.Point(24, 85);
            this.UserNameLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UserNameLbl.Name = "UserNameLbl";
            this.UserNameLbl.Size = new System.Drawing.Size(99, 20);
            this.UserNameLbl.TabIndex = 2;
            this.UserNameLbl.Text = "User Name:";
            // 
            // PasswordLbl
            // 
            this.PasswordLbl.AutoSize = true;
            this.PasswordLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordLbl.Location = new System.Drawing.Point(24, 122);
            this.PasswordLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PasswordLbl.Name = "PasswordLbl";
            this.PasswordLbl.Size = new System.Drawing.Size(88, 20);
            this.PasswordLbl.TabIndex = 4;
            this.PasswordLbl.Text = "Password:";
            // 
            // UrlTxt
            // 
            this.UrlTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UrlTxt.Location = new System.Drawing.Point(171, 44);
            this.UrlTxt.Margin = new System.Windows.Forms.Padding(4);
            this.UrlTxt.Name = "UrlTxt";
            this.UrlTxt.Size = new System.Drawing.Size(532, 26);
            this.UrlTxt.TabIndex = 1;
            this.UrlTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UrlTxt_KeyDown);
            // 
            // UsernameTxt
            // 
            this.UsernameTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameTxt.Location = new System.Drawing.Point(171, 81);
            this.UsernameTxt.Margin = new System.Windows.Forms.Padding(4);
            this.UsernameTxt.Name = "UsernameTxt";
            this.UsernameTxt.Size = new System.Drawing.Size(532, 26);
            this.UsernameTxt.TabIndex = 3;
            this.UsernameTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UsernameTxt_KeyDown);
            // 
            // PasswordTxt
            // 
            this.PasswordTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordTxt.Location = new System.Drawing.Point(171, 118);
            this.PasswordTxt.Margin = new System.Windows.Forms.Padding(4);
            this.PasswordTxt.Name = "PasswordTxt";
            this.PasswordTxt.PasswordChar = '*';
            this.PasswordTxt.Size = new System.Drawing.Size(532, 26);
            this.PasswordTxt.TabIndex = 5;
            this.PasswordTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasswordTxt_KeyDown);
            // 
            // LoginBtn
            // 
            this.LoginBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginBtn.Location = new System.Drawing.Point(623, 349);
            this.LoginBtn.Margin = new System.Windows.Forms.Padding(4);
            this.LoginBtn.Name = "LoginBtn";
            this.LoginBtn.Size = new System.Drawing.Size(133, 37);
            this.LoginBtn.TabIndex = 3;
            this.LoginBtn.Text = "&Login";
            this.LoginBtn.UseVisualStyleBackColor = true;
            this.LoginBtn.Click += new System.EventHandler(this.LoginBtn_Click);
            // 
            // CredentialsGrb
            // 
            this.CredentialsGrb.Controls.Add(this.UrlTxt);
            this.CredentialsGrb.Controls.Add(this.UrlLbl);
            this.CredentialsGrb.Controls.Add(this.PasswordTxt);
            this.CredentialsGrb.Controls.Add(this.UserNameLbl);
            this.CredentialsGrb.Controls.Add(this.UsernameTxt);
            this.CredentialsGrb.Controls.Add(this.PasswordLbl);
            this.CredentialsGrb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CredentialsGrb.Location = new System.Drawing.Point(23, 156);
            this.CredentialsGrb.Margin = new System.Windows.Forms.Padding(4);
            this.CredentialsGrb.Name = "CredentialsGrb";
            this.CredentialsGrb.Padding = new System.Windows.Forms.Padding(4);
            this.CredentialsGrb.Size = new System.Drawing.Size(733, 185);
            this.CredentialsGrb.TabIndex = 1;
            this.CredentialsGrb.TabStop = false;
            this.CredentialsGrb.Text = "Credentials";
            // 
            // DynamicsConnectionLbl
            // 
            this.DynamicsConnectionLbl.AutoSize = true;
            this.DynamicsConnectionLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DynamicsConnectionLbl.Location = new System.Drawing.Point(16, 31);
            this.DynamicsConnectionLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DynamicsConnectionLbl.Name = "DynamicsConnectionLbl";
            this.DynamicsConnectionLbl.Size = new System.Drawing.Size(344, 39);
            this.DynamicsConnectionLbl.TabIndex = 0;
            this.DynamicsConnectionLbl.Text = "Dynamics 365 Login";
            // 
            // StatusStrip
            // 
            this.StatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgressBar});
            this.StatusStrip.Location = new System.Drawing.Point(0, 410);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(776, 24);
            this.StatusStrip.TabIndex = 4;
            this.StatusStrip.Text = "StatusStrip";
            this.StatusStrip.Resize += new System.EventHandler(this.StatusStrip_Resize);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // LoginFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 434);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.DynamicsConnectionLbl);
            this.Controls.Add(this.CredentialsGrb);
            this.Controls.Add(this.LoginBtn);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(794, 481);
            this.MinimumSize = new System.Drawing.Size(794, 481);
            this.Name = "LoginFrm";
            this.Text = "Login";
            this.CredentialsGrb.ResumeLayout(false);
            this.CredentialsGrb.PerformLayout();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UrlLbl;
        private System.Windows.Forms.Label UserNameLbl;
        private System.Windows.Forms.Label PasswordLbl;
        private System.Windows.Forms.TextBox UrlTxt;
        private System.Windows.Forms.TextBox UsernameTxt;
        private System.Windows.Forms.TextBox PasswordTxt;
        private System.Windows.Forms.Button LoginBtn;
        private System.Windows.Forms.GroupBox CredentialsGrb;
        private System.Windows.Forms.Label DynamicsConnectionLbl;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar;
    }
}