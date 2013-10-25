namespace DXFImporter
{
    partial class LineEdit
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxStartX = new System.Windows.Forms.TextBox();
            this.textBoxStartY = new System.Windows.Forms.TextBox();
            this.textBoxEndX = new System.Windows.Forms.TextBox();
            this.textBoxEndY = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "起点X坐标：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "起点Y坐标：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "终点X坐标：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(63, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "终点Y坐标：";
            // 
            // textBoxStartX
            // 
            this.textBoxStartX.Location = new System.Drawing.Point(146, 57);
            this.textBoxStartX.Name = "textBoxStartX";
            this.textBoxStartX.Size = new System.Drawing.Size(78, 21);
            this.textBoxStartX.TabIndex = 0;
            // 
            // textBoxStartY
            // 
            this.textBoxStartY.Location = new System.Drawing.Point(146, 86);
            this.textBoxStartY.Name = "textBoxStartY";
            this.textBoxStartY.Size = new System.Drawing.Size(78, 21);
            this.textBoxStartY.TabIndex = 1;
            // 
            // textBoxEndX
            // 
            this.textBoxEndX.Location = new System.Drawing.Point(146, 115);
            this.textBoxEndX.Name = "textBoxEndX";
            this.textBoxEndX.Size = new System.Drawing.Size(78, 21);
            this.textBoxEndX.TabIndex = 2;
            // 
            // textBoxEndY
            // 
            this.textBoxEndY.Location = new System.Drawing.Point(146, 144);
            this.textBoxEndY.Name = "textBoxEndY";
            this.textBoxEndY.Size = new System.Drawing.Size(78, 21);
            this.textBoxEndY.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(17, 210);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 25);
            this.button1.TabIndex = 6;
            this.button1.Text = "改变方向";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(110, 210);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(69, 25);
            this.button2.TabIndex = 4;
            this.button2.Text = "确认";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.Location = new System.Drawing.Point(203, 210);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(69, 25);
            this.button3.TabIndex = 5;
            this.button3.Text = "取消";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // LineEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxEndY);
            this.Controls.Add(this.textBoxEndX);
            this.Controls.Add(this.textBoxStartY);
            this.Controls.Add(this.textBoxStartX);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(500, 200);
            this.Name = "LineEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "LineEdit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxStartX;
        private System.Windows.Forms.TextBox textBoxStartY;
        private System.Windows.Forms.TextBox textBoxEndX;
        private System.Windows.Forms.TextBox textBoxEndY;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}