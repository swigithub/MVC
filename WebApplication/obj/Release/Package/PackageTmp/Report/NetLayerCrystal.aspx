<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NetLayerCrystal.aspx.cs" Inherits="WebApplication.Report.NetLayerCrystal" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>

    <%--<QueryBuilder Start>--%>
    <link href="../AdminLTE/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/js/Plugins/QueryBuilder/css/query-builder.default.css" rel="stylesheet" />
    <link href="../AdminLTE/plugins/datepicker/css/datepicker3.css" rel="stylesheet" />
    <style>
        #content {
            background: #0074d9;
            background: linear-gradient(135deg, #0074d9, #001f3f);
        }
    </style>
    <script src="../AdminLTE/plugins/jquery/js/jQuery-2.1.4.min.js"></script>
    <script src="../AdminLTE/bootstrap/js/bootstrap.min.js"></script>
    <script src="../Content/js/Plugins/QueryBuilder/js/moment.min.js"></script>
    <%--<QueryBuilder End>--%>
</head>
<body>
    <form id="form1" runat="server">
        <section class="bs-docs-section clearfix">
            

          
            <div class="col-md-12">
                  <div class="box box-primary">
                <div class="box-header with-border">

                    <h4 class="box-title">Report Criteria</h4>
                </div>
                <div class="box-body">
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div id="builder-basic" style="background-color:white"></div>
                        <div class="btn-group pull-right" style="margin-top:20px";>
                  <button type="button" class="btn btn-info">View Report</button>
                  <button type="button" class="btn btn-info dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                    <span class="caret"></span>
                    <span class="sr-only">Toggle Dropdown</span>
                  </button>
                  <ul class="dropdown-menu" role="menu">
                    <li style="text-align:left"><%--<a href="#" onclick="myFunction()" >Export To PDF</a>--%>
<asp:Button ID="PDF"  runat="server" style="background-color:white;border:none;padding-right:60px;"   Text="Export To PDF" OnClick="PDF_Click" />
                           
                    </li>
                    <li style="text-align:left"><%--<a href="#">Export To Excel</a>--%>
<asp:Button ID="EXCEL" style="background-color:white;border:none;padding-right:45px;" runat="server"  Text="Export To EXCEL" OnClick="EXCEL_Click" />
                      
                    </li>
                    <li style="text-align:left"><%--<a href="#">Export To Word</a>--%>
<asp:Button ID="WORD" style="background-color:white;border:none;padding-right:45px;" runat="server"  Text="Export To WORD" OnClick="WORD_Click" />
          

                    </li>
                   
                  </ul>
                </div>
                       
                    </ContentTemplate>
                </asp:UpdatePanel>
                </div></div>
            </div>
            <asp:TextBox ID="txtwhere" Style="display: none;" runat="server"></asp:TextBox>

            <CR:CrystalReportViewer ID="CrystalViewer1" runat="server" AutoDataBind="True" ToolPanelView="None" />

        </section>
    </form>



    <%--<QueryBuilder Start>--%>
    <script src="../Content/js/Plugins/QueryBuilder/js/jQuery.extendext.js"></script>

    <script src="../Content/js/Plugins/QueryBuilder/js/query-builder.js"></script>
    <script src="../Content/js/Plugins/QueryBuilder/js/query-builder.standalone.js"></script>
    <script src="../Content/js/Plugins/QueryBuilder/js/query-builder.en.js"></script>

    <script src="../AdminLTE/plugins/datepicker/js/bootstrap-datepicker.js"></script>
    <style>
        .code-popup {
            max-height: 500px;
        }
    </style>
    <%--<script>
        function myFunction() {
 var result = $('#builder-basic').queryBuilder('getRules');

 $("#PDF").click(function () {

                var result = $('#builder-basic').queryBuilder('getSQL', false);
              
                $("#<%=txtwhere.ClientID%>").val(result.sql);
                 $("#<%=PDF.ClientID%>").click();
 });

        }
    </script>--%>
    <script>

        $(document).ready(function () {
            var DEFAULTS = {
                operators: ['AND', 'OR', 'XOR']
            };

            var config = {
                operators: ['OR', 'XOR']
            };

            config = $.extend(true, {}, DEFAULTS, config);



            var sql_import_export = 'name LIKE "%Johnny%" AND (Country = 2 OR in_stock = 1)';



            var obj = {};
            var Filter = [];

            $.ajax({
                url: '/Report/ReportFilters',
                type: 'post',
                async: false,
                data: { 'ReportId': '555' },
                dataType: 'json',
                success: function (res) {
                    console.log(res);
                    for (var i = 0; i < res.length; i++) {

                        obj['id'] = res[i].MapColumn;
                        obj['label'] = res[i].DefinationName;
                        obj['type'] = res[i].InputType;

                        if (res[i].DisplayType == 'select') {
                            obj['input'] = res[i].DisplayType;
                            obj['values'] = SelectResult(res[i].DefinationId, res[i].DefinationName);
                        }
                        if (res[i].DisplayType == 'date') {
                            type: 'date',
                            obj['validation'] = {
                                format: 'YYYY/MM/DD'
                            },
                            obj['plugin'] = 'datepicker',
                            obj['plugin_config'] = {
                                format: 'yyyy/mm/dd',
                                todayBtn: 'linked',
                                todayHighlight: true,
                                autoclose: true
                            }
                        }
                        Filter.push(obj);
                        obj = {};
                    }
                }
            });


            function SelectResult(Filter, Name) {
                var SelectedList = { 0: 'Select ' + Name };
                $.ajax({
                    url: '/Report/FilterQuery',
                    type: 'post',
                    async: false,
                    data: { 'value': Filter },
                    dataType: 'json',
                    success: function (res) {
                        for (var i = 0; i < res.length; i++) {
                            SelectedList[res[i].Value] = res[i].Text;
                        }
                    }
                });
                return SelectedList;
            }



            $('#builder-basic').queryBuilder({
                plugins: ['bt-tooltip-errors',
                    'not-group'
                ],

                filters: Filter,

            });

            var result = $('#builder-basic').queryBuilder('getRules');

            $("#PDF").click(function () {

                var result = $('#builder-basic').queryBuilder('getSQL', false);
              
                $("#<%=txtwhere.ClientID%>").val(result.sql);
                 $("#<%=PDF.ClientID%>").click();
            });
           
            $("#EXCEL").click(function () {

                var result = $('#builder-basic').queryBuilder('getSQL', false);
               
                $("#<%=txtwhere.ClientID%>").val(result.sql);
                 $("#<%=EXCEL.ClientID%>").click();
            });
            $("#WORD").click(function () {

                var result = $('#builder-basic').queryBuilder('getSQL', false);
                
                $("#<%=txtwhere.ClientID%>").val(result.sql);
                 $("#<%=WORD.ClientID%>").click();
            });




        });
        $(function () {


        });

    </script>
    <%--<QueryBuilder End>--%>
</body>
</html>
