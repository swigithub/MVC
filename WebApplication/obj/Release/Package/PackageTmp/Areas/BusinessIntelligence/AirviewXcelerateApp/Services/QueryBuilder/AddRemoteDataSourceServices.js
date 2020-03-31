

airviewXcelerateApp.factory('RemoteDataSourceServices', function ($http, $timeout, ivhTreeviewMgr, $uibModal) {

    //#region handler

    var addRemoteDataSourceServicesHandler = function () {
        var handleGroupByViewsNTablesNColumns = function (tablesNColumns) {
            let getKeyValueOfTablesNColumns = tablesNColumns.reduce((finalList, currentItem) => {
                finalList[currentItem['tableName']] = finalList[currentItem['tableName']] || {};
                finalList[currentItem['tableName']]['schema'] = currentItem['Schemas'];
                return finalList;



            }, {});
            return Object.keys(getKeyValueOfTablesNColumns).map((key) => {
                return { label: key, id: key, selected: false, value: key, schema: getKeyValueOfTablesNColumns[key]['schema'] };
            });
        };

        var handleGroupByViewsNTables = function (viewsNTables) {
            let getKeyValueOfViewsNTables = viewsNTables.reduce((finalList, currentItem) => {
                finalList[currentItem['TableType']] = finalList[currentItem['TableType']] || [];
                finalList[currentItem['TableType']].push(currentItem);
                return finalList;
            }, {});

            let getTreeViews = Object.keys(getKeyValueOfViewsNTables).map((key) => {

                return { label: key, selected: false, value: key, children: handleGroupByViewsNTablesNColumns(getKeyValueOfViewsNTables[key]) };

            });
           
            return getTreeViews;

            //  vm.dashboardAndReportComInformation.widgetList[1].treeviewOfMultiDatasetsWithItsColumns.push(getTreeViews[1]);
            //console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].treeviewOfMultiDatasetsWithItsColumns);
            //console.log(vm.dashboardAndReportComInformation.centralizeDataSourceNDatabaseInfo);


        };




        //#region Initialization
        var handleInitialization = function (pvm, vm, widgetIndex, scope, compile, toastr, rootScope, parentScope, uibModalInstance) {
            vm.tablesNViewsTree = [];
            vm.remoteDataSourceinfo = {};
            vm.remoteDataSourceinfo['dataSourceName'] = "";
            vm.remoteDataSourceinfo['remoteUserName'] = "";
            vm.remoteDataSourceinfo['remotePassword'] = "";
            vm.remoteDataSourceinfo['remoteDatabase'] = "";
            vm.remoteDataSourceinfo['remoteProductName'] = "Sql Server";
            vm.remoteDataSourceinfo['remoteDatabaseWhereClause'] = "(" + "'DriveSummaryStatus'" + "," + "'DS_SiteTestSummary'" + ")";
            //  vm.remoteDataSourceinfo['remoteProductName'] = "MySQL";
            vm.remoteDatabaseDdl = [];

            //#region Events
            //#region Get Remote Connection
            vm.getConnection = function () {
                $('.rmt.overlay').attr('style', 'display:block;');
                $.ajax({
                    type: "POST",
                    url: '/BusinessIntelligence/QueryBuilder/GetRemoteDatabase',
                    data: vm.remoteDataSourceinfo,

                    success: function (result) {
                        if (result.status) {
                            let getData = JSON.parse(result.data);
                            if (getData[0]['errorMsg']) {
                                toastr.error(getData[0]['errorMsg']);
                                vm.remoteDatabaseDdl = [];
                                vm.remoteDataSourceinfo['remoteDatabase'] = "";
                                scope.$apply();

                            }
                            else {
                                vm.remoteDatabaseDdl = getData;
                                scope.$apply();

                            }




                        }
                        else {
                            vm.remoteDatabaseDdl = [];
                            vm.remoteDataSourceinfo['remoteDatabase'] = "";
                            toastr.error("The username or password incorrect");
                        }
                        $('.rmt.overlay').attr('style', 'display:none;');

                    }
                });
            }
            //#endregion


            vm.addTablesNViews = function () {
                if (!vm.remoteDatasourceForm.$invalid && vm.remoteDataSourceinfo.remoteDatabase != "") {
                    $('.rmt.overlay').attr('style', 'display:block;');
                    $.ajax({
                        type: "POST",
                        url: '/BusinessIntelligence/QueryBuilder/GetRemoteViewsNTables',
                        data: vm.remoteDataSourceinfo,
                        success: function (result) {
                            let timeout = 0;
                            if (result.status) {
                                let getData = JSON.parse(result.data);
                                let timeout = getData.length;
                                vm.tablesNViewsTree = handleGroupByViewsNTables(getData)
                                

                                vm.openLinkProperteisModal = $uibModal.open({
                                    templateUrl: 'dsrpt/DashboardNReport/getPartialPage/QueryBuilder/_AddTablesNViews.cshtml',
                                    backdrop: 'static',
                                    controller: "RemoteTablesNViews as vm",
                                    size: 'lg  datasource-modal',
                                    resolve: {
                                        pvm: function () {
                                            return vm;
                                        },
                                        widgetIndex: function () {
                                            return widgetIndex;
                                        },
                                        parentScope: function () {
                                            return parentScope;
                                        }
                                    }
                                });
                                vm.openLinkProperteisModal.result.then(function () {
                                   
                                });
                               

                             

                                
                             

                            }
                            setTimeout(() => { uibModalInstance.dismiss('cancel'); $('.rmt.overlay').attr('style', 'display:none;'); }, timeout);
                           


                        }
                    });

                }

               
            };
            //#region 
            //vm.addRemoteDatasource = function () {
            //    if (!vm.remoteDatasourceForm.$invalid && vm.remoteDataSourceinfo.remoteDatabase != "") {
            //        $('.rmt.overlay').attr('style', 'display:block;');
            //        $.ajax({
            //            type: "POST",
            //            url: '/BusinessIntelligence/QueryBuilder/GetRemoteColumnsOfTablesNViews',
            //            data: vm.remoteDataSourceinfo,
            //            success: function (result) {
            //                let timeout = 0;
            //                if (result.status) {
            //                    let getData = JSON.parse(result.data);
            //                    let dataSourceNTablesNViewsNColumns = { dataSource: vm.remoteDataSourceinfo, viewsTablesNColumn: getData, widgetIndex: widgetIndex };
            //                    timeout = getData.length;
            //                    // handleGroupByViewsNTables(getData);
            //                    uibModalInstance.dismiss('cancel');
            //                    parentScope.$emit('postViewsNTablesNColumnsE', dataSourceNTablesNViewsNColumns);





            //                }
            //                setTimeout(() => { $('.rmt.overlay').attr('style', 'display:none;'); }, timeout);
                           

            //            }
            //        });

            //    }


            //    // uibModalInstance.dismiss('cancel');
            //};

            //#endregion
            //#region cancel modal
            vm.cancel = function () {

                uibModalInstance.dismiss('cancel');

            };

            //#endregion

            //#endregion
        };

        //#endregion

        return {
            initialization: function (pvm, vm, widgetIndex, scope, compile, toastr, rootScope, parentScope, uibModalInstance) {
                handleInitialization(pvm, vm, widgetIndex, scope, compile, toastr, rootScope, parentScope, uibModalInstance);

            }
        };
    }();
    return addRemoteDataSourceServicesHandler;

    //#endregion
});

//#endregion