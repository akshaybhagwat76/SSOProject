using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSOApp.ViewModels
{
    public class AssignmentViewModule
    {
        public List<ListItemValue> AvailableValues { get; set; }

        public List<ListItemValue> CurrentValues { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Entity { get; set; }

        public string SelectedValue { get; set; }
    }

    public class ListItemValue
    {
        public string DisplayText { get; set; }
        public string DisplayValue { get; set; }
    }

    public class AssignmentSaveViewModule

    {
        public Guid TenantId { get; set; }
        public string SelectedValue { get; set; }
        public List<string> ListofAssignment { get; set; }
    }
}
