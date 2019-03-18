using Autofac;
using Challenge.DTOs;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Challenge
{
    public partial class OrdersForm : Form
    {
        private readonly IMediator _mediator;
        private readonly ILifetimeScope _scope;

        public OrdersForm(IMediator mediator, ILifetimeScope scope)
        {
            InitializeComponent();
            _mediator = mediator;
            _scope = scope;
        }

        private async void OrdersForm_Load(object sender, EventArgs e)
        {
            var progressIndicator = new Progress<int>(ReportProgress);

            await LoadDataAsync(progressIndicator);
        }

        private void ReportProgress(int value)
        {
            progressBarControl1.EditValue = value;
        }

        private async Task LoadDataAsync(IProgress<int> progress)
        {
            ReportProgress(25);
            var result = await Task.Run(async () => { progress?.Report(50); var data = await _mediator.Send(new Queries.OrderSumaryQueries.OrderSummaryQuery()); progress?.Report(75); return data; });

            gridControl1.DataSource = result;
            gridControl1.RefreshDataSource();
            ReportProgress(100);
        }

        private async void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (gridView1.GetFocusedRow() is OrderSummaryDTO orderSummary)
            {
                var progressIndicator = new Progress<int>(ReportProgress);
                using (var nestedScope = _scope.BeginLifetimeScope())
                {
                    var editCustomerForm = nestedScope.Resolve<EditCustomerForm>();


                    editCustomerForm.CustomerId = orderSummary.CustomerId;
                    editCustomerForm.ShowDialog();
                    await LoadDataAsync(progressIndicator);
                }
            }
        }        
    }
}
