
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
            this.UserNameTxt = new System.Windows.Forms.TextBox();
            this.PasswordTxt = new System.Windows.Forms.TextBox();
            this.LoginBtn = new System.Windows.Forms.Button();
            this.CredentialsGrb = new System.Windows.Forms.GroupBox();
            this.DynamicsConnectionLbl = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.CredentialsGrb.SuspendLayout();
            this.SuspendLayout();
            // 
            // UrlLbl
            // 
            this.UrlLbl.AutoSize = true;
            this.UrlLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UrlLbl.Location = new System.Drawing.Point(18, 39);
            this.UrlLbl.Name = "UrlLbl";
            this.UrlLbl.Size = new System.Drawing.Size(38, 16);
            this.UrlLbl.TabIndex = 0;
            this.UrlLbl.Text = "URL:";
            // 
            // UserNameLbl
            // 
            this.UserNameLbl.AutoSize = true;
            this.UserNameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserNameLbl.Location = new System.Drawing.Point(18, 69);
            this.UserNameLbl.Name = "UserNameLbl";
            this.UserNameLbl.Size = new System.Drawing.Size(80, 16);
            this.UserNameLbl.TabIndex = 2;
            this.UserNameLbl.Text = "User Name:";
            // 
            // PasswordLbl
            // 
            this.PasswordLbl.AutoSize = true;
            this.PasswordLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordLbl.Location = new System.Drawing.Point(18, 99);
            this.PasswordLbl.Name = "PasswordLbl";
            this.PasswordLbl.Size = new System.Drawing.Size(71, 16);
            this.PasswordLbl.TabIndex = 4;
            this.PasswordLbl.Text = "Password:";
            // 
            // UrlTxt
            // 
            this.UrlTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UrlTxt.Location = new System.Drawing.Point(128, 36);
            this.UrlTxt.Name = "UrlTxt";
            this.UrlTxt.Size = new System.Drawing.Size(400, 22);
            this.UrlTxt.TabIndex = 1;
            // 
            // UserNameTxt
            // 
            this.UserNameTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserNameTxt.Location = new System.Drawing.Point(128, 66);
            this.UserNameTxt.Name = "UserNameTxt";
            this.UserNameTxt.Size = new System.Drawing.Size(400, 22);
            this.UserNameTxt.TabIndex = 3;
            // 
            // PasswordTxt
            // 
            this.PasswordTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordTxt.Location = new System.Drawing.Point(128, 96);
            this.PasswordTxt.Name = "PasswordTxt";
            this.PasswordTxt.PasswordChar = '*';
            this.PasswordTxt.Size = new System.Drawing.Size(400, 22);
            this.PasswordTxt.TabIndex = 5;
            // 
            // LoginBtn
            // 
            this.LoginBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginBtn.Location = new System.Drawing.Point(467, 313);
            this.LoginBtn.Name = "LoginBtn";
            this.LoginBtn.Size = new System.Drawing.Size(100, 30);
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
            this.CredentialsGrb.Controls.Add(this.UserNameTxt);
            this.CredentialsGrb.Controls.Add(this.PasswordLbl);
            this.CredentialsGrb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CredentialsGrb.Location = new System.Drawing.Point(17, 127);
            this.CredentialsGrb.Name = "CredentialsGrb";
            this.CredentialsGrb.Size = new System.Drawing.Size(550, 150);
            this.CredentialsGrb.TabIndex = 1;
            this.CredentialsGrb.TabStop = false;
            this.CredentialsGrb.Text = "Credentials";
            // 
            // DynamicsConnectionLbl
            // 
            this.DynamicsConnectionLbl.AutoSize = true;
            this.DynamicsConnectionLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DynamicsConnectionLbl.Location = new System.Drawing.Point(12, 25);
            this.DynamicsConnectionLbl.Name = "DynamicsConnectionLbl";
            this.DynamicsConnectionLbl.Size = new System.Drawing.Size(298, 31);
            this.DynamicsConnectionLbl.TabIndex = 0;
            this.DynamicsConnectionLbl.Text = "Dynamics Connection";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelBtn.Location = new System.Drawing.Point(361, 313);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(100, 30);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "&Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // LoginFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.DynamicsConnectionLbl);
            this.Controls.Add(this.CredentialsGrb);
            this.Controls.Add(this.LoginBtn);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 400);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "LoginFrm";
            this.Text = "Login";
            this.CredentialsGrb.ResumeLayout(false);
            this.CredentialsGrb.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UrlLbl;
        private System.Windows.Forms.Label UserNameLbl;
        private System.Windows.Forms.Label PasswordLbl;
        private System.Windows.Forms.TextBox UrlTxt;
        private System.Windows.Forms.TextBox UserNameTxt;
        private System.Windows.Forms.TextBox PasswordTxt;
        private System.Windows.Forms.Button LoginBtn;
        private System.Windows.Forms.GroupBox CredentialsGrb;
        private System.Windows.Forms.Label DynamicsConnectionLbl;
        private System.Windows.Forms.Button CancelBtn;
    }
}