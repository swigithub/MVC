<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UtilizationPerMarket.aspx.cs" Inherits="WebApplication.Report.UtilizationPerMarket" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
</head>
<body>
    <form id="form1" runat="server">
   <section class="bs-docs-section clearfix">
        <div class="col-md-12">
             <div class="box box-primary">
                
                 <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                 <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                         <ContentTemplate>
<div class="box-header with-border">

                    <h4 class="box-title">Report Criteria</h4>
                </div>
                 <div class="box-body">
            <div id="builder-basic" style="background-color:white"></div>
              <div class="btn-group pull-right" style="margin-top:20px">
            <button class="btn btn-info parse-json " id="btn-search" data-target="basic">View Report</button>
                   <%--<button type="button" class="btn btn-info dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                    <span class="caret"></span>
                                    <span class="sr-only">Toggle Dropdown</span>
                                </button>--%>
                            </div></div>
                         </ContentTemplate>
                 </asp:UpdatePanel>
        
       
                 </div></div>
       <asp:TextBox ID="txtwhere" style="display:none"   runat="server"></asp:TextBox>
      <asp:Button ID="btnreport" runat="server" Text="Report" style="display:none" OnClick="btnreport_Click" UseSubmitBehavior="False" />  
       <%-- <asp:Button ID="btnreport" runat="server"  style="display:none" Text="Report" OnClick="btnreport_Click" UseSubmitBehavior="False" />--%>

      
                 <center>
<rsweb:ReportViewer ID="ReportViewer1" AsyncRendering="False" ZoomMode="FullPage" SizeToReportContent="True" Width="100%" Height="100%" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"></rsweb:ReportViewer>
         
        </center>
             

            </section>
    </form>

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

            //$(document).on('click', '#btnSearch', function () {

            //    alert('sdf');
            //    return false;
            //});

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



            $('#btn-search').on('click', function () {

                var result = $('#builder-basic').queryBuilder('getSQL', false);

                
                $('#txtwhere').val(result.sql);
                $("#<%=btnreport.ClientID%>").click();

                //var result = $('#builder-basic').queryBuilder('getRules');

                //if (!$.isEmptyObject(result)) {
                //    alert(JSON.stringify(result, null, 2));
                //}
            });



        });
       $(function () {


       });

    </script>
</body>
</html>
