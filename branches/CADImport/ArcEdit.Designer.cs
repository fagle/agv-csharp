namespace AGV
{
    partial class ArcEdit
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxSweepAngle = new System.Windows.Forms.TextBox();
            this.textBoxEndAngle = new System.Windows.Forms.TextBox();
            this.textBoxStartAngle = new System.Windows.Forms.TextBox();
            this.textBoxOy = new System.Windows.Forms.TextBox();
            this.textBoxOx = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxRadius = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxNo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(112, 215);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(63, 24);
            this.button1.TabIndex = 6;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(202, 215);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(63, 24);
            this.button2.TabIndex = 7;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // textBoxSweepAngle
            // 
            this.textBoxSweepAngle.Location = new System.Drawing.Point(164, 188);
            this.textBoxSweepAngle.Name = "textBoxSweepAngle";
            this.textBoxSweepAngle.Size = new System.Drawing.Size(71, 21);
            this.textBoxSweepAngle.TabIndex = 5;
            // 
            // textBoxEndAngle
            // 
            this.textBoxEndAngle.Location = new System.Drawing.Point(164, 159);
            this.textBoxEndAngle.Name = "textBoxEndAngle";
            this.textBoxEndAngle.Size = new System.Drawing.Size(71, 21);
            this.textBoxEndAngle.TabIndex = 4;
            // 
            // textBoxStartAngle
            // 
            this.textBoxStartAngle.Location = new System.Drawing.Point(164, 130);
            this.textBoxStartAngle.Name = "textBoxStartAngle";
            this.textBoxStartAngle.Size = new System.Drawing.Size(71, 21);
            this.textBoxStartAngle.TabIndex = 3;
            // 
            // textBoxOy
            // 
            this.textBoxOy.Location = new System.Drawing.Point(164, 101);
            this.textBoxOy.Name = "textBoxOy";
            this.textBoxOy.Size = new System.Drawing.Size(71, 21);
            this.textBoxOy.TabIndex = 2;
            // 
            // textBoxOx
            // 
            this.textBoxOx.Location = new System.Drawing.Point(164, 72);
            this.textBoxOx.Name = "textBoxOx";
            this.textBoxOx.Size = new System.Drawing.Size(71, 21);
            this.textBoxOx.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(77, 187);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "扫过角度：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(77, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "结束角度：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(77, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "起始角度：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(77, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "圆心Y坐标：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "圆心X坐标：";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(22, 215);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(63, 24);
            this.button3.TabIndex = 8;
            this.button3.Text = "改变方向";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(77, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "半径：";
            // 
            // textBoxRadius
            // 
            this.textBoxRadius.Location = new System.Drawing.Point(164, 43);
            this.textBoxRadius.Name = "textBoxRadius";
            this.textBoxRadius.Size = new System.Drawing.Size(71, 21);
            this.textBoxRadius.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(77, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "序号：";
            // 
            // textBoxNo
            // 
            this.textBoxNo.Location = new System.Drawing.Point(164, 14);
            this.textBoxNo.Name = "textBoxNo";
            this.textBoxNo.Size = new System.Drawing.Size(71, 21);
            this.textBoxNo.TabIndex = 16;
            // 
            // ArcEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.textBoxNo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxRadius);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBoxOy);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxSweepAngle);
            this.Controls.Add(this.textBoxEndAngle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxStartAngle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxOx);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Location = new System.Drawing.Point(500, 100);
            this.Name = "ArcEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ArcEdit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBoxSweepAngle;
        private System.Windows.Forms.TextBox textBoxEndAngle;
        private System.Windows.Forms.TextBox textBoxStartAngle;
        private System.Windows.Forms.TextBox textBoxOy;
        private System.Windows.Forms.TextBox textBoxOx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxRadius;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxNo;
    }
}