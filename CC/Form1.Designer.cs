namespace CC
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.上位机配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.下位机配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.上位机配置ToolStripMenuItem,
            this.下位机配置ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(822, 24);
            this.menuStrip1.TabIndex = 19;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 上位机配置ToolStripMenuItem
            // 
            this.上位机配置ToolStripMenuItem.Name = "上位机配置ToolStripMenuItem";
            this.上位机配置ToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.上位机配置ToolStripMenuItem.Text = "上位机配置";
            this.上位机配置ToolStripMenuItem.Click += new System.EventHandler(this.上位机配置ToolStripMenuItem_Click);
            // 
            // 下位机配置ToolStripMenuItem
            // 
            this.下位机配置ToolStripMenuItem.Name = "下位机配置ToolStripMenuItem";
            this.下位机配置ToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.下位机配置ToolStripMenuItem.Text = "下位机配置";
            this.下位机配置ToolStripMenuItem.Click += new System.EventHandler(this.下位机配置ToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CC.Properties.Resources.bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(822, 686);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 上位机配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 下位机配置ToolStripMenuItem;
    }
}

