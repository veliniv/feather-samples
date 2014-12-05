using DevMagazine.Issues.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.Data;
using DynamicContent.Mvc.Controllers;
using Telerik.Sitefinity.Mvc.ActionFilters;

namespace DevMagazine.Issues.Mvc.Controllers
{
    public class PagerController: DynamicContentController
    {
        [StandaloneResponseFilter]
        public ActionResult Archive(int? page, int? totalCount)
        {
            IQueryable<Telerik.Sitefinity.DynamicModules.Model.DynamicContent> issues = DynamicModuleManager.GetManager().GetDataItems(IssueViewModel.IssueType);

            var filterExpression = string.Format("Visible = true AND Status = Live");
            var sortingExpression = "PublicationDate DESC";

            var itemsPerPage = 6;

            int? skip = page > 1 ? ((page - 1) * itemsPerPage) : 0;
            int? take = itemsPerPage;

            issues = DataProviderBase.SetExpressions(issues, filterExpression, sortingExpression, skip, take, ref totalCount);

            var pagerVM = new PagerViewModel();

            pagerVM.Issues = issues.ToList()
                .Select(item => IssueViewModel.GetIssue(item));
            pagerVM.TotalPagesCount = totalCount;

            return PartialView("List.ArchivedIssues", pagerVM);
        }        
    }
}
