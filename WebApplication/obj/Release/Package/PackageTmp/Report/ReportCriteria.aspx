<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportCriteria.aspx.cs" Inherits="WebApplication.Report.ReportCriteria" %>

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

                    <div class="box-header with-border">
                        <h4 class="box-title">Report Criteria</h4>
                    </div>
                    <div id="builder-basic" style="background-color: white"></div>
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">

                        <ContentTemplate>

                            <div class="box-body">

                                <div class="btn-group pull-right" style="margin-top: 20px">
                                    <asp:Button ID="GetReport" OnClientClick="call()" runat="server" CssClass="btn btn-info" Text="View Report" OnClick="GetReport_Click" UseSubmitBehavior="False" />
                                </div>

                                <div class="box box-primary">

                                    <div class="box-header with-border"></div>
                                    <div class="box-body">
                                        <asp:UpdateProgress ID="UpdateProgress1" style="align-content: center" runat="server" AssociatedUpdatePanelID="UpdatePanel2" DynamicLayout="true">
                                            <ProgressTemplate>
                                                <center>
                                        <img src="../Content/Images/Application/Logo_Loading.gif" />
                                            </center>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <center>
                                            <rsweb:ReportViewer ID="ReportViewer1" AsyncRendering="False" ZoomMode="FullPage" SizeToReportContent="True" Width="100%" Height="100%" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"></rsweb:ReportViewer>
                                        </center>
                                        <asp:TextBox ID="txtwhere" Style="display: none" runat="server"></asp:TextBox>

                                    </div>

                                </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>


            </div>

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
        function call() {
            var result = $('#builder-basic').queryBuilder('getSQL', false);
            $("#<%=txtwhere.ClientID%>").val(result.sql);
            //alert(result.sql);
             return true;
         }

         $(document).ready(function () {
             var DEFAULTS = {
                 operators: ['AND', 'OR', 'XOR']
             };

             var config = {
                 operators: ['OR', 'XOR']
             };

             config = $.extend(true, {}, DEFAULTS, config);


             var sql_import_export = $("#<%=txtwhere.ClientID%>").val();


            var obj = {};
            var Filter = [];

            $.ajax({
                url: '/Report/ReportFilters',
                type: 'post',
                async: false,
                data: { 'ReportId': '33247' },
                dataType: 'json',
                success: function (res) {
                    //    console.log(res);
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

            //var result = $('#builder-basic').queryBuilder('getRules');




            <%--$('#btn-search').on('click', function () {
                
              //  debugger;
                var result = $('#builder-basic').queryBuilder('getSQL', false);
                



                $("#<%=txtwhere.ClientID%>").val(result.sql);

                $("#<%=GetReport.ClientID%>").click();
                $("#<%=Button1.ClientID%>").click();
            });--%>



        });


    </script>
</body>
</html>
