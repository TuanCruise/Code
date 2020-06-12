using System;
using System.Text;
using System.Collections.Generic;

namespace WebModelCore {
    public class SortAndPageModel
    {
        public SortAndPageModel()
        {
            PageSize = 20;
            CurrentPageIndex = 1;
            TotalRecordCount = 0;
        }
        public string SortBy { get; set; }
        public bool SortDescending { get; set; }
        public int CurrentPageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecordCount { get; set; }

        //Gen mã Html cho phân trang
        public string GenPaging()
        {
            var page = new Pager
            {
                TotalRecords = TotalRecordCount,
                PageSize = PageSize,
                CurrentPage = CurrentPageIndex
            };
            return page.Render();
        }

        public string GenPaging(string functionName)
        {
            var page = new Pager
            {
                TotalRecords = TotalRecordCount,
                PageSize = PageSize,
                CurrentPage = CurrentPageIndex
            };
            return page.Render(functionName);
        }
    }

    public class Pager
    {
        private int _currentPage = 1;
        private int _pageNumber = 5;
        private int _pageSize = 25;
        // Fields
        private int _totalPages = -1;

        public Pager()
        {
            TotalRecords = 0;
        }

        //Exec
        public string Render()
        {
            var tbLink = BindPageNumbers(TotalRecords, PageSize);

            var builder = new StringBuilder();
            builder.Append("<ul class='pagination' style='visibility: visible;'>" + GetFirstLink());
            builder.Append(GetPreviousLink());

            if (tbLink != null)
            {
                foreach (string rNode in tbLink)
                {
                    builder.Append(GetLink(Convert.ToInt32(rNode)));
                }
            }
            builder.Append(GetNextLink());
            builder.Append(GetLastLink() + "</ul>");
            return builder.ToString();
        }

        public string Render(string functionName)
        {
            var tbLink = BindPageNumbers(TotalRecords, PageSize);

            var builder = new StringBuilder();
            builder.Append("<ul class='pagination' style='visibility: visible;'>" + GetFirstLink(functionName));
            builder.Append(GetPreviousLink(functionName));

            if (tbLink != null)
            {
                foreach (string rNode in tbLink)
                {
                    builder.Append(GetLink(Convert.ToInt32(rNode), functionName));
                }
            }

            builder.Append(GetNextLink(functionName));
            builder.Append(GetLastLink(functionName) + "</ul>");

            return builder.ToString();
        }

        #region Properties

        public int CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = value; }
        }

        public int TotalRecords { get; set; }

        #endregion

        #region Functions CreateLink

        private string GetFirstLink()
        {
            if (PageSize >= TotalRecords)
            {
                return "";
            }
            if ((CurrentPage > 1) && (_totalPages > 0))
            {
                return
                    "<li class='prev'><a  tabindex='8' title='First' onclick=\"PagerClick(1);\"><i class='fa fa-angle-double-left'></i></a></li>";
            }
            return "";
        }

        private string GetFirstLink(string functionName)
        {
            if (PageSize >= TotalRecords)
            {
                return "";
            }
            if ((CurrentPage > 1) && (_totalPages > 0))
            {
                return
                    "<li class='prev'><a tabindex='8' title='First' onclick='" + functionName + "(" + "1" + ")" + "'><i class='fa fa-angle-double-left'></i></a></li>";
            }
            return "";
        }

        private string GetLastLink()
        {
            if (PageSize >= TotalRecords)
            {
                return "";
            }
            if ((CurrentPage < _totalPages) && (_totalPages > 0))
            {
                return "<li class='next'><a tabindex='50' title='Last' onclick=\"PagerClick(" + _totalPages +
                       ");\"><i class='fa fa-angle-double-right'></i></a></li>";
            }
            return "";
        }

        private string GetLastLink(string functionName)
        {
            if (PageSize >= TotalRecords)
            {
                return "";
            }
            if ((CurrentPage < _totalPages) && (_totalPages > 0))
            {
                return "<li class='next'><a tabindex='50' title='Last' onclick='" + functionName + "(" + _totalPages + ")" + "'><i class='fa fa-angle-double-right'></i></a></li>";
            }
            return "";
        }

        private string GetLink(int pageNum)
        {
            if (PageSize >= TotalRecords)
            {
                return "";
            }
            if (pageNum == CurrentPage)
            {
                return "<li class='active'><a tabindex='" + (10 + pageNum) + "' >" + pageNum + "</a></li>";
            }
            return "<li><a tabindex='" + (10 + pageNum) + "' onclick=\"PagerClick(" + pageNum + ");\">" + pageNum + "</a></li>";
        }

        private string GetLink(int pageNum, string functionName)
        {
            if (PageSize >= TotalRecords)
            {
                return "";
            }
            if (pageNum == CurrentPage)
            {
                return "<li class='active'><a tabindex='" + (10 + pageNum).ToString() + "'>" + pageNum + "</a></li>";
            }
            return "<li><a onclick='" + functionName + "(" + pageNum + ")" + "' tabindex='" + (10 + pageNum).ToString() + "'>" + pageNum + "</a></li>";
        }

        private string GetNextLink()
        {
            if (PageSize >= TotalRecords)
            {
                return "";
            }
            if ((CurrentPage < _totalPages) && (_totalPages > 0))
            {
                return "<li class='next'><a title='Next' tabindex='20' onclick=\"PagerClick(" + (CurrentPage + 1) +
                       ");\"><i class='fa fa-angle-right'></i></a></li>";
            }
            return "";
        }

        private string GetNextLink(string functionName)
        {
            if (PageSize >= TotalRecords)
            {
                return "";
            }
            if ((CurrentPage < _totalPages) && (_totalPages > 0))
            {
                return "<li class='next'><a tabindex='20' title='Next' onclick='" + functionName + "(" + (CurrentPage + 1).ToString() + ")" + "'><i class='fa fa-angle-right'></i></a></li>";
            }
            return "";
        }

        private string GetPreviousLink()
        {
            if (PageSize >= TotalRecords)
            {
                return "";
            }
            if ((CurrentPage > 1) && (_totalPages > 0))
            {
                return "<li class='prev'><a tabindex='10' title='Prev' onclick=\"PagerClick(" + (CurrentPage - 1) +
                       ");\"><i class='fa fa-angle-left'></i></a></li>";
            }
            return "";
        }

        private string GetPreviousLink(string functionName)
        {
            if (PageSize >= TotalRecords)
            {
                return "";
            }
            if ((CurrentPage > 1) && (_totalPages > 0))
            {
                return "<li class='prev'><a  tabindex='10' title='Prev' onclick='" + functionName + "(" + (CurrentPage - 1).ToString() + ")" + "'><i class='fa fa-angle-left'></i></a></li>";
            }
            return "";
        }

        // Methods Other
        private List<string> BindPageNumbers(int totalRecords, int recordsPerPage)
        {
            if ((totalRecords < 1) || (recordsPerPage < 1))
            {
                _totalPages = 1;
            }
            else
            {
                _totalPages = totalRecords / recordsPerPage >= 1
                    ? Convert.ToInt32(Math.Ceiling(Convert.ToDouble(totalRecords) / recordsPerPage))
                    : 0;
                if (_totalPages > 0)
                {
                    var table = new List<string>();
                    var num = 1;
                    int num2;
                    double d = CurrentPage - PageNumber / 2;
                    if (d < 1.0)
                    {
                        d = 1.0;
                    }
                    if (CurrentPage > PageNumber / 2)
                    {
                        num = Convert.ToInt32(Math.Floor(d));
                    }
                    if (Convert.ToInt32(_totalPages) <= PageNumber)
                    {
                        num2 = Convert.ToInt32(_totalPages);
                    }
                    else
                    {
                        num2 = num + PageNumber - 1;
                    }
                    if (num2 > Convert.ToInt32(_totalPages))
                    {
                        num2 = Convert.ToInt32(_totalPages);
                        if (num2 - num < PageNumber)
                        {
                            num = num2 - PageNumber + 1;
                        }
                    }
                    if (num2 > Convert.ToInt32(_totalPages))
                    {
                        num2 = Convert.ToInt32(_totalPages);
                    }
                    if (num < 1)
                    {
                        num = 1;
                    }
                    for (var i = num; i <= num2; i++)
                    {
                        table.Add(i + "");
                    }
                    return table;
                }
            }

            return null;
        }

        #endregion
    }
}