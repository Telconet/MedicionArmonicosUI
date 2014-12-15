namespace MedicionArmonicosUI
{
    partial class Form1
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
            this.opcionesMuestreo = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.diasRegistrosCmbBx = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numeroTarjetasCmbBx = new System.Windows.Forms.ComboBox();
            this.medicionContinuaChckBx = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.transformadorTxtBx = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.divisorVoltajeTxtBx = new System.Windows.Forms.TextBox();
            this.ventanaCmbBx = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tiempoMedicionChckBx = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.rutaArchivoTxtBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.iniciarMuestroBttn = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ayudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acercaDeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tarjeta0FrecTxtBx = new System.Windows.Forms.TextBox();
            this.tarjeta1FrecTxtBx = new System.Windows.Forms.TextBox();
            this.tarjeta2FrecTxtBx = new System.Windows.Forms.TextBox();
            this.tarjeta3FrecTxtBx = new System.Windows.Forms.TextBox();
            this.tarjeta0CanalTxtBx = new System.Windows.Forms.TextBox();
            this.tarjeta1CanalTxtBx = new System.Windows.Forms.TextBox();
            this.tarjeta2CanalTxtBx = new System.Windows.Forms.TextBox();
            this.tarjeta3CanalTxtBx = new System.Windows.Forms.TextBox();
            this.opcionesMuestreo.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // opcionesMuestreo
            // 
            this.opcionesMuestreo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.opcionesMuestreo.Controls.Add(this.label1);
            this.opcionesMuestreo.Controls.Add(this.diasRegistrosCmbBx);
            this.opcionesMuestreo.Controls.Add(this.label7);
            this.opcionesMuestreo.Controls.Add(this.numeroTarjetasCmbBx);
            this.opcionesMuestreo.Controls.Add(this.medicionContinuaChckBx);
            this.opcionesMuestreo.Controls.Add(this.label5);
            this.opcionesMuestreo.Controls.Add(this.transformadorTxtBx);
            this.opcionesMuestreo.Controls.Add(this.label6);
            this.opcionesMuestreo.Controls.Add(this.divisorVoltajeTxtBx);
            this.opcionesMuestreo.Controls.Add(this.ventanaCmbBx);
            this.opcionesMuestreo.Controls.Add(this.label4);
            this.opcionesMuestreo.Controls.Add(this.tiempoMedicionChckBx);
            this.opcionesMuestreo.Controls.Add(this.button2);
            this.opcionesMuestreo.Controls.Add(this.rutaArchivoTxtBox);
            this.opcionesMuestreo.Controls.Add(this.label3);
            this.opcionesMuestreo.Location = new System.Drawing.Point(12, 29);
            this.opcionesMuestreo.Name = "opcionesMuestreo";
            this.opcionesMuestreo.Size = new System.Drawing.Size(456, 326);
            this.opcionesMuestreo.TabIndex = 0;
            this.opcionesMuestreo.TabStop = false;
            this.opcionesMuestreo.Text = "Opciones Generales de Muestreo";
            this.opcionesMuestreo.Enter += new System.EventHandler(this.opcionesMuestreo_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Días  de registros a mantener";
            // 
            // diasRegistrosCmbBx
            // 
            this.diasRegistrosCmbBx.FormattingEnabled = true;
            this.diasRegistrosCmbBx.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7"});
            this.diasRegistrosCmbBx.Location = new System.Drawing.Point(158, 144);
            this.diasRegistrosCmbBx.Name = "diasRegistrosCmbBx";
            this.diasRegistrosCmbBx.Size = new System.Drawing.Size(121, 21);
            this.diasRegistrosCmbBx.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(52, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Numero de Tarjetas";
            // 
            // numeroTarjetasCmbBx
            // 
            this.numeroTarjetasCmbBx.FormattingEnabled = true;
            this.numeroTarjetasCmbBx.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.numeroTarjetasCmbBx.Location = new System.Drawing.Point(158, 61);
            this.numeroTarjetasCmbBx.Name = "numeroTarjetasCmbBx";
            this.numeroTarjetasCmbBx.Size = new System.Drawing.Size(121, 21);
            this.numeroTarjetasCmbBx.TabIndex = 18;
            this.numeroTarjetasCmbBx.TextChanged += new System.EventHandler(this.numeroTarjetasCmbBx_TextChanged);
            // 
            // medicionContinuaChckBx
            // 
            this.medicionContinuaChckBx.AutoSize = true;
            this.medicionContinuaChckBx.Location = new System.Drawing.Point(156, 231);
            this.medicionContinuaChckBx.Name = "medicionContinuaChckBx";
            this.medicionContinuaChckBx.Size = new System.Drawing.Size(137, 17);
            this.medicionContinuaChckBx.TabIndex = 17;
            this.medicionContinuaChckBx.Text = "¿Medir continuamente?";
            this.medicionContinuaChckBx.UseVisualStyleBackColor = true;
            this.medicionContinuaChckBx.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(135, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Tasa vueltas transformador";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // transformadorTxtBx
            // 
            this.transformadorTxtBx.Location = new System.Drawing.Point(159, 117);
            this.transformadorTxtBx.Name = "transformadorTxtBx";
            this.transformadorTxtBx.Size = new System.Drawing.Size(187, 20);
            this.transformadorTxtBx.TabIndex = 15;
            this.transformadorTxtBx.Text = "1.0";
            this.transformadorTxtBx.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(64, 94);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Divisor de voltaje";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // divisorVoltajeTxtBx
            // 
            this.divisorVoltajeTxtBx.Location = new System.Drawing.Point(158, 91);
            this.divisorVoltajeTxtBx.Name = "divisorVoltajeTxtBx";
            this.divisorVoltajeTxtBx.Size = new System.Drawing.Size(187, 20);
            this.divisorVoltajeTxtBx.TabIndex = 13;
            this.divisorVoltajeTxtBx.Text = "0.004628775";
            this.divisorVoltajeTxtBx.TextChanged += new System.EventHandler(this.textBox2_TextChanged_1);
            // 
            // ventanaCmbBx
            // 
            this.ventanaCmbBx.FormattingEnabled = true;
            this.ventanaCmbBx.Items.AddRange(new object[] {
            "1 minuto",
            "5 minutos",
            "10 minutos",
            "15 minutos",
            "30 minutos",
            "60 minutos"});
            this.ventanaCmbBx.Location = new System.Drawing.Point(158, 32);
            this.ventanaCmbBx.Name = "ventanaCmbBx";
            this.ventanaCmbBx.Size = new System.Drawing.Size(121, 21);
            this.ventanaCmbBx.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(47, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Ventana de medicion";
            // 
            // tiempoMedicionChckBx
            // 
            this.tiempoMedicionChckBx.AutoSize = true;
            this.tiempoMedicionChckBx.Location = new System.Drawing.Point(157, 208);
            this.tiempoMedicionChckBx.Name = "tiempoMedicionChckBx";
            this.tiempoMedicionChckBx.Size = new System.Drawing.Size(182, 17);
            this.tiempoMedicionChckBx.TabIndex = 9;
            this.tiempoMedicionChckBx.Text = "¿Almacenar tiempo de medición?";
            this.tiempoMedicionChckBx.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(349, 172);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(30, 20);
            this.button2.TabIndex = 7;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // rutaArchivoTxtBox
            // 
            this.rutaArchivoTxtBox.Location = new System.Drawing.Point(156, 172);
            this.rutaArchivoTxtBox.Name = "rutaArchivoTxtBox";
            this.rutaArchivoTxtBox.ReadOnly = true;
            this.rutaArchivoTxtBox.Size = new System.Drawing.Size(187, 20);
            this.rutaArchivoTxtBox.TabIndex = 6;
            this.rutaArchivoTxtBox.Text = "C:\\Users\\Eduardo\\Documents";
            this.rutaArchivoTxtBox.TextChanged += new System.EventHandler(this.rutaArchivoTxtBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(49, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Carpeta de registros ";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // iniciarMuestroBttn
            // 
            this.iniciarMuestroBttn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.iniciarMuestroBttn.Location = new System.Drawing.Point(275, 276);
            this.iniciarMuestroBttn.Name = "iniciarMuestroBttn";
            this.iniciarMuestroBttn.Size = new System.Drawing.Size(108, 35);
            this.iniciarMuestroBttn.TabIndex = 4;
            this.iniciarMuestroBttn.Text = "Inciar Muestreo";
            this.iniciarMuestroBttn.UseVisualStyleBackColor = true;
            this.iniciarMuestroBttn.Click += new System.EventHandler(this.button1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.ayudaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(816, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.salirToolStripMenuItem});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // ayudaToolStripMenuItem
            // 
            this.ayudaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.acercaDeToolStripMenuItem});
            this.ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            this.ayudaToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.ayudaToolStripMenuItem.Text = "Ayuda";
            // 
            // acercaDeToolStripMenuItem
            // 
            this.acercaDeToolStripMenuItem.Name = "acercaDeToolStripMenuItem";
            this.acercaDeToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.acercaDeToolStripMenuItem.Text = "Acerca de...";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Controls.Add(this.iniciarMuestroBttn);
            this.groupBox1.Location = new System.Drawing.Point(408, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(396, 326);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Opciones de Tarjetas";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.51771F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.14986F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.05994F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 136F));
            this.tableLayoutPanel1.Controls.Add(this.label8, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label9, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tarjeta0FrecTxtBx, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tarjeta1FrecTxtBx, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tarjeta2FrecTxtBx, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.tarjeta3FrecTxtBx, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.tarjeta0CanalTxtBx, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tarjeta1CanalTxtBx, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.tarjeta2CanalTxtBx, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.tarjeta3CanalTxtBx, 2, 4);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(14, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.10714F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.89286F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(369, 240);
            this.tableLayoutPanel1.TabIndex = 5;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(146, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Frecuencia:";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(277, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Canal 0 a:";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(32, 52);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Tarjeta 1";
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(32, 99);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "Tarjeta 2";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(32, 152);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(49, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "Tarjeta 3";
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(32, 205);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(49, 13);
            this.label13.TabIndex = 5;
            this.label13.Text = "Tarjeta 4";
            // 
            // tarjeta0FrecTxtBx
            // 
            this.tarjeta0FrecTxtBx.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tarjeta0FrecTxtBx.Location = new System.Drawing.Point(117, 49);
            this.tarjeta0FrecTxtBx.Name = "tarjeta0FrecTxtBx";
            this.tarjeta0FrecTxtBx.Size = new System.Drawing.Size(117, 20);
            this.tarjeta0FrecTxtBx.TabIndex = 6;
            // 
            // tarjeta1FrecTxtBx
            // 
            this.tarjeta1FrecTxtBx.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tarjeta1FrecTxtBx.Location = new System.Drawing.Point(117, 96);
            this.tarjeta1FrecTxtBx.Name = "tarjeta1FrecTxtBx";
            this.tarjeta1FrecTxtBx.Size = new System.Drawing.Size(117, 20);
            this.tarjeta1FrecTxtBx.TabIndex = 7;
            // 
            // tarjeta2FrecTxtBx
            // 
            this.tarjeta2FrecTxtBx.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tarjeta2FrecTxtBx.Location = new System.Drawing.Point(117, 148);
            this.tarjeta2FrecTxtBx.Name = "tarjeta2FrecTxtBx";
            this.tarjeta2FrecTxtBx.Size = new System.Drawing.Size(117, 20);
            this.tarjeta2FrecTxtBx.TabIndex = 8;
            // 
            // tarjeta3FrecTxtBx
            // 
            this.tarjeta3FrecTxtBx.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tarjeta3FrecTxtBx.Location = new System.Drawing.Point(117, 201);
            this.tarjeta3FrecTxtBx.Name = "tarjeta3FrecTxtBx";
            this.tarjeta3FrecTxtBx.Size = new System.Drawing.Size(117, 20);
            this.tarjeta3FrecTxtBx.TabIndex = 9;
            // 
            // tarjeta0CanalTxtBx
            // 
            this.tarjeta0CanalTxtBx.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tarjeta0CanalTxtBx.Location = new System.Drawing.Point(246, 49);
            this.tarjeta0CanalTxtBx.Name = "tarjeta0CanalTxtBx";
            this.tarjeta0CanalTxtBx.Size = new System.Drawing.Size(116, 20);
            this.tarjeta0CanalTxtBx.TabIndex = 10;
            // 
            // tarjeta1CanalTxtBx
            // 
            this.tarjeta1CanalTxtBx.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tarjeta1CanalTxtBx.Location = new System.Drawing.Point(246, 96);
            this.tarjeta1CanalTxtBx.Name = "tarjeta1CanalTxtBx";
            this.tarjeta1CanalTxtBx.Size = new System.Drawing.Size(116, 20);
            this.tarjeta1CanalTxtBx.TabIndex = 11;
            // 
            // tarjeta2CanalTxtBx
            // 
            this.tarjeta2CanalTxtBx.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tarjeta2CanalTxtBx.Location = new System.Drawing.Point(246, 148);
            this.tarjeta2CanalTxtBx.Name = "tarjeta2CanalTxtBx";
            this.tarjeta2CanalTxtBx.Size = new System.Drawing.Size(116, 20);
            this.tarjeta2CanalTxtBx.TabIndex = 12;
            // 
            // tarjeta3CanalTxtBx
            // 
            this.tarjeta3CanalTxtBx.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tarjeta3CanalTxtBx.Location = new System.Drawing.Point(246, 201);
            this.tarjeta3CanalTxtBx.Name = "tarjeta3CanalTxtBx";
            this.tarjeta3CanalTxtBx.Size = new System.Drawing.Size(116, 20);
            this.tarjeta3CanalTxtBx.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 367);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.opcionesMuestreo);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Sistema de Medición de Armónicos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.opcionesMuestreo.ResumeLayout(false);
            this.opcionesMuestreo.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox opcionesMuestreo;
        private System.Windows.Forms.Button iniciarMuestroBttn;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ayudaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem acercaDeToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox rutaArchivoTxtBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox tiempoMedicionChckBx;
        private System.Windows.Forms.ComboBox ventanaCmbBx;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox transformadorTxtBx;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox divisorVoltajeTxtBx;
        private System.Windows.Forms.CheckBox medicionContinuaChckBx;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox numeroTarjetasCmbBx;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tarjeta0FrecTxtBx;
        private System.Windows.Forms.TextBox tarjeta1FrecTxtBx;
        private System.Windows.Forms.TextBox tarjeta2FrecTxtBx;
        private System.Windows.Forms.TextBox tarjeta3FrecTxtBx;
        private System.Windows.Forms.TextBox tarjeta0CanalTxtBx;
        private System.Windows.Forms.TextBox tarjeta1CanalTxtBx;
        private System.Windows.Forms.TextBox tarjeta2CanalTxtBx;
        private System.Windows.Forms.TextBox tarjeta3CanalTxtBx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox diasRegistrosCmbBx;
    }
}

