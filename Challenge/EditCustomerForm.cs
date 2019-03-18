using Challenge.Commands;
using Challenge.Entities;
using Challenge.Queries;
using MediatR;
using System;
using System.Windows.Forms;

namespace Challenge
{
    public partial class EditCustomerForm : Form
    {
        private readonly IMediator _mediator;
        private Customer Customer { get; set; }
        public Guid CustomerId { get; set; }

        public EditCustomerForm(IMediator mediator)
        {
            InitializeComponent();
            _mediator = mediator;
        }

        private async void bttnSave_Click(object sender, EventArgs e)
        {
            var result = await _mediator.Send(new CustomerUpdates.Command(CustomerId, txtFirstName.Text, txtLastName.Text));

            if (result.IsSuccess)
            {
                Close();
            }
            else
            {
                MessageBox.Show(result.Error);
            }
        }

        private async void EditCustomerForm_Load(object sender, EventArgs e)
        {
            Customer = await _mediator.Send(new CustomerQueries.CustomerQuery(CustomerId));

            txtFirstName.Text = Customer.FirstName;
            txtLastName.Text = Customer.LastName;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
