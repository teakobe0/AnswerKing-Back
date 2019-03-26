namespace PgDataToSql
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnexport = new System.Windows.Forms.Button();
            this.lvFile = new System.Windows.Forms.ListView();
            this.lblfile = new System.Windows.Forms.Label();
            this.lbldb = new System.Windows.Forms.Label();
            this.lblmsg = new System.Windows.Forms.Label();
            this.lvDb = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnrefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnexport
            // 
            this.btnexport.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnexport.Location = new System.Drawing.Point(622, 158);
            this.btnexport.Name = "btnexport";
            this.btnexport.Size = new System.Drawing.Size(154, 79);
            this.btnexport.TabIndex = 0;
            this.btnexport.Text = "export";
            this.btnexport.UseVisualStyleBackColor = true;
            this.btnexport.Click += new System.EventHandler(this.btnexport_Click);
            // 
            // lvFile
            // 
            this.lvFile.BackColor = System.Drawing.SystemColors.Window;
            this.lvFile.HideSelection = false;
            this.lvFile.Location = new System.Drawing.Point(10, 50);
            this.lvFile.Name = "lvFile";
            this.lvFile.Size = new System.Drawing.Size(273, 388);
            this.lvFile.TabIndex = 2;
            this.lvFile.UseCompatibleStateImageBehavior = false;
            this.lvFile.View = System.Windows.Forms.View.List;
            this.lvFile.Click += new System.EventHandler(this.lvFile_Click);
            // 
            // lblfile
            // 
            this.lblfile.AutoSize = true;
            this.lblfile.Location = new System.Drawing.Point(44, 23);
            this.lblfile.Name = "lblfile";
            this.lblfile.Size = new System.Drawing.Size(0, 12);
            this.lblfile.TabIndex = 4;
            // 
            // lbldb
            // 
            this.lbldb.AutoSize = true;
            this.lbldb.Location = new System.Drawing.Point(374, 23);
            this.lbldb.Name = "lbldb";
            this.lbldb.Size = new System.Drawing.Size(0, 12);
            this.lbldb.TabIndex = 4;
            // 
            // lblmsg
            // 
            this.lblmsg.AutoSize = true;
            this.lblmsg.Location = new System.Drawing.Point(620, 274);
            this.lblmsg.Name = "lblmsg";
            this.lblmsg.Size = new System.Drawing.Size(65, 12);
            this.lblmsg.TabIndex = 4;
            this.lblmsg.Text = "提示信息：";
            // 
            // lvDb
            // 
            this.lvDb.BackColor = System.Drawing.SystemColors.Window;
            this.lvDb.HideSelection = false;
            this.lvDb.Location = new System.Drawing.Point(317, 50);
            this.lvDb.Name = "lvDb";
            this.lvDb.Size = new System.Drawing.Size(275, 388);
            this.lvDb.TabIndex = 5;
            this.lvDb.UseCompatibleStateImageBehavior = false;
            this.lvDb.View = System.Windows.Forms.View.List;
            this.lvDb.Click += new System.EventHandler(this.lvDb_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "文件:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(315, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "数据库表:";
            // 
            // btnrefresh
            // 
            this.btnrefresh.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnrefresh.Location = new System.Drawing.Point(641, 99);
            this.btnrefresh.Name = "btnrefresh";
            this.btnrefresh.Size = new System.Drawing.Size(103, 33);
            this.btnrefresh.TabIndex = 0;
            this.btnrefresh.Text = "刷新选择列表";
            this.btnrefresh.UseVisualStyleBackColor = true;
            this.btnrefresh.Click += new System.EventHandler(this.btnrefresh_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lvDb);
            this.Controls.Add(this.lblmsg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbldb);
            this.Controls.Add(this.lblfile);
            this.Controls.Add(this.lvFile);
            this.Controls.Add(this.btnrefresh);
            this.Controls.Add(this.btnexport);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnexport;
        private System.Windows.Forms.ListView lvFile;
        private System.Windows.Forms.Label lblfile;
        private System.Windows.Forms.Label lbldb;
        private System.Windows.Forms.Label lblmsg;
        private System.Windows.Forms.ListView lvDb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnrefresh;
    }
}

