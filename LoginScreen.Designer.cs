namespace SchedulingSoftware
{
    partial class LoginScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginScreen));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.EnterUsername = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.EnterPassword = new System.Windows.Forms.TextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.LoginButton = new System.Windows.Forms.Button();
            this.UserLoginLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.LoginButton, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.UserLoginLabel, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.UsernameLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.EnterUsername, 0, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // UsernameLabel
            // 
            resources.ApplyResources(this.UsernameLabel, "UsernameLabel");
            this.UsernameLabel.Name = "UsernameLabel";
            // 
            // EnterUsername
            // 
            resources.ApplyResources(this.EnterUsername, "EnterUsername");
            this.EnterUsername.Name = "EnterUsername";
            this.EnterUsername.ShortcutsEnabled = false;
            this.EnterUsername.TextChanged += new System.EventHandler(this.ValidateFields);
            this.EnterUsername.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EnterUsername_KeyPress);
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.EnterPassword, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.PasswordLabel, 0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // EnterPassword
            // 
            resources.ApplyResources(this.EnterPassword, "EnterPassword");
            this.EnterPassword.Name = "EnterPassword";
            this.EnterPassword.ShortcutsEnabled = false;
            this.EnterPassword.TextChanged += new System.EventHandler(this.ValidateFields);
            this.EnterPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EnterPassword_KeyPress);
            // 
            // PasswordLabel
            // 
            resources.ApplyResources(this.PasswordLabel, "PasswordLabel");
            this.PasswordLabel.Name = "PasswordLabel";
            // 
            // LoginButton
            // 
            resources.ApplyResources(this.LoginButton, "LoginButton");
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // UserLoginLabel
            // 
            resources.ApplyResources(this.UserLoginLabel, "UserLoginLabel");
            this.UserLoginLabel.Name = "UserLoginLabel";
            // 
            // LoginScreen
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LoginScreen";
            this.ShowIcon = false;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox EnterUsername;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox EnterPassword;
        private System.Windows.Forms.Button LoginButton;
        public System.Windows.Forms.Label UsernameLabel;
        public System.Windows.Forms.Label PasswordLabel;
        public System.Windows.Forms.Label UserLoginLabel;
    }
}