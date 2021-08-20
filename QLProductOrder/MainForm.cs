using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLProductOrder.Model;

namespace QLProductOrder
{
    public partial class MainForm : Form
    {
        dbProductOrder data = new dbProductOrder();
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Updatedgv();
            dateTimePickerStart.Value = data.Invoices.Select(p => p.OrderDate).First();
            dateTimePickerEnd.Value = data.Invoices.Select(p => p.DeliveryDate).First();
        }

        private void cbXemTatCaTrongThang_CheckedChanged(object sender, EventArgs e)
        {
            Updatedgv();
        }

        private void Updatedgv()
        {
            int i = 0;
            int tong = 0;
            dgv.Rows.Clear();

            var q = data.Orders.GroupBy(p => p.Invoice)
            .Select(g => new {
                Invoice = g.Key,
                OrderDate = g.Key.OrderDate,
                DeliveryDate = g.Key.DeliveryDate,
                Sum = g.Sum(p => p.Quantity * p.Price),
            }).ToList();

            if(cbXemTatCaTrongThang.Checked)
            {
                foreach (var order in q)
                {
                    if (order.OrderDate.Month == dateTimePickerStart.Value.Month)
                    {
                        dgv.Rows.Add();
                        dgv.Rows[i].Cells[0].Value = i + 1;
                        dgv.Rows[i].Cells[1].Value = order.Invoice.InvoiceNo;
                        dgv.Rows[i].Cells[2].Value = order.OrderDate.Date;
                        dgv.Rows[i].Cells[3].Value = order.DeliveryDate.Date;
                        dgv.Rows[i].Cells[4].Value = order.Sum;

                        tong += (int)order.Sum;
                        i++;
                    }
                }
                tbTong.Text = tong.ToString();
            }
            else
            {
                foreach (var order in q)
                {
                    if (order.OrderDate >= dateTimePickerStart.Value && order.DeliveryDate <= dateTimePickerEnd.Value)
                    {
                        dgv.Rows.Add();
                        dgv.Rows[i].Cells[0].Value = i + 1;
                        dgv.Rows[i].Cells[1].Value = order.Invoice.InvoiceNo;
                        dgv.Rows[i].Cells[2].Value = order.OrderDate.Date;
                        dgv.Rows[i].Cells[3].Value = order.DeliveryDate.Date;
                        dgv.Rows[i].Cells[4].Value = order.Sum;

                        tong += (int)order.Sum;
                        i++;
                    }
                }
                tbTong.Text = tong.ToString();
            }

        }

        private void dateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {
            Updatedgv();
        }

        private void dateTimePickerEnd_ValueChanged(object sender, EventArgs e)
        {
            Updatedgv();
        }
    }
}
