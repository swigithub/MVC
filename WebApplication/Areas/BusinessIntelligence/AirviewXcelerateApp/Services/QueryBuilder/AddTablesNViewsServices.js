

airviewXcelerateApp.factory('RemoteTablesNViewsServices', function ($http, $timeout, toastr, uiGridConstants) {

    //#region handler

    var addRemoteDataSourceServicesHandler = function () {

        
        var handleBindGridDiv = function () {
            return ` <div style="height:597px" ui-grid="vm.uiGridConfiguration.uiGridInitailization" class="grid" ui-grid-resize-columns ui-grid-pagination ui-grid-selection ui-grid-auto-resize></div>`;

        }


        var handleGetQueryResult = function (queryBuild) {
            return $.ajax({
                type: "POST",
                url: '/BusinessIntelligence/QueryBuilder/GetQueryResult',
                data: { query: queryBuild }
            });
        };
        //#region make query for pagination before render grid
        var handleMakeQueryBeforeRenderGrid = function (queryBuild, pageSize, pageNumber, orderBy) {

            let makeQueryForUiGrid = "WITH selectTable AS (putTable) SELECT (SELECT COUNT(*)  FROM selectTable) AS ROWCOUNTS,* FROM selectTable orderby OFFSET @PageSize * (@PageNumber-1) ROWS  FETCH NEXT @PageSize  ROWS ONLY";
            makeQueryForUiGrid = makeQueryForUiGrid.replace(/putTable/g, queryBuild);
            makeQueryForUiGrid = makeQueryForUiGrid.replace(/@PageSize/g, pageSize);
            makeQueryForUiGrid = makeQueryForUiGrid.replace(/@PageNumber/g, pageNumber);
            makeQueryForUiGrid = makeQueryForUiGrid.replace(/orderby/g, orderBy);
            return makeQueryForUiGrid;

        };
        //#endregion

        //#region make query for pagination after render grid
        var handleMakeQueryAfterRenderGrid = function (queryBuild, pageSize, pageNumber, orderBy) {
            let makeQueryForUiGrid = "putTable orderby OFFSET @PageSize * (@PageNumber-1) ROWS  FETCH NEXT @PageSize  ROWS ONLY";
            makeQueryForUiGrid = makeQueryForUiGrid.replace(/putTable/g, queryBuild);
            makeQueryForUiGrid = makeQueryForUiGrid.replace(/@PageSize/g, pageSize);
            makeQueryForUiGrid = makeQueryForUiGrid.replace(/@PageNumber/g, pageNumber);
            makeQueryForUiGrid = makeQueryForUiGrid.replace(/orderby/g, orderBy);
            return makeQueryForUiGrid;
        };
        //#endregion
        var handleShowDataLastSelectTable = function (vm, pvm, tableName, schema, scope, compile) {
            $('.grid-div.overlay').attr('style', 'display:block;');
            let makeQuery = "SELECT * FROM " + "[" + pvm.remoteDataSourceinfo['dataSourceName'] + "].[" + pvm.remoteDataSourceinfo['remoteDatabase'] + "].[" + schema + "].[" + tableName + "]";
            makeQuery = handleMakeQueryBeforeRenderGrid(makeQuery, 30, 1, 'ORDER BY (SELECT NULL)');
           
            handleGetQueryResult(makeQuery).success((res) => {
                vm.data = [];
                if (res.status) {
                    $('.bind-uigrid > div:eq(1)').remove();
                    $('.bind-uigrid').append(compile($.parseHTML(handleBindGridDiv()))(scope));
                    var getData = JSON.parse(res.data);
                    vm.data = getData;
                    if (vm.data.length > 0) {
                      
                        vm.uiGridConfiguration.uiGridInitailization = {};
                        vm.uiGridConfiguration['paginationParams']['totalItemsOfUiGrid'] = vm.data[0]['ROWCOUNTS'];
                        vm.data.filter(function (item) {
                            delete item.ROWCOUNTS;
                        })

                        handleInitUiGrid(vm, scope)
                    }
                    
                    scope.$apply();
                }
                 $('.grid-div.overlay').attr('style', 'display:none;');
               
                })
                
           
        }

        var handleInitUiGrid = function (vm, scope) {

            vm.uiGridConfiguration.uiGridInitailization = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.NEVER,
                paginationPageSizes: [5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100],
                paginationPageSize: vm.uiGridConfiguration.paginationParams.paginationSize,
                useExternalPagination: true,
                useExternalSorting: true,
                appScopeProvider: vm,
                enableSorting: true,
                rowHeight: 30,
                showColumnFooter: false,
                enableGridMenu: true,
                totalItems: vm.uiGridConfiguration.paginationParams.totalItemsOfUiGrid,
                //rowTemplate:
                //    '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
                columnDefs: [],
                onRegisterApi: function (gridApi) {

                    vm.uiGridConfiguration.gridApi = gridApi;
                    vm.uiGridConfiguration.gridApi.core.on.sortChanged(scope, function (grid, sortColumns) {

                        if (!sortColumns.length || !sortColumns[0].field) {
                            requestParams.sorting = null;
                        } else {
                            requestParams.sorting = '[' + sortColumns[0].field + '] ' + sortColumns[0].sort.direction;
                            vm.uiGridConfiguration.paginationParams.orderBy = " ORDER BY " + requestParams.sorting;
                        }
                        //console.log(vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.paginationParams.orderBy);
                       // handleQueryChangePageOrOrderBy(compile, scope, vm, uiGridConstants, WigetIndex, vm.uiGridConfiguration.paginationParams.paginationSize, vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.paginationParams.pageNumber);
                    });
                    vm.uiGridConfiguration.gridApi.pagination.on.paginationChanged(scope, function (pageNumber, pageSize) {
                       // handleQueryChangePageOrOrderBy(compile, scope, vm, uiGridConstants, WigetIndex, pageSize, pageNumber);

                    });
                    vm.uiGridConfiguration.gridApi.grid.registerRowsProcessor(function (renderableRows) {
                        // console.log(vm.dashboardAndReportComInformation.widgetList[WigetIndex].SearchText);

                        //if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].SearchText != '') {
                        //    var value = vm.dashboardAndReportComInformation.widgetList[WigetIndex].SearchText ? vm.dashboardAndReportComInformation.widgetList[WigetIndex].SearchText.toLowerCase() : "";
                        //    var matcher = new RegExp(value);
                        //    renderableRows.forEach(function (row) {
                        //        var match = false;

                        //        vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.uiGridInitailization.columnDefs.forEach(function (field) {


                        //            var val = row.entity[field.name] ? row.entity[field.name] : "";


                        //            if (String(val.toString().toLowerCase()).match(matcher)) {
                        //                match = true;
                        //            }
                        //        });
                        //        if (!match) {
                        //            row.visible = false;
                        //        }
                        //    });
                        //}

                        return renderableRows;
                    }, 200);
                   
                },

                data: vm.data
            };
           

        }


        var handleAddTableOrView = function (vm, value) {
            vm.tableNViews.push(value);

        };
        var handleRemoveTableOrView = function (vm, value) {
            let getIndex = vm.tableNViews.findIndex(x=>x == value);
            if (getIndex > -1) {
                vm.tableNViews.splice(1, getIndex);
            }
            
        };
        var handleAddListTableOrView = function (vm, ivhNode) {
            for (let i = 0; i < ivhNode.children.length; i++) {
                handleAddTableOrView(vm, ivhNode.children[i]['value']);

            }
        };
        var handleRemoveListTableOrView = function (vm, ivhNode) {
            for (let i = 0; i < ivhNode.children.length; i++) {
                handleRemoveTableOrView(vm, ivhNode.children[i]['value']);

            }
        };
        var handleCheckAddListOrSingleTableOrView = function (vm, ivhNode, pvm, scope, compile) {
            if (typeof ivhNode.children != "undefined") {
                handleAddListTableOrView(vm, ivhNode);
            }

            else {
                handleAddTableOrView(vm, ivhNode['value']);
                handleShowDataLastSelectTable(vm, pvm, ivhNode['value'], ivhNode['schema'], scope, compile);
            }
        };
        var handleCheckRemoveListOrSingleTableOrView = function (vm, ivhNode, pvm,scope) {
            if (typeof ivhNode.children != "undefined") {
                handleRemoveListTableOrView(vm, ivhNode);
            }

            else {
                handleRemoveTableOrView(vm, ivhNode['value']);

            }
        };
        var handleMakeWhereClauseForTablesNViews = function (vm) {
            var whereClause = "("
            for (var i = 0; i < vm.tableNViews.length; i++) {
                whereClause += "'" + vm.tableNViews[i] + "'" + ","

            }
            whereClause = whereClause.substr(0, whereClause.length - 1)
            whereClause += ")";
            return whereClause;

        }

        //#region Initialization
        var handleInitialization = function (pvm, vm, widgetIndex, scope, compile, toastr, rootScope, parentScope, uibModalInstance, uiGridConstants) {
            vm.tableNViews = [];
            vm.data = [];
            vm.uiGridConfiguration = {};
            vm.uiGridConfiguration['uiGridInitailization'] = {};
            vm.uiGridConfiguration['gridApi'] = {};            
            vm.uiGridConfiguration['paginationParams'] = {}
            vm.uiGridConfiguration['paginationParams']['paginationSize'] = 30;
            vm.uiGridConfiguration['paginationParams']['totalItemsOfUiGrid'] = 0
            vm.uiGridConfiguration['paginationParams']['orderBy'] = '';
            
        
            
            vm.schmaDesignerNodeChange = function (ivhNode, ivhIsSelected, ivhTree) {
                if (ivhIsSelected) {
                    handleCheckAddListOrSingleTableOrView(vm, ivhNode, pvm, scope, compile);
                }
                else {
                    handleCheckRemoveListOrSingleTableOrView(vm, ivhNode, pvm,scope);
                }

            };
            vm.addRemoteDatasource = function () {
                if (vm.tableNViews.length > 0) {

                    pvm.remoteDataSourceinfo['remoteDatabaseWhereClause'] = handleMakeWhereClauseForTablesNViews(vm);

                    $('.rmt.overlay').attr('style', 'display:block;');
                    $.ajax({
                        type: "POST",
                        url: '/BusinessIntelligence/QueryBuilder/GetRemoteColumnsOfTablesNViews',
                        data: pvm.remoteDataSourceinfo,
                        success: function (result) {
                            let timeout = 0;
                            if (result.status) {
                                let getData = JSON.parse(result.data);
                                let dataSourceNTablesNViewsNColumns = { dataSource: pvm.remoteDataSourceinfo, viewsTablesNColumn: getData, widgetIndex: widgetIndex };
                                timeout = getData.length;
                                // handleGroupByViewsNTables(getData);
                                
                                parentScope.$emit('postViewsNTablesNColumnsE', dataSourceNTablesNViewsNColumns);





                            }
                            setTimeout(() => { uibModalInstance.dismiss('cancel'); $('.rmt.overlay').attr('style', 'display:none;'); }, timeout);


                        }
                    });

                }
                else {
                    toastr.error("Please select atleast one view or table");
                }


                // uibModalInstance.dismiss('cancel');
            };
            vm.tablesNViewsTree = $.extend(true, [], pvm.tablesNViewsTree)
            vm.customOpts = {
                defaultSelectedState: false,
                useCheckboxes: true,
                validate: true,
                twistieExpandedTpl: '<i class="fa fa-caret-down"></i>',
                twistieCollapsedTpl: '<i class="fa fa-caret-right"></i>',
                twistieLeafTpl: ' ',
                expandToDepth: 0
            };

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
            initialization: function (pvm, vm, widgetIndex, scope, compile, toastr, rootScope, parentScope, uibModalInstance, uiGridConstants) {
                handleInitialization(pvm, vm, widgetIndex, scope, compile, toastr, rootScope, parentScope, uibModalInstance, uiGridConstants);

            }
        };
    }();
    return addRemoteDataSourceServicesHandler;

    //#endregion
});

//#endregion