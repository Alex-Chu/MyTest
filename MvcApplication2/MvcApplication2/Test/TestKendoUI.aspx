﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestKendoUI.aspx.cs" Inherits="WebApplication2.TestKendoUI2" EnableTheming="False" EnableViewState="False" Theme="" StylesheetTheme=""    %>

<!DOCTYPE html>
 
<html>
<head>
    <title>Grid Lab 1</title>
    <META HTTP-EQUIV="Pragma" CONTENT="no-cache"> 
    <META HTTP-EQUIV="Expires" CONTENT="-1"> 
    <!--In the header of your page, paste the following for Kendo styles-->
    <!-- <link href="../Content/kendo/2013.3.1119/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="../Content/kendo/2013.3.1119/kendo.default.min.css" rel="stylesheet" type="text/css" />-->
    <link rel="stylesheet" type="text/css" href="http://cdn.kendostatic.com/2013.3.1119/styles/kendo.common.min.css"/>
    <link rel="stylesheet" type="text/css" href="http://cdn.kendostatic.com/2013.3.1119/styles/kendo.default.min.css"/>
 
    <!--Then paste the following for Kendo scripts-->
    <!--<script src="../Scripts/kendo/2013.3.1119/jquery.min.js" type="text/javascript"></script>-->
    <script type='text/javascript' src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <!--<script src="../Scripts/kendo/2013.3.1119/kendo.web.min.js" type="text/javascript"></script>-->
    <script type='text/javascript'src="http://cdn.kendostatic.com/2013.3.1119/js/kendo.web.min.js"></script>
    <style>
        body { font-size: 9pt; }
        #dvGrid { width: 500px; }
        span.hi-lite { color: red; }
        #dvGrid th.k-header { text-align: center }
    </style>
    <script>
        $(function () {
            //建立資料來源物件
            var dataSrc = new kendo.data.DataSource({
                transport: {
                    read: {
                        //以下其實就是$.ajax的參數
                        type: "POST",
                        url: "<%=Request.Url.Segments[Request.Url.Segments.Length-1]%>",
                        dataType: "json",
                        data: {
                            //額外傳至後方的參數
                            keywd: function () {
                                return $("#tKeyword").val();
                            }
                        }
                    }
                },
                schema: {
                    //取出資料陣列
                    data: function (d) { return d.Data; },
                    //取出資料總筆數(計算頁數用)
                    total: function (d) { return d.TotalCount; }
                },
                pageSize: 10,
                serverPaging: true,
                serverSorting: true
            });
            //JSON日期轉換
            var dateRegExp = /^\/Date\((.*?)\)\/$/;
            window.toDate = function (value) {
                var date = dateRegExp.exec(value);
                return new Date(parseInt(date[1]));
            }

            $("#dvGrid").kendoGrid({
                dataSource: dataSrc,
                columns: [
                    { field: "UserNo", title: "會員編號" },
                    {
                        field: "UserName", title: "會員名稱",
                        template: '#= "<span class=\\"u-name\\">" + UserName + "</span>" #'
                    },
                    {
                        field: "RegDate", title: "加入日期",
                        template: '#= kendo.toString(toDate(RegDate), "yyyy/MM/dd")#'
                    },
                    { field: "Points", title: "累積點數" },
                ],
                sortable: true,
                pageable: true,
                dataBound: function () {
                    //AJAX資料Bind完成後觸發
                    var kw = $("#tKeyword").val();
                    //若有設關鍵字，做Highlight處理
                    if (kw.length > 0) {
                        var re = new RegExp(kw, "g");
                        $(".u-name").each(function () {
                            var $td = $(this);
                            $td.html($td.text()
                           .replace(re, "<span class='hi-lite'>$&</span>"));
                        });
                    }

                    $("#dvGrid").slideUp();
                    $("#dvGrid").slideDown();
                    //$("#dvGrid").fadeOut();
                    //$("#dvGrid").fadeIn();
                }
            });
            //按下查詢鈕
            $("#bQuery").click(function () {
                //要求資料來源重新讀取(並指定切至第一頁)
                dataSrc.read({ page: 1, skip: 0 });
                //Grid重新顯示資料 2013-07-19更正，以下可省略
                //$("#dvGrid").data("kendoGrid").refresh();
            });
        });
    </script>
</head>
<body>
  <center>
     <b style="color:#B22222;font-size:20px;">--- KendoUI ---</b>
     <br/>
     <b style="color:blue;">(Webform</b>
     <!--<br/>-->
     <b style="color:black;">\</b>
     <b style="color:blue;">ASPX)</b>
  </center>
<hr />
<div style="padding: 10px;width:100%">
    <b style="color:#FF1493;">關鍵字: </b><input id="tKeyword" /><input type="button" value="查詢" id="bQuery" />
    &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Test/TestKendoUI">ASP.MVC</asp:HyperLink>
    &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Test/TestKendoUI.html">WebAPI</asp:HyperLink>
</div>
<div id="dvGrid" style="width:100%;"></div>
</body>
</html>
