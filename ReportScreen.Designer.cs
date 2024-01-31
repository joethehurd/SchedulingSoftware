namespace SchedulingSoftware
{
    partial class ReportScreen
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ReportOneButton = new System.Windows.Forms.Button();
            this.ReportTwoButton = new System.Windows.Forms.Button();
            this.ReportThreeButton = new System.Windows.Forms.Button();
            this.MenuButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.ReportOneButton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ReportTwoButton, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ReportThreeButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.MenuButton, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(878, 457);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // ReportOneButton
            // 
            this.ReportOneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReportOneButton.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReportOneButton.Location = new System.Drawing.Point(25, 139);
            this.ReportOneButton.Margin = new System.Windows.Forms.Padding(25);
            this.ReportOneButton.Name = "ReportOneButton";
            this.ReportOneButton.Size = new System.Drawing.Size(242, 178);
            this.ReportOneButton.TabIndex = 0;
            this.ReportOneButton.Text = "# of Appointment\r\nTypes by Month";
            this.ReportOneButton.UseVisualStyleBackColor = true;
            this.ReportOneButton.Click += new System.EventHandler(this.ReportOneButton_Click);
            // 
            // ReportTwoButton
            // 
            this.ReportTwoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReportTwoButton.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReportTwoButton.Location = new System.Drawing.Point(317, 139);
            this.ReportTwoButton.Margin = new System.Windows.Forms.Padding(25);
            this.ReportTwoButton.Name = "ReportTwoButton";
            this.ReportTwoButton.Size = new System.Drawing.Size(242, 178);
            this.ReportTwoButton.TabIndex = 1;
            this.ReportTwoButton.Text = "User Schedules";
            this.ReportTwoButton.UseVisualStyleBackColor = true;
            this.ReportTwoButton.Click += new System.EventHandler(this.ReportTwoButton_Click);
            // 
            // ReportThreeButton
            // 
            this.ReportThreeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReportThreeButton.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReportThreeButton.Location = new System.Drawing.Point(609, 139);
            this.ReportThreeButton.Margin = new System.Windows.Forms.Padding(25);
            this.ReportThreeButton.Name = "ReportThreeButton";
            this.ReportThreeButton.Size = new System.Drawing.Size(244, 178);
            this.ReportThreeButton.TabIndex = 2;
            this.ReportThreeButton.Text = "# of Customer\r\nAppointments\r\nBy Month";
            this.ReportThreeButton.UseVisualStyleBackColor = true;
            this.ReportThreeButton.Click += new System.EventHandler(this.ReportThreeButton_Click);
            // 
            // MenuButton
            // 
            this.MenuButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MenuButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MenuButton.Location = new System.Drawing.Point(783, 25);
            this.MenuButton.Margin = new System.Windows.Forms.Padding(3, 25, 25, 3);
            this.MenuButton.Name = "MenuButton";
            this.MenuButton.Size = new System.Drawing.Size(70, 35);
            this.MenuButton.TabIndex = 3;
            this.MenuButton.Text = "Menu";
            this.MenuButton.UseVisualStyleBackColor = true;
            this.MenuButton.Click += new System.EventHandler(this.MenuButton_Click);
            // 
            // ReportScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 457);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ReportScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reports";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button ReportOneButton;
        private System.Windows.Forms.Button ReportTwoButton;
        private System.Windows.Forms.Button ReportThreeButton;
        private System.Windows.Forms.Button MenuButton;
    }
}