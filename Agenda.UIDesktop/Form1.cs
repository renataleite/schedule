﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Agenda.UIDesktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            string nome = txtContatoNovo.Text;
            //txtContatoSalvo.Text = nome;

            string strCon = @"Data Source=.\sqlexpress;Initial Catalog=Agenda;Integrated Security=True;";
            string id = Guid.NewGuid().ToString();

            SqlConnection con = new SqlConnection(strCon);
            con.Open();

            string sql = String.Format("insert into Agenda (id, Nome) values ('{0}', '{1}');", id, nome);


            SqlCommand cmd = new SqlCommand(sql, con);

            cmd.ExecuteNonQuery();

            sql = String.Format("select Nome from Contato where Id = '{0}';", id);

            txtContatoSalvo.Text = cmd.ExecuteScalar().ToString();
            
            con.Close();


        }
    }
}
